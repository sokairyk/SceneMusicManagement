using MusicManagementLib.DAL.ClementineDTO;
using SokairykFramework.Repository;
using System.Linq;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var di = new DependencyInjectionManager();
            var clementineService = di.ResolveInterface<IDataService>("Clementine");

            var song = clementineService.Repository.GetAll<ClementineSong>().Where(s => s.Title.Contains("ein")).SingleOrDefault();

            //var collectionManager = new ManagerFactory().GetManager();
            //collectionManager.RootFolder = new FolderItem(@"D:\Sokairyk\SceneMusicManagement\Data", null);
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
        }
    }
}