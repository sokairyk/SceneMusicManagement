using Microsoft.Extensions.Logging;

namespace Sokairyk.Hashing
{
    public class HashCheckCRC32 : IHashChecker
    {
        private const int CHUNK_SIZE_IN_BYTES = 10000000;
        private static byte[] _readBuffer = new byte[CHUNK_SIZE_IN_BYTES];
        private ILogger _logger;

        public HashTypeEnum HashAlgorithm => HashTypeEnum.CRC32;

        public HashCheckCRC32(ILogger<HashCheckCRC32> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetHashAsync(string filepath)
        {
            if (!File.Exists(filepath))
            {
                _logger.LogWarning($"Requested file in {filepath} for CRC verification was not found.");
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
                    var bytesRead = await fileStream.ReadAsync(_readBuffer, 0, CHUNK_SIZE_IN_BYTES);

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

        public async Task<bool> ValidateAsync(string filepath, string hashValue)
        {
            return await GetHashAsync(filepath) == hashValue?.ToLower()?.Trim();
        }
    }
}