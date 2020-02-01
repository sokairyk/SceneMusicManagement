using CollectionManagementLib.Helpers;
using CollectionManagementLib.Interfaces;
using System.IO;

namespace CollectionManagementLib
{
    public class HashCheckCRC : IHashCheck
    {
        public HashType HashAlgorithm => HashType.CRC32;

        public string GetHash(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Logging.Instance.Logger.Warn($"Requested file in {filepath} for CRC verification was not found.");
                return null;
            }

            var fileBytes = File.ReadAllBytes(filepath);

            var crc32 = CRC32.Compute(fileBytes);
            return $"{crc32:X}".ToLower().PadLeft(8, '0');
        }

        public bool Validate(string filepath, string hashValue)
        {
            return GetHash(filepath) == hashValue?.ToLower()?.Trim();
        }
    }
}