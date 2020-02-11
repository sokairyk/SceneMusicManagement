using CollectionManagementLib.FileStructure;
using System.Threading.Tasks;

namespace CollectionManagementLib.Manager
{
    public interface IManager
    {
        FolderItem RootFolder { get; set; }
        void GenerateStructure();
        string SerializeStructure();
        bool DeserializeStructure(string input);
        void Refresh();
        Task<string> GenerateHashAsync();
        Task<bool> ValidateAsync();
    }
}
