using CollectionManagementLib;
using CollectionManagementLib.Composite;
using CollectionManagementLib.Factories;
using Microsoft.Extensions.Configuration;
using MusicManagementLib.Helpers;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var test2 = config.Providers.FirstOrDefault();

            IConfigurationProvider aaa = test2;

            aaa.TryGet("CLEMENTINE_DB_PATH", out var iii);

            var eeee = iii;

            var test =  ConfigurationManager.AppSettings["CLEMENTINE_DB_PATH"];

            var collectionManager = new ManagerFactory().GetManager();
            collectionManager.RootFolder = new FolderItem(@"G:\Downloads\Torrents\Hitman 2\Hitman.2-CPY", null);
            collectionManager.GenerateStructure();
            var counter = new Stopwatch();
            counter.Start();
            var res = collectionManager.Validate();
            counter.Stop();
            TimeSpan t = TimeSpan.FromMilliseconds(counter.ElapsedMilliseconds);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            Console.WriteLine($"File is {(res ? "" : "in")}valid. Time elapsed: {answer}");
            Console.ReadLine();
        }
    }
}