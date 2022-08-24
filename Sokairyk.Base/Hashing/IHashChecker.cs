namespace Sokairyk.Hashing
{
    public interface IHashChecker
    {
        HashTypeEnum HashAlgorithm { get; }
        Task<string> GetHashAsync(string filepath);
        Task<bool> ValidateAsync(string filepath, string hashValue);
    }
}
