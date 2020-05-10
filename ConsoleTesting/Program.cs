using CollectionManagementLib.Manager;
using MusicManagementLib.DAL.ClementineDTO;
using SokairykFramework.Configuration;
using SokairykFramework.Diagnostics;
using SokairykFramework.Repository;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var di = new DependencyInjectionManager();
            var clementineService = di.ResolveInterface<IDataService>("Clementine");
            var configurationManager = di.ResolveInterface<IConfigurationManager>();

            var collectionManager = di.ResolveInterface<IManager>();
            collectionManager.SetCollectionPath(configurationManager.GetApplicationSetting("MUSIC_SOURCE_PATH"));

            var totalMs = StatisticsHelper.GetExecutionTimeElapsedMilliseconds(() => {
                collectionManager.GenerateStructure();
            });
            
            TimeSpan t = TimeSpan.FromMilliseconds(totalMs);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            Console.WriteLine($"Generate structure {answer}");
            Console.ReadKey();
        }
    }
}