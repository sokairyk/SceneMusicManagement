using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CollectionManagementLib.Composite
{
    public abstract class BaseComposite
    {
        public readonly string Name;
        public readonly string FullPath;
        public virtual bool IsDirectory => true;
        public readonly BaseComposite Parent;
        private List<BaseComposite> _children = new List<BaseComposite>();
        public IEnumerable<BaseComposite> Children { get => _children.AsReadOnly(); }
        public abstract bool Exists { get; }

        public BaseComposite(string fullpath, BaseComposite parent)
        {
            if (fullpath == null)
                throw new NullReferenceException("ERROR: Cannot initialize a BaseComposite instance with a null path");

            Name = fullpath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).Last();
            FullPath = fullpath;
            Parent = parent;
        }

        public void ScanCurrentPath(bool recursive = false)
        {
            if (!IsDirectory) return;

            if (!Directory.Exists(FullPath))
            {
                Logging.Instance.Logger.Warn($"WARNING: Directory \"{FullPath}\" was not found!");
                return;
            }

            foreach (var systemEntry in Directory.GetFileSystemEntries(FullPath))
            {
                var entryAttributes = File.GetAttributes(systemEntry);

                //Check for duplicates, warn and skip
                if (_children.Select(c => c.FullPath).Contains(systemEntry)
                    && ((entryAttributes == FileAttributes.Directory && _children.SingleOrDefault(c => c is FolderItem) != null)
                        ||
                        (entryAttributes == FileAttributes.Archive && _children.SingleOrDefault(c => c is FileItem) != null)))
                {
                    Logging.Instance.Logger.Warn($"WARNING: Directory \"{FullPath}\" already contains a {(entryAttributes == FileAttributes.Directory ? "folder" : "file")} child entry: \"{systemEntry}\". Skipping...");
                    continue;
                }

                switch (entryAttributes)
                {
                    case FileAttributes.Directory:
                        _children.Add(new FolderItem(systemEntry, this));
                        break;
                    case FileAttributes.Normal:
                        _children.Add(new FileItem(systemEntry, this));
                        break;
                    default:
                        Logging.Instance.Logger.Warn($"WARNING: System item \"{systemEntry}\" is not supported. Skipping...");
                        break;
                }
            }

            if (recursive) _children.AsParallel().ForAll(c => c.ScanCurrentPath(true));
        }
    }
}
