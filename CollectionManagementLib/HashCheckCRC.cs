using CollectionManagementLib.Helpers;
using CollectionManagementLib.Interfaces;
using log4net;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Threading.Tasks;

namespace CollectionManagementLib
{
    public class HashCheckCRC : IHashCheck
    {
        private const int CHUNK_SIZE = 5000000;
        private static byte[] _readBuffer = new byte[CHUNK_SIZE];
        public HashType HashAlgorithm => HashType.CRC32;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string GetHash(string filepath)
        {
            return GetHashAsync(filepath).Result;
        }

        public async Task<string> GetHashAsync(string filepath)
        {
            if (!File.Exists(filepath))
            {
                _logger.Warn($"Requested file in {filepath} for CRC verification was not found.");
                return null;
            }

            var remainingBytesToRead = new FileInfo(filepath).Length;
            long offsetPosition = 0;
            uint? calculatedHash = null;

            using (var fileStream = new FileStream(filepath, FileMode.Open))
            {
                while (remainingBytesToRead > 0)
                {
                    fileStream.Position = offsetPosition;
                    var bytesRead = await fileStream.ReadAsync(_readBuffer, 0, CHUNK_SIZE);

                    if (bytesRead == 0) break;

                    var actualReadContent = new byte[bytesRead];
                    Array.Copy(_readBuffer, 0, actualReadContent, 0, bytesRead);

                    calculatedHash = CRC32.CalculateHash(actualReadContent, calculatedHash);

                    remainingBytesToRead -= bytesRead;
                    offsetPosition += bytesRead;
                }
            }

            var crc32 = ~calculatedHash;
            return $"{crc32:X}".ToLower().PadLeft(8, '0');
        }

        public bool Validate(string filepath, string hashValue)
        {
            return ValidateAsync(filepath, hashValue).Result;
        }

        public async Task<bool> ValidateAsync(string filepath, string hashValue)
        {
            return await GetHashAsync(filepath) == hashValue?.ToLower()?.Trim();
        }
    }
}