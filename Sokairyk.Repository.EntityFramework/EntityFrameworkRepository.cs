using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sokairyk.Repository.EntityFramework
{
    public class EntityFrameworkRepository : IRepository, IDisposable
    {
        private readonly DbContext _dbContext;
        private readonly ILogger _logger;

        public EntityFrameworkRepository(DbContext dbContext, ILogger<EntityFrameworkRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            try
            {
                return _dbContext.Set<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<T> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                return await _dbContext.FindAsync<T>(new object[] { id }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _dbContext.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await Task.Run(() =>
                {
                    _dbContext.Update(entity);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await Task.Run(() =>
                {
                    _dbContext.Remove(entity);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            await DeleteAsync(await GetByIdAsync<T>(id, cancellationToken), cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Database.CurrentTransaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
