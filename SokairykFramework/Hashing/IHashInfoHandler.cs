using System.Collections.Generic;

namespace SokairykFramework.Hashing
{
    public interface IHashInfoHandler
    {
        string HashInfoExtension { get; }
        bool ValidateFile(string filepath);
        bool ValidateLine(string line);
        Dictionary<string, string> Parse(string filepath);
    }
}
