using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using SokairykFramework.Repository;

namespace SokairykFramework.RepositoryImplementations
{
    public class NHibernateRepository : IRepository
    {
        public ISession Session { get; set; }

        public IQueryable<T> GetAll<T>()
        {
            return Session.Query<T>();
        }

        public async Task<T> GetByIdAsync<T>(object id)
        {
            
            return await Session.GetAsync<T>(id);
        }

        public async Task CreateAsync<T>(T entity)
        {
            await Session.SaveAsync(entity);
        }

        public async Task UpdateAsync<T>(T entity)
        {
            await Session.UpdateAsync(entity);
        }

        public async Task DeleteAsync<T>(T entity)
        {
            await Session.DeleteAsync(entity);
        }

        public async Task DeleteAsync<T>(object id)
        {
            await Session.DeleteAsync(await Session.LoadAsync<T>(id));
        }
    }
}
