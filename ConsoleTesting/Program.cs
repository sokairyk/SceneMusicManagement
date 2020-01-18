using CollectionManagementLib;
using CollectionManagementLib.Factory;
using System;
using System.IO;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var collectionManager = new ManagerFactory().GetManager();
            collectionManager.GenerateStructure(@"D:\Sok\SceneMusicManagement-master\CollectionManagementLib");
        }
    }
}