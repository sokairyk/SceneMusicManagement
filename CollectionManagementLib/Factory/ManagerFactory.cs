using CollectionManagementLib.Interfaces;
using Unity;

namespace CollectionManagementLib.Factories
{
    public class ManagerFactory : BaseFactory
    {
        internal override void RegisterInterfaces()
        {
            Container.RegisterType<IHashCheck, HashCheckCRC>();
            Container.RegisterType<IHashInfoHandler, HashInfoHandlerSFV>();
            Container.RegisterType<IManager, CollectionManager>();
        }

        public IManager GetManager()
        {
            return Container.Resolve<IManager>();
        }
    }

}
