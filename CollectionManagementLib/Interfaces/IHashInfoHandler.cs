using System.Collections.Generic;

namespace CollectionManagementLib.Interfaces
{
    public interface IHashInfoHandler
    {
        string HashInfoExtension { get; }
        bool ValidateFile(string filepath);
        bool ValidateLine(string line);
        Dictionary<string, string> Parse(string filepath);
    }
}
