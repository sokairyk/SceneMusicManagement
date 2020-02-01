using CollectionManagementLib.Composite;

namespace CollectionManagementLib.Interfaces
{
    public interface IManager
    {
        FolderItem RootFolder { get; set; }
        void GenerateStructure();
        string GenerateHash();
        bool Validate();
        void Refresh();
        string SerializeStructure();
        bool DeserializeStructure(string input);
    }
}
