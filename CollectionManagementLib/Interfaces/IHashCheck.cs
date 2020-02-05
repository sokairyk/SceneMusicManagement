using CollectionManagementLib.Helpers;
using System.Threading.Tasks;

namespace CollectionManagementLib.Interfaces
{
    public interface IHashCheck
    {
        HashType HashAlgorithm { get; }
        string GetHash(string filepath);
        Task<string> GetHashAsync(string filepath);
        bool Validate(string filepath, string hashValue);
        Task<bool> ValidateAsync(string filepath, string hashValue);
    }
}
