using CollectionManagementLib.Composite;
using CollectionManagementLib.Interfaces;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CollectionManagementLib
{
    public class CollectionManager : IManager
    {
        private IHashInfoHandler _hashInfoHandler;
        private IHashCheck _hashChecker;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FolderItem RootFolder { get; set; }

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Objects
        };

        public CollectionManager(IHashCheck hashChecker, IHashInfoHandler hashInfoHandler)
        {
            _hashChecker = hashChecker;
            _hashInfoHandler = hashInfoHandler;
        }

        public void GenerateStructure()
        {
            if (RootFolder == null)
            {
                _logger.Error($"Requested directory for scan is not set! Aborting...");
                return;
            }

            RootFolder.Scan(true);
        }

        public string GenerateHash()
        {
            return GenerateHash(this.RootFolder, true);
        }

        private string GenerateHash(BaseComposite item, bool recursive = false, string innerFolderPathSegment = "")
        {
            var response = string.Empty;
            innerFolderPathSegment = string.IsNullOrEmpty(innerFolderPathSegment) ? string.Empty : $"{innerFolderPathSegment}{Path.DirectorySeparatorChar}";

            if (!item.Exists) return null;

            if (item is FileItem)
                response = $"{innerFolderPathSegment}{item.Name}   {_hashChecker.GetHash(item.FullPath)}";

            if (item is FolderItem)
            {
                foreach (var child in item.Children.Where(c => c is FileItem))
                    response += $"{GenerateHash(child, recursive)}\n";

                if (recursive)
                    foreach (var child in item.Children.Where(c => c is FolderItem))
                        response += $"{GenerateHash(child, recursive, innerFolderPathSegment + child.Name)}{Environment.NewLine}";
            }
            return response;
        }

        public bool Validate()
        {
            var sfvFiles = this.RootFolder.Search("*.sfv", true);
            var validityCheck = true;

            foreach (var sfvFile in sfvFiles)
            {
                var parentFolder = sfvFile.Parent;

                if (!_hashInfoHandler.ValidateFile(sfvFile.FullPath))
                {
                    _logger.Warn($"SFV file in {sfvFile.FullPath} is invalid! Skipping....");
                    continue;
                }

                var sfvInfo = _hashInfoHandler.Parse(sfvFile.FullPath);
                foreach (var sfvFileInfo in sfvInfo.Keys)
                {
                    var properpath = Path.Combine(sfvFile.Parent.FullPath, sfvFileInfo);

                    if (!_hashChecker.Validate(properpath, sfvInfo[sfvFileInfo]))
                    {
                        _logger.Warn($@"File {sfvFileInfo} has invalid CRC according to sfv file {sfvFile.FullPath}.
Expected CRC: {sfvInfo[sfvFileInfo]} | Calculated CRC: {_hashChecker.GetHash(sfvFileInfo)}");
                        validityCheck = false;
                    }
                }
            }

            return validityCheck;
        }

        public void Refresh()
        {
            this.RootFolder.Refresh(true);
        }

        public string SerializeStructure()
        {
            return JsonConvert.SerializeObject(this.RootFolder, _serializerSettings);
        }

        public bool DeserializeStructure(string input)
        {
            try
            {
                this.RootFolder = JsonConvert.DeserializeObject<FolderItem>(input, _serializerSettings);
            }
            catch (Exception ex)
            {
                this.RootFolder = null;
                _logger.Error("CollectionManager deserialization error!", ex);
            }

            return this.RootFolder != null;
        }

        public Task<string> GenerateHashAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAsync()
        {
            throw new NotImplementedException();
        }
    }
}