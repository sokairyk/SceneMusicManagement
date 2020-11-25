namespace DiskFilesManagement.Manager
{
    public interface IManager
    {
        void SetCollectionPath(string path);
        void GenerateStructure();
        string SerializeStructure();
        bool DeserializeStructure(string input);
        void Refresh();
    }
}
