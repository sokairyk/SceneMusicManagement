using SokairykFramework.Common;
using System.Threading.Tasks;

namespace SokairykFramework.Hashing
{
    public interface IHashCheck
    {
        HashType HashAlgorithm { get; }
        Task<string> GetHashAsync(string filepath);
        Task<bool> ValidateAsync(string filepath, string hashValue);
    }
}
