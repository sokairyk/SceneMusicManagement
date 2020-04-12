using System;
using System.Threading.Tasks;

namespace SokairykFramework.Repository
{
    public interface IDataService
    {
        IRepository Repository { get; }
        IUnitOfWork UnitOfWork { get; }

        Task ExecuteInUnitOfWorkAsync(Action<IRepository> action);
    }
}
