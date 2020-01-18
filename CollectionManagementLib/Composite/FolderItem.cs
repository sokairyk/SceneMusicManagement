using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CollectionManagementLib.Composite
{
    public class FolderItem : BaseComposite
    {
        public FolderItem(string fullPath, BaseComposite parent) : base(fullPath, parent)
        {

        }

        private bool? _exists = null;
        public override bool Exists
        {
            get
            {
                _exists = _exists ?? Directory.Exists(FullPath);
                return _exists.Value;
            }
        }
    }
}
