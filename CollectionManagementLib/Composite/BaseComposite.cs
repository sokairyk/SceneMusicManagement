using CollectionManagementLib.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CollectionManagementLib.Composite
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseComposite
    {
        public readonly string Name;
        [JsonProperty]
        public readonly string FullPath;
        public virtual bool IsDirectory => true;
        public abstract bool Exists { get; }
        [JsonProperty]
        public bool Scanned { get; set; }
        [JsonProperty]
        public DateTime? ScannedOn { get; set; }
        [JsonProperty]
        public readonly BaseComposite Parent;
        [JsonProperty]
        protected List<BaseComposite> _children { get; set; }
        public IEnumerable<BaseComposite> Children { get => _children?.AsReadOnly(); }

        private static readonly char[] _invalidFilenameChars = BaseComposite.CustomInvalidChars();

        public BaseComposite(string fullpath, BaseComposite parent, List<BaseComposite> children = null)
        {
            if (fullpath == null)
                throw new NullReferenceException("ERROR: Cannot initialize a BaseComposite instance with a null path");

            Name = fullpath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).Last();
            FullPath = fullpath;
            Parent = parent;
            _children = children ?? new List<BaseComposite>();
        }

        public void Scan(bool recursive = false)
        {
            if (!IsDirectory)
            {
                Scanned = true;
                ScannedOn = DateTime.Now;
                return;
            }

            if (!Directory.Exists(FullPath))
            {
                Logging.Instance.Logger.Warn($"WARNING: Directory \"{FullPath}\" was not found!");
                return;
            }

            foreach (var systemEntry in Directory.GetFileSystemEntries(FullPath))
            {
                var entryAttributes = File.GetAttributes(systemEntry);

                //Check for duplicates, warn if necessary and skip
                var sameFullPathChildren = Children.Where(c => c.FullPath == systemEntry);
                if (sameFullPathChildren.Count() > 0
                    && ((entryAttributes == FileAttributes.Directory
                         && sameFullPathChildren.SingleOrDefault(c => c.FullPath == systemEntry && c.IsDirectory) != null)
                      ||
                        (entryAttributes != FileAttributes.Directory
                         && sameFullPathChildren.SingleOrDefault(c => c.FullPath == systemEntry && !c.IsDirectory) != null)))
                {
                    if (!Scanned)
                        Logging.Instance.Logger.Warn($"WARNING: Directory \"{FullPath}\" already contains a {(entryAttributes == FileAttributes.Directory ? "folder" : "file")} child entry: \"{systemEntry}\". Skipping...");

                    continue;
                }


                if ((entryAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    _children.Add(new FolderItem(systemEntry, this));
                else if ((entryAttributes & FileAttributes.Normal) == FileAttributes.Normal
                        ||
                        (entryAttributes & FileAttributes.Archive) == FileAttributes.Archive)
                    _children.Add(new FileItem(systemEntry, this));
                else
                    Logging.Instance.Logger.Warn($"WARNING: System item \"{systemEntry}\" is not supported. Skipping...");

            }

            if (recursive) Children.AsParallel().ForAll(c => c.Scan(true));
        }

        public virtual void Refresh(bool recursive = false)
        {
            if (!this.Exists)
            {
                _children.Clear();
                this.Parent?._children?.Remove(this);
                return;
            }

            if (recursive) _children.AsParallel().ForAll(c => c.Refresh());
        }

        public IEnumerable<BaseComposite> Search(string pattern, bool recursive = false)
        {
            //Sanitize search input and apply regex match only for wildcards
            pattern = string.Join("", pattern.Split(_invalidFilenameChars, StringSplitOptions.RemoveEmptyEntries));
            pattern = string.Join("[0-9a-zA-Z].", pattern.Split("*", StringSplitOptions.None).Select(p => Regex.Escape(p)));

            var regexPattern = new Regex(pattern, RegexOptions.Compiled & RegexOptions.IgnoreCase);
            var results = Search(regexPattern, this, null, recursive);

            return results;
        }

        protected static IEnumerable<BaseComposite> Search(Regex pattern, BaseComposite item, HashSet<BaseComposite> results, bool recursive)
        {
            //Initialize if needed
            results = results ?? new HashSet<BaseComposite>();

            if (!item.IsDirectory)
            {
                if (pattern.IsMatch(item.Name))
                    results.Add(item);
            }
            else
            {
                foreach (var match in item.Children.Where(c => pattern.IsMatch(c.Name)))
                    results.Add(match);
                if (recursive)
                    foreach (var match in item.Children.Flatten(c => Search(pattern, c, null, recursive)))
                        results.Add(match);
            }

            return results;
        }

        private static char[] CustomInvalidChars()
        {
            var invalidChars = Path.GetInvalidFileNameChars().ToList();
            //Allow * as a wildcard in search
            invalidChars.Remove('*');
            return invalidChars.ToArray();
        }
    }
}
