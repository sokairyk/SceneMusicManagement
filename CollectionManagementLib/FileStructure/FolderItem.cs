using System.IO;
using System.Linq;

namespace CollectionManagementLib.FileStructure
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
            _exists = null;
            _totalItemsContained = null;

            (this as BaseComposite).Refresh(recursive);
        }
    }
}