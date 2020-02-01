using Unity;

namespace CollectionManagementLib.Factories
{
    public abstract class BaseFactory
    {
        internal IUnityContainer Container { get; private set; }

        public BaseFactory()
        {
            Container = new UnityContainer();
            RegisterInterfaces();
        }

        public BaseFactory(IUnityContainer container)
        {
            Container = container ?? new UnityContainer();
            RegisterInterfaces();
        }

        internal abstract void RegisterInterfaces();
    }
}
