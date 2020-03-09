using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SokairykFramework.Encryption
{
    public interface IEncrypt
    {
        byte[] Encypt(Stream inputStream, string password);
        byte[] Decrypt(Stream inputStream, string password);
    }
}
