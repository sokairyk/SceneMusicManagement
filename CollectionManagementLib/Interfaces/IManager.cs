using CollectionManagementLib.Composite;
using System.Threading.Tasks;

namespace CollectionManagementLib.Interfaces
{
    public interface IManager
    {
        FolderItem RootFolder { get; set; }
        void GenerateStructure();
        string SerializeStructure();
        bool DeserializeStructure(string input);
        void Refresh();
        string GenerateHash();
        Task<string> GenerateHashAsync();
        bool Validate();
        Task<bool> ValidateAsync();
    }
}
