using CollectionManagementLib.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManagementLib.Composite
{
    public class FolderItem : BaseComposite
    {
        private bool? _exists = null;
        public override bool Exists
        {
            get
            {
                _exists = _exists ?? Directory.Exists(FullPath);
                return _exists.Value;
            }
        }

        private int? _totalItemsContained = null;
        public int TotalItemsContained
        {
            get
            {
                if (_totalItemsContained == null)
                    _totalItemsContained = Directory.EnumerateFiles(FullPath, "*.*", SearchOption.AllDirectories).Count();

                return _totalItemsContained.Value;
            }
        }

        public FolderItem(string fullPath, BaseComposite parent) : base(fullPath, parent)
        {

        }

        public new void Refresh(bool recursive = false)
        {
            this._exists = null;
            this._totalItemsContained = null;

            (this as BaseComposite).Refresh(recursive);
        }
    }
}
