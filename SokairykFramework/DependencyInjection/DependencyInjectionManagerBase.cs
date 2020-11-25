using Unity;
using Unity.Resolution;

namespace SokairykFramework.DependencyInjection
{
    public abstract class DependencyInjectionManagerBase
    {
        public DependencyInjectionManagerBase()
        {
            Container = new UnityContainer();
            RegisterInterfaces();
        }

        protected DependencyInjectionManagerBase(IUnityContainer container)
        {
            Container = container ?? new UnityContainer();
            RegisterInterfaces();
        }

        protected IUnityContainer Container { get; }

        protected abstract void RegisterInterfaces();

        public T ResolveInterface<T>(params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(overrides);
        }

        public T ResolveInterface<T>(string name, params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(name, overrides);
        }
    }
}