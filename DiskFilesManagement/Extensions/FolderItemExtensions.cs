using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiskFilesManagement.FileStructure;
using SokairykFramework.Hashing;
using SokairykFramework.Logger;

namespace DiskFilesManagement.Extensions
{
    public static class FolderItemExtensions
    {
        public static async Task<string> GenerateHashAsync(this FolderItem folderItem, IHashCheck hashChecker, bool recursive = false)
        {
            return await GenerateHashAsync(folderItem, hashChecker, recursive, "");
        }

        private static async Task<string> GenerateHashAsync(BaseComposite item, IHashCheck hashChecker, bool recursive = false, string innerFolderPathSegment = "")
        {
            var response = string.Empty;
            innerFolderPathSegment = string.IsNullOrEmpty(innerFolderPathSegment) ? string.Empty : $"{innerFolderPathSegment}{Path.DirectorySeparatorChar}";

            if (!item.Exists) return null;

            if (item is FileItem)
                response = $"{innerFolderPathSegment}{item.Name}   {await hashChecker.GetHashAsync(item.FullPath)}";

            if (item is FolderItem)
            {
                foreach (var child in item.Children.Where(c => c is FileItem))
                    response += $"{await GenerateHashAsync(child, hashChecker, recursive)}{Environment.NewLine}";

                if (recursive)
                    foreach (var child in item.Children.Where(c => c is FolderItem))
                        response += $"{await GenerateHashAsync(child, hashChecker, recursive, innerFolderPathSegment + child.Name)}{Environment.NewLine}";
            }

            return response;
        }

        public static async Task<bool> ValidateAsync(this FolderItem folderItem, IHashCheck hashChecker, IHashInfoHandler hashInfoHandler, ILogger logger = null)
        {
            var sfvFiles = folderItem.Search("*.sfv", true);
            var validityCheck = true;

            foreach (var sfvFile in sfvFiles)
            {
                var parentFolder = sfvFile.Parent;

                if (!hashInfoHandler.ValidateFile(sfvFile.FullPath))
                {
                    logger?.LogWarning($"SFV file in {sfvFile.FullPath} is invalid! Skipping....");
                    continue;
                }

                var sfvInfo = hashInfoHandler.Parse(sfvFile.FullPath);
                foreach (var sfvFileInfo in sfvInfo.Keys)
                {
                    var properpath = Path.Combine(sfvFile.Parent.FullPath, sfvFileInfo);

                    if (!await hashChecker.ValidateAsync(properpath, sfvInfo[sfvFileInfo]))
                    {
                        logger?.LogWarning($@"File {sfvFileInfo} has invalid CRC according to sfv file {sfvFile.FullPath}.
Expected CRC: {sfvInfo[sfvFileInfo]} | Calculated CRC: {await hashChecker.GetHashAsync(sfvFileInfo)}");
                        validityCheck = false;
                    }
                }
            }

            return validityCheck;
        }
    }
}