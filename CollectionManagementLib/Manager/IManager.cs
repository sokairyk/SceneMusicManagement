using CollectionManagementLib.FileStructure;
using System.Threading.Tasks;

namespace CollectionManagementLib.Manager
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
