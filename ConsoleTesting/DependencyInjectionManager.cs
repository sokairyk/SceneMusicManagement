using MusicManagementLib.Repository;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
using SokairykFramework.Repository;
using Unity;
using Unity.Injection;

namespace ConsoleTesting
{
    public class DependencyInjectionManager : DependencyInjectionManagerBase
    {
        protected override void RegisterInterfaces()
        {
            _container.RegisterType<IConfigurationManager, ConfigurationManager>(new InjectionConstructor("appsettings.json"));
            _container.RegisterType<IRepositoryWithUnitOfWork, ClementineRepository>("Clementine");

        }
    }
}