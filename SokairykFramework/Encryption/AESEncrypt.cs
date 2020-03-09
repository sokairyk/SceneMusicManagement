using System.IO;

namespace SokairykFramework.Encryption
{
    public class AESEncrypt : IEncrypt
    {
        public byte[] Encypt(Stream inputStream, string password)
        {
            if (inputStream == null || !inputStream.CanRead || !inputStream.CanSeek) return null;

            using (var outputStream = new MemoryStream())
            {
                SharpAESCrypt.SharpAESCrypt.Encrypt(password, inputStream, outputStream);
                return outputStream.ToArray();
            }
        }

        public byte[] Decrypt(Stream inputStream, string password)
        {
            if (inputStream == null || !inputStream.CanRead || !inputStream.CanSeek) return null;

            using (var outputStream = new MemoryStream())
            {
                SharpAESCrypt.SharpAESCrypt.Decrypt(password, inputStream, outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
