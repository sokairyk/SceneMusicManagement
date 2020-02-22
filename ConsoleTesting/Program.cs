using CollectionManagementLib.FileStructure;
using CollectionManagementLib.Manager;
using MusicManagementLib.DAL.ClementineDTO;
using MusicManagementLib.Domain;
using SokairykFramework.AutoMapper;
using SokairykFramework.Configuration;
using SokairykFramework.Extensions;
using SokairykFramework.Repository;
using System;
using System.Linq;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {


            var config = AutoMapperExtensions.CreateConfig();
            var mapper = config.CreateMapper();
            

            var song = new Song { Title = "Loud and Clear", Artist = new Artist { Name = "Cranberries" }, Album = new Album { Name = "Bury the Hatchet" } };
            var clemSong = mapper.Map<Song, ClementineSong>(song);


            var di = new DependencyInjectionManager();

            var cf = di.ResolveInterface<IConfigurationManager>();
            var tst = cf.GetApplicationSetting("CLEMENTINE_DB_PATH");

            using (var repo = di.ResolveInterface<IRepositoryWithUnitOfWork>("Clementine"))
            {
                var test = repo.GetAll<ClementineSong>().Where(s => s.Title.Contains("ein")).SingleOrDefault();
            }

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