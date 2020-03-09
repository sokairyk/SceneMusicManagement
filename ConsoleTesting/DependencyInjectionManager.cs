using MusicManagementLib.Repository;
using SokairykFramework.AutoMapper;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
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
            _container.RegisterFactory<AutoMapper.IMapper>(f => AutoMapperExtensions.CreateConfig().CreateMapper(), new SingletonLifetimeManager());
        }
    }
}