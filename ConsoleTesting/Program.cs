using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiskFilesManagement.Manager;
using MusicPlayersDAL.DTO.Clementine;
using MusicPlayersDAL.Repositories;
using NHibernate.Linq;
using SokairykFramework.Configuration;
using SokairykFramework.Diagnostics;
using SokairykFramework.Extensions;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var di = new DependencyInjectionManager();
            var repo = di.ResolveInterface<IClementineRepository>();

            var songsQuery = repo.Repository.GetAll<ClementineSong>().Where(x => x.Album.ToLower().Contains("roboxai"));
            var check = songsQuery.AsQueryable().GetSQLStatement();
            var songs = songsQuery.ToList();

            var configurationManager = di.ResolveInterface<IConfigurationManager>();

            var collectionManager = di.ResolveInterface<IManager>();
            collectionManager.SetCollectionPath(configurationManager.GetApplicationSetting("MUSIC_SOURCE_PATH"));

            var totalMs = StatisticsHelper.GetExecutionTimeElapsedMilliseconds(() =>
            {
                collectionManager.GenerateStructure();
            });

            var t = TimeSpan.FromMilliseconds(totalMs);
            var answer = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
            Console.WriteLine($"Generate structure {answer}");
            Console.ReadKey();
        }
    }
}