using CollectionManagementLib.FileStructure;
using Newtonsoft.Json;
using SokairykFramework.Hashing;
using SokairykFramework.Logger;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManagementLib.Manager
{
    public class CollectionManager : IManager
    {
        private IHashInfoHandler _hashInfoHandler;
        private IHashCheck _hashChecker;
        private ILogger _logger;

        public FolderItem RootFolder { get; set; }

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Objects
        };

        public CollectionManager(IHashCheck hashChecker, IHashInfoHandler hashInfoHandler, ILogger logger)
        {
            _hashChecker = hashChecker;
            _hashInfoHandler = hashInfoHandler;
            _logger = logger;
        }

        public void GenerateStructure()
        {
            if (RootFolder == null)
            {
                _logger.LogWarning($"Requested directory for scan is not set! Aborting...");
                return;
            }

            RootFolder.Scan(true);
        }

        public async Task<string> GenerateHashAsync()
        {
            return await GenerateHashAsync(RootFolder, true);
        }

        private async Task<string> GenerateHashAsync(BaseComposite item, bool recursive = false, string innerFolderPathSegment = "")
        {
            var response = string.Empty;
            innerFolderPathSegment = string.IsNullOrEmpty(innerFolderPathSegment) ? string.Empty : $"{innerFolderPathSegment}{Path.DirectorySeparatorChar}";

            if (!item.Exists) return null;

            if (item is FileItem)
                response = $"{innerFolderPathSegment}{item.Name}   {await _hashChecker.GetHashAsync(item.FullPath)}";

            if (item is FolderItem)
            {
                foreach (var child in item.Children.Where(c => c is FileItem))
                    response += $"{await GenerateHashAsync(child, recursive)}\n";

                if (recursive)
                    foreach (var child in item.Children.Where(c => c is FolderItem))
                        response += $"{await GenerateHashAsync(child, recursive, innerFolderPathSegment + child.Name)}{Environment.NewLine}";
            }
            return response;
        }

        public async Task<bool> ValidateAsync()
        {
            var sfvFiles = this.RootFolder.Search("*.sfv", true);
            var validityCheck = true;

            foreach (var sfvFile in sfvFiles)
            {
                var parentFolder = sfvFile.Parent;

                if (!_hashInfoHandler.ValidateFile(sfvFile.FullPath))
                {
                    _logger.LogWarning($"SFV file in {sfvFile.FullPath} is invalid! Skipping....");
                    continue;
                }

                var sfvInfo = _hashInfoHandler.Parse(sfvFile.FullPath);
                foreach (var sfvFileInfo in sfvInfo.Keys)
                {
                    var properpath = Path.Combine(sfvFile.Parent.FullPath, sfvFileInfo);

                    if (!await _hashChecker.ValidateAsync(properpath, sfvInfo[sfvFileInfo]))
                    {
                        _logger.LogWarning($@"File {sfvFileInfo} has invalid CRC according to sfv file {sfvFile.FullPath}.
Expected CRC: {sfvInfo[sfvFileInfo]} | Calculated CRC: {await _hashChecker.GetHashAsync(sfvFileInfo)}");
                        validityCheck = false;
                    }
                }
            }

            return validityCheck;
        }

        public void Refresh()
        {
            RootFolder.Refresh(true);
        }

        public string SerializeStructure()
        {
            return JsonConvert.SerializeObject(RootFolder, _serializerSettings);
        }

        public bool DeserializeStructure(string input)
        {
            try
            {
                RootFolder = JsonConvert.DeserializeObject<FolderItem>(input, _serializerSettings);
            }
            catch (Exception ex)
            {
                RootFolder = null;
                _logger.LogError("CollectionManager deserialization error!", ex);
            }

            return RootFolder != null;
        }
    }
}