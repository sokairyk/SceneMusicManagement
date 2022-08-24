using Microsoft.Extensions.Logging;

namespace Sokairyk.Repository.NHibernate
{
    public class NHibernateRepository : IRepository, IDisposable
    {
        private readonly NHibernateSessionManager _sessionManager;
        private readonly ILogger _logger;

        public NHibernateRepository(NHibernateSessionManager sessionManager, ILogger<NHibernateRepository> logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            try
            {
                return _sessionManager.GetSession().Query<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<T> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                return await _sessionManager.GetSession().GetAsync<T>(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task CreateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _sessionManager.GetSession().SaveAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _sessionManager.GetSession().UpdateAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _sessionManager.GetSession().DeleteAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task DeleteAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await _sessionManager.GetSession().DeleteAsync(await _sessionManager.GetSession().LoadAsync<T>(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public void Dispose()
        {
            _sessionManager.GetSession()?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
