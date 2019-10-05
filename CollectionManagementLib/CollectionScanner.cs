using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectionManagementLib
{
    public class CollectionScanner
    {
        public bool VerboseOutput { get; set; }

        public void GenerateStructure(string folderPath)
        {
            if(!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Directory path: '{folderPath}' was not found");
        }

        private void ScanDirectory(string folderPath)
        {
            
        }
    }

    public class CollectionFolder
    {
        public string FolderPath { get; set; }
        public string FolderName { get; set; }
        public IEnumerable<CollectionFolder> ContainedFolders { get; set; }
        public IEnumerable<CollectionFile> ContainedFiles { get; set; }
        public bool IsScanned { get; set; }
        public bool ContainsSfvFile { get; set; }

        public CollectionFolder(DirectoryInfo folderInfo)
        {
            FolderPath = folderInfo.FullName;
            FolderName = folderInfo.Name;
            ContainedFolders = new List<CollectionFolder>();
            ContainedFiles = new List<CollectionFile>();
            ScanFolder();
        }
        
        public void ScanFolder()
        {
            foreach (var folder in Directory.EnumerateDirectories(FolderPath))
            {
                ContainedFolders.Append(new CollectionFolder(new DirectoryInfo(folder)));
            }
            foreach (var file in Directory.EnumerateFiles(FolderPath))
            {
                ContainedFiles.Append(new CollectionFile(new FileInfo(file)));
            }
        }
    }

    public class CollectionFile
    {
        public CollectionFile(FileInfo fileInfo)
        {
            Filename = fileInfo.Name;
            Filepath = fileInfo.FullName;
        }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string Extension => Filename.Split(".", StringSplitOptions.RemoveEmptyEntries).Last();
        private string _crc32 = null;

        public string CRC32
        {
            get { return _crc32 ?? (_crc32 = $"{CollectionManagementLib.CRC32.Compute(File.ReadAllBytes(this.Filepath)):X}"); }
        }

        public bool Exists => File.Exists(Filepath);
    }
}