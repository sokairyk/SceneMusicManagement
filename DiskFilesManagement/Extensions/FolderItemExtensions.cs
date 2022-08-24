using DiskFilesManagement.FileStructureModels;
using Microsoft.Extensions.Logging;
using Sokairyk.Base.CustomAttributes;
using Sokairyk.Hashing;

namespace DiskFilesManagement.Extensions
{
    public static class FolderItemExtensions
    {
        public static async Task<string> GenerateHashAsync(this FolderItem folderItem, IHashChecker hashChecker, bool recursive = false)
        {
            return await GenerateHashAsync(folderItem, hashChecker, recursive, "");
        }

        private static async Task<string> GenerateHashAsync(BaseComposite item, IHashChecker hashChecker, bool recursive = false, string innerFolderPathSegment = "")
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

        public static async Task<bool> ValidateAsync(this FolderItem folderItem, IHashChecker hashChecker, IHashInfoHandler hashInfoHandler, ILogger logger = null)
        {
            var hashFileExtension = hashChecker.HashAlgorithm.StringValue();
            var hashContentFiles = folderItem.Search($"*.{hashFileExtension}", true);
            var validityCheck = true;

            foreach (var hashContentFile in hashContentFiles)
            {
                var parentFolder = hashContentFile.Parent;

                if (!hashInfoHandler.ValidateFile(hashContentFile.FullPath))
                {
                    logger?.LogWarning($"{hashFileExtension.ToUpper()} file in {hashContentFile.FullPath} is invalid! Skipping....");
                    continue;
                }

                var hashInfo = hashInfoHandler.Parse(hashContentFile.FullPath);
                foreach (var hashFileInfo in hashInfo.Keys)
                {
                    var properpath = Path.Combine(hashContentFile.Parent.FullPath, hashFileInfo);

                    if (!await hashChecker.ValidateAsync(properpath, hashInfo[hashFileInfo]))
                    {
                        logger?.LogWarning($@"File {hashFileInfo} has invalid hash according to {hashFileExtension} file {hashContentFile.FullPath}.
Expected {hashChecker.HashAlgorithm}: {hashInfo[hashFileInfo]} | Calculated {hashChecker.HashAlgorithm}: {await hashChecker.GetHashAsync(hashFileInfo)}");
                        validityCheck = false;
                    }
                }
            }

            return validityCheck;
        }
    }
}