using MusicManagementLib.DAL.ClementineDTO;
using MusicManagementLib.DAL.Repository;
using MusicManagementLib.Repository;
using SokairykFramework.Configuration;
using SokairykFramework.DependencyInjection;
using SokairykFramework.UnitOfWork;
using Unity;

namespace ConsoleTesting
{
    public class DependencyInjectionManager : DependencyInjectionManagerBase
    {
        protected override void RegisterInterfaces()
        {
            _container.RegisterType<IConfigurationManager, ConfigurationManager>();
            _container.RegisterType<IUnitOfWork, ClementineUnitOfWork>();
            _container.RegisterType<IClementineRepository<ClementineSong>, ClementineRepository<ClementineSong>>();
        }

    }
}
