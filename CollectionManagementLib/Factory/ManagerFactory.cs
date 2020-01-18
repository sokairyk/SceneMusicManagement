using CollectionManagementLib.Factory;
using CollectionManagementLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace CollectionManagementLib.Factory
{
    public class ManagerFactory : BaseFactory
    {
        internal override void RegisterInterfaces()
        {
            Container.RegisterType<IHashCheck, HashCheckCRC>();
        }

        public CollectionManager GetManager()
        {
            return new CollectionManager(Container.Resolve<IHashCheck>());
        }
    }

}
