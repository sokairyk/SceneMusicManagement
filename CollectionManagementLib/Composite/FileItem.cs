using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CollectionManagementLib.Composite
{
    public class FileItem : BaseComposite
    {
        public override bool IsDirectory => false;
        private string _extension;
        public string Extension
        {
            get
            {
                if (string.IsNullOrEmpty(_extension))
                    _extension = Name.Split(".", StringSplitOptions.RemoveEmptyEntries).Last();
                return _extension;
            }
        }
        private static readonly FileExtensionContentTypeProvider _contentProvider = new FileExtensionContentTypeProvider();

        private string _mimeType;
        public string MimeType
        {
            get
            {
                if (_mimeType != null) return _mimeType;

                string mimeType;
                if (_contentProvider.TryGetContentType(FullPath, out mimeType))
                    _mimeType = mimeType;

                return mimeType;
            }
        }

        private bool? _exists = null;
        public override bool Exists
        {
            get
            {
                _exists = _exists ?? File.Exists(FullPath);
                return _exists.Value;
            }
        }

        public FileItem(string fullpath, BaseComposite parent) : base(fullpath, parent)
        {

        }
    }
}
