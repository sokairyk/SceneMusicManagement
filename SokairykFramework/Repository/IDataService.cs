using System;

namespace SokairykFramework.Repository
{
    public interface IDataService
    {
        IRepository Repository { get; }
        IUnitOfWork UnitOfWork { get; }

        void ExecuteInSeparateUnitOfWork(Action<IRepository> action);
    }
}
