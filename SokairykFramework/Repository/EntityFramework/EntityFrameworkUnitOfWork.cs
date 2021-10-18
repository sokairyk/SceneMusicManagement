using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SokairykFramework.Repository.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException();
        }
    }
}
