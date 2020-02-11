using Unity;

namespace SokairykFramework.DependencyInjection
{
    public abstract class DependencyInjectionManager
    {
        internal IUnityContainer Container { get; private set; }

        public DependencyInjectionManager()
        {
            Container = new UnityContainer();
            RegisterInterfaces();
        }

        public DependencyInjectionManager(IUnityContainer container)
        {
            Container = container ?? new UnityContainer();
            RegisterInterfaces();
        }

        internal abstract void RegisterInterfaces();
    }
}
