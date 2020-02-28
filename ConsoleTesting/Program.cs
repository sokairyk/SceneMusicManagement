using AutoMapper;
using CollectionManagementLib.FileStructure;
using CollectionManagementLib.Manager;
using MusicManagementLib.DAL.ClementineDTO;
using MusicManagementLib.Domain;
using MusicManagementLib.Helpers;
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
            var di = new DependencyInjectionManager();
            var mapper = di.ResolveInterface<IMapper>();
            
            var cf = di.ResolveInterface<IConfigurationManager>();

            using (var repo = di.ResolveInterface<IRepositoryWithUnitOfWork>("Clementine"))
            {
                var test = repo.GetAll<ClementineSong>().Where(s => s.Title.Contains("ein")).SingleOrDefault();
            }

            var sng = @"D:\Sokairyk\SceneMusicManagement\Data\Die_Krupps-The_Machinists_of_Joy-2CD-Limited_Edition-DE-2013-FWYH\103_die_krupps-risikofaktor.mp3";

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