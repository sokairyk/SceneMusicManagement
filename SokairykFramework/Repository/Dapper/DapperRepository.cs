using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokairykFramework.Repository.Dapper
{
    public class DapperRepository : IRepository
    {
        public Task CreateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(object id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
