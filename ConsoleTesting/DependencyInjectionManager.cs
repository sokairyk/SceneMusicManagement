using CollectionManagementLib;
using CollectionManagementLib.Manager;
using MusicManagementLib.Repository;
using SokairykFramework.AutoMapper;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
using SokairykFramework.Hashing;
using SokairykFramework.Logger;
using SokairykFramework.Repository;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ConsoleTesting
{
    public class DependencyInjectionManager : DependencyInjectionManagerBase
    {
        protected override void RegisterInterfaces()
        {
            _container.RegisterType<IConfigurationManager, ConfigurationManager>(new InjectionConstructor("appsettings.json"));
            _container.RegisterType<IDataService, ClementineService>("Clementine");
            _container.RegisterInstance(typeof(ILogger), Log4NetLogger.Instance);
            _container.RegisterType<IHashInfoHandler, HashInfoHandlerSFV>();
            _container.RegisterType<IHashCheck, HashCheckCRC>();
            _container.RegisterType<IManager, CollectionManager>();
            _container.RegisterFactory<AutoMapper.IMapper>(f => AutoMapperExtensions.CreateConfig().CreateMapper(), new SingletonLifetimeManager());
        }
    }
}