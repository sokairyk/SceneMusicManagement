using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Sokairyk.Repository.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly ILogger _logger;
        private readonly ILogger<EntityFrameworkRepository> _repositoryLogger;

        public EntityFrameworkUnitOfWork(DbContext dbContext, ILogger<EntityFrameworkUnitOfWork> logger, ILogger<EntityFrameworkRepository> repositoryLogger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _repositoryLogger = repositoryLogger;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {

                await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await RollbackAsync(cancellationToken);
                _logger.LogError(ex, ex.Message);
                throw;
            }
            finally
            {
                _dbContext.Database.CurrentTransaction?.Dispose();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ExecuteInUnitOfWorkAsync(Action<IRepository> action, CancellationToken cancellationToken = default)
        {
            Exception excpetionToRethrow = null;

            await semaphoreSlim.WaitAsync();

            if (_dbContext.Database.CurrentTransaction != null)
            {
                semaphoreSlim.Release();
                throw new Exception("An existing transaction is already in progress. Cannot execute action.");
            }

            try
            {
                using (var repository = new EntityFrameworkRepository(_dbContext, _repositoryLogger))
                {
                    await BeginTransactionAsync();
                    action(repository);
                    await CommitAsync(default);
                }
            }
            catch (Exception ex)
            {
                excpetionToRethrow = ex;
            }
            finally
            {
                semaphoreSlim.Release();
            }

            if (excpetionToRethrow != null)
                throw excpetionToRethrow;
        }

        public void Dispose()
        {
            _dbContext.Database.CurrentTransaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
