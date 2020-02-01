using CollectionManagementLib.Composite;
using CollectionManagementLib.Factories;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var collectionManager = new ManagerFactory().GetManager();
            collectionManager.RootFolder = new FolderItem(@"D:\Sokairyk\SceneMusicManagement\Data", null);
            collectionManager.GenerateStructure();

        }
    }
}