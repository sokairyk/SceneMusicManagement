using System;
using System.Linq;
using System.Threading.Tasks;

namespace SokairykFramework.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>();
        Task<T> GetByIdAsync<T>(object id);
        Task CreateAsync<T>(T entity);
        Task UpdateAsync<T>(T entity);
        Task DeleteAsync<T>(object id);
        Task DeleteAsync<T>(T entity);
    }
}
