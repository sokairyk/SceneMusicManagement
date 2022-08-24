using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace DiskFilesManagement.FileStructureModels
{
    public class FileItem : BaseComposite
    {
        private bool? _exists = null;
        private string _mimeType;
        private string _extension;
        private readonly ILogger _logger;
        private static readonly FileExtensionContentTypeProvider _contentProvider = new FileExtensionContentTypeProvider();

        public override bool IsDirectory => false;
        
        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(_extension))
                    _extension = Name.Split(".", StringSplitOptions.RemoveEmptyEntries).Last();
                return _extension;
            }
        }
        
        public string MimeType
        {
            get
            {
                if (_mimeType != null) return _mimeType;

                if (_contentProvider.TryGetContentType(FullPath, out var mimeType))
                    _mimeType = mimeType;

                return mimeType;
            }
        }

        public override bool Exists
        {
            get
            {
                _exists = _exists ?? File.Exists(FullPath);
                return _exists.Value;
            }
        }

        public FileItem(string fullpath, BaseComposite parent, ILogger logger) : base(fullpath, parent, logger)
        {
            _logger = logger;
        }

        public new void Refresh(bool recursive = false)
        {
            _exists = null;
            _extension = null;
            _mimeType = null;
            (this as BaseComposite).Refresh(recursive);
        }
    }
}
