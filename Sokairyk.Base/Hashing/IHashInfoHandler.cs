namespace Sokairyk.Hashing
{
    public interface IHashInfoHandler
    {
        string HashInfoExtension { get; }
        bool ValidateFile(string filepath);
        bool ValidateLine(string line);
        //Dictionary contains filename and hash key/value pairs
        Dictionary<string, string> Parse(string filepath);
    }
}
