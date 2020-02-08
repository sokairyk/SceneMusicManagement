﻿using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private static readonly char[] _invalidFilenameChars = CustomInvalidChars();
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseComposite(string fullpath, BaseComposite parent, List<BaseComposite> children = null)
        {
            if (fullpath == null)
                throw new NullReferenceException("Cannot initialize a BaseComposite instance with a null path");

            Name = fullpath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).Last();
            FullPath = fullpath;
            Parent = parent;
            _children = children ?? new List<BaseComposite>();
        }

        public void Scan(bool recursive = false)
        {
            Scanned = true;
            ScannedOn = DateTime.Now;

            if (!Exists)
            {
                _logger.Warn($"{FullPath} was not found!");
                return;
            }

            if (!IsDirectory) return;

            var folderNotScannedOrEmpty = _children.Count == 0;
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
                    if (folderNotScannedOrEmpty)
                        _logger.Warn($"Directory \"{Name}\" already contains a {(entryAttributes == FileAttributes.Directory ? "folder" : "file")} child entry: \"{systemEntry}\". Skipping...");

                    continue;
                }


                if ((entryAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                    _children.Add(new FolderItem(systemEntry, this));
                else if ((entryAttributes & FileAttributes.Normal) == FileAttributes.Normal
                        ||
                        (entryAttributes & FileAttributes.Archive) == FileAttributes.Archive)
                    _children.Add(new FileItem(systemEntry, this));
                else
                        _logger.Warn($"System item type \"{systemEntry}\" found in \"{FullPath}\" is not supported. Skipping...");

            }

            Scanned = true;
            ScannedOn = DateTime.Now;

            if (recursive) Children.AsParallel().ForAll(c => c.Scan(true));
        }

        public virtual void Refresh(bool recursive = false)
        {
            if (!Exists)
            {
                _children.Clear();
                Parent?._children?.Remove(this);
                return;
            }

            if (recursive) _children.AsParallel().ForAll(c => c.Refresh());
        }

        public HashSet<BaseComposite> Search(string pattern, bool recursive = false)
        {
            var results = new HashSet<BaseComposite>();
            var searchTerms = pattern.Split(" \t", StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in searchTerms)
            {
                //Sanitize search input and apply regex match only for wildcards
                var processedTerm = string.Join("", term.Split(_invalidFilenameChars, StringSplitOptions.RemoveEmptyEntries));
                processedTerm = string.Join("[0-9a-zA-Z]*", processedTerm.Split("*", StringSplitOptions.None).Select(p => Regex.Escape(p)));

                var regexPattern = new Regex(processedTerm, RegexOptions.Compiled & RegexOptions.IgnoreCase);
                Search(regexPattern, this, null, recursive).Select(r => results.Add(r));
            }

            return results;
        }

        protected static HashSet<BaseComposite> Search(Regex pattern, BaseComposite item, HashSet<BaseComposite> results, bool recursive)
        {
            //Initialize if needed
            results = results ?? new HashSet<BaseComposite>();

            if (!item.IsDirectory && pattern.IsMatch(item.Name))
                results.Add(item);
            else
            {
                item.Children.Where(c => pattern.IsMatch(c.Name)).Select(r => results.Add(r));

                if (recursive)
                    item.Children.SelectMany(c => Search(pattern, c, null, recursive)).Select(r => results.Add(r));
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
