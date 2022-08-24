namespace Sokairyk.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;
        Task<T> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default) where T : class;
        Task CreateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class;
        Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    }
}
