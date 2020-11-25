using AutoMapper;
using CollectionManagementLib;
using DiskFilesManagement.Manager;
using MediaPlayersDAL.Clementine;
using MusicPlayersDAL.Repositories;
using SokairykFramework.AutoMapper;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
using SokairykFramework.Hashing;
using SokairykFramework.Logger;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ConsoleTesting
{
    public class DependencyInjectionManager : DependencyInjectionManagerBase
    {
        protected override void RegisterInterfaces()
        {
            Container.RegisterType<IConfigurationManager, ConfigurationManager>(new InjectionConstructor("appsettings.json"));
            Container.RegisterType<IClementineRepository, ClementineNHibernateRepository>();
            Container.RegisterInstance(typeof(ILogger), Log4NetLogger.Instance);
            Container.RegisterType<IHashInfoHandler, HashInfoHandlerSFV>();
            Container.RegisterType<IHashCheck, HashCheckCRC>();
            Container.RegisterType<IManager, CollectionManager>();
            Container.RegisterFactory<IMapper>(f => AutoMapperExtensions.CreateConfig().CreateMapper(), new SingletonLifetimeManager());
        }
    }
}