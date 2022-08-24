using Microsoft.Extensions.Logging;
using NHibernate;

namespace Sokairyk.Repository.NHibernate
{
    public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NHibernateSessionManager _sessionManager;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly ILogger _logger;
        private readonly ILogger<NHibernateRepository> _repositoryLogger;

        public NHibernateUnitOfWork(NHibernateSessionManager sessionManager, ILogger<NHibernateUnitOfWork> logger, ILogger<NHibernateRepository> repositoryLogger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
            _repositoryLogger = repositoryLogger;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_sessionManager.GetSession().GetCurrentTransaction()?.IsActive != true)
                        _sessionManager.GetSession().BeginTransaction();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            var currentTransaction = _sessionManager.GetSession().GetCurrentTransaction();

            try
            {
                if (currentTransaction?.IsActive == true)
                    await currentTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await currentTransaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex.Message, ex);
                throw;
            }
            finally
            {
                _sessionManager.GetSession()?.Dispose();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            var currentTransaction = _sessionManager.GetSession().GetCurrentTransaction();

            try
            {
                if (currentTransaction?.IsActive == true)
                    await currentTransaction.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
            finally
            {
                _sessionManager.GetSession()?.Dispose();
            }
        }

        public async Task ExecuteInUnitOfWorkAsync(Action<IRepository> action, CancellationToken cancellationToken = default)
        {
            Exception excpetionToRethrow = null;

            await semaphoreSlim.WaitAsync();

            if (_sessionManager.GetSession()?.GetCurrentTransaction()?.IsActive == true)
            {
                semaphoreSlim.Release();
                throw new Exception("An existing transaction is already in progress. Cannot execute action.");
            }

            try
            {
                using (var repository = new NHibernateRepository(_sessionManager, _repositoryLogger))
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
            _sessionManager.GetSession()?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
