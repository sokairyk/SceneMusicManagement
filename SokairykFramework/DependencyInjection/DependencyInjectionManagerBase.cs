using SokairykFramework.Configuration;
using Unity;
using Unity.Resolution;

namespace SokairykFramework.DependencyInjection
{
    public abstract class DependencyInjectionManagerBase
    {
        protected IUnityContainer _container { get; private set; }

        public DependencyInjectionManagerBase()
        {
            _container = new UnityContainer();
            RegisterInterfaces();
        }

        public DependencyInjectionManagerBase(IUnityContainer container)
        {
            _container = container ?? new UnityContainer();
            RegisterInterfaces();
        }

        protected abstract void RegisterInterfaces();

        public T ResolveInterface<T>(params ResolverOverride[] overrides)
        {
            return _container.Resolve<T>(overrides);
        }

        public T ResolveInterface<T>(string name, params ResolverOverride[] overrides)
        {
            return _container.Resolve<T>(name, overrides);
        }
    }
}
