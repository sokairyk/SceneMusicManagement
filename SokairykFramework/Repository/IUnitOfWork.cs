using System;
using System.Threading.Tasks;

namespace SokairykFramework.Repository
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
