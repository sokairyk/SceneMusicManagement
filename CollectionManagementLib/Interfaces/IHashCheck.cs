using CollectionManagementLib.Helpers;

namespace CollectionManagementLib.Interfaces
{
    public interface IHashCheck
    {
        HashType HashAlgorithm { get; }
        string GetHash(string filepath);
        bool Validate(string filepath, string hashValue);
    }
}
