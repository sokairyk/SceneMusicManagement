using System;
using System.Collections.Generic;
using System.Text;

namespace SokairykFramework.Repository
{
    public interface IRepositoryWithUnitOfWork : IRepository
    {
        void BeginTransaction();
        void Commit();
    }
}
