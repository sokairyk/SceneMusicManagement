using System;
using DiskFilesManagement.FileStructure;
using Newtonsoft.Json;
using SokairykFramework.Hashing;
using SokairykFramework.Logger;

namespace DiskFilesManagement.Manager
{
    public class CollectionManager : IManager
    {
        private IHashInfoHandler _hashInfoHandler;
        private IHashCheck _hashChecker;
        private ILogger _logger;

        public FolderItem RootFolder { get; private set; }

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

        public void SetCollectionPath(string path)
        {
            RootFolder = new FolderItem(path, null);
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