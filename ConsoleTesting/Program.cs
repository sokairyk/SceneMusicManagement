using MusicManagementLib.DAL.DTO;
using MusicManagementLib.Repository;
using SokairykFramework.Configuration;
using System;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var res = new ConfigurationManager().GetApplicationSetting("CLEMENTINE_DB_PATH");

            var aaa = new ClementineUnitOfWork();

            var uow = new ClementineRepository<ClementineSong>(aaa);
            aaa.BeginTransaction();

            var test = uow.GetAll();

            //var collectionManager = new ManagerFactory().GetManager();
            //collectionManager.RootFolder = new FolderItem(@"G:\Downloads\Torrents\Hitman 2\Hitman.2-CPY", null);
            //collectionManager.GenerateStructure();
            //var counter = new Stopwatch();
            //counter.Start();
            //var res = collectionManager.Validate();
            //counter.Stop();
            //TimeSpan t = TimeSpan.FromMilliseconds(counter.ElapsedMilliseconds);
            //string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            //                        t.Hours,
            //                        t.Minutes,
            //                        t.Seconds,
            //                        t.Milliseconds);
            //Console.WriteLine($"File is {(res ? "" : "in")}valid. Time elapsed: {answer}");
            Console.ReadLine();
        }
    }
}