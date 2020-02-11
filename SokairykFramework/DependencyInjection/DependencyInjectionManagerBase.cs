using SokairykFramework.Configuration;
using Unity;

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

        public T ResolveInterface<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
