using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SokairykFramework.Repository.Dapper
{
    public class DapperDataService : IDataService
    {
        public IRepository Repository => throw new NotImplementedException();

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task ExecuteInUnitOfWorkAsync(Action<IRepository> action)
        {
            throw new NotImplementedException();
        }
    }
}
