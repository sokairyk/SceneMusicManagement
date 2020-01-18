using CollectionManagementLib.Composite;
using CollectionManagementLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManagementLib
{
    public class CollectionManager
    {
        private IHashCheck _hashChecker;
        public CollectionManager(IHashCheck hashChecker)
        {
            _hashChecker = hashChecker;
        }

        public void GenerateStructure(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Logging.Instance.Logger.Error($"Requested directory for scan: {folderPath} was not found");
                return;
            }

            var test = new FolderItem(folderPath, null);
            test.ScanCurrentPath(true);
        }
    }
}