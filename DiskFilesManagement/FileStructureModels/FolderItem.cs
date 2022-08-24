using Microsoft.Extensions.Logging;

namespace DiskFilesManagement.FileStructureModels
{
    public class FolderItem : BaseComposite
    {
        private int? _totalItemsContained = null;
        private bool? _exists = null;
        private readonly ILogger _logger;
        public override bool Exists
        {
            get
            {
                _exists ??= Directory.Exists(FullPath);
                return _exists.Value;
            }
        }

        public int TotalItemsContained
        {
            get
            {
                if (_totalItemsContained == null)
                    _totalItemsContained = Directory.EnumerateFiles(FullPath, "*.*", SearchOption.AllDirectories).Count();

                return _totalItemsContained.Value;
            }
        }

        public FolderItem(string fullPath, BaseComposite parent, ILogger logger) : base(fullPath, parent, logger)
        {
            _logger = logger;
        }

        public new void Refresh(bool recursive = false)
        {
            _exists = null;
            _totalItemsContained = null;

            (this as BaseComposite).Refresh(recursive);
        }
    }
}
