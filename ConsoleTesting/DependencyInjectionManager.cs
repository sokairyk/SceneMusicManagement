using MusicManagementLib.Repository;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
using SokairykFramework.Repository;
using Unity;

namespace ConsoleTesting
{
    public class DependencyInjectionManager : DependencyInjectionManagerBase
    {
        protected override void RegisterInterfaces()
        {
            _container.RegisterType<IConfigurationManager, ConfigurationManager>();
            _container.RegisterType<IRepositoryWithUnitOfWork, ClementineRepository>("Clementine");
        }
    }
}