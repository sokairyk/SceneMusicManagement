using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SokairykFramework.Repository.EntityFramework
{
    public abstract class EntityFrameworkDataService : IDataService
    {
        public IRepository Repository => throw new NotImplementedException();

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public EntityFrameworkDataService()
        {

        }

        public Task ExecuteInUnitOfWorkAsync(Action<IRepository> action)
        {
            throw new NotImplementedException();
        }
    }
}
