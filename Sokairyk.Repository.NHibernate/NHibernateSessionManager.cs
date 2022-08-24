using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Cfg;

namespace Sokairyk.Repository.NHibernate
{
    public class NHibernateSessionManager : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger _logger;
        private ISession _currentSession;
        public NHibernateSessionManager(Configuration configuration, ILogger<NHibernateSessionManager> logger)
        {
            _sessionFactory = configuration.BuildSessionFactory();
            _logger = logger;
        }

        public ISession GetSession()
        {
            if (_currentSession?.IsConnected == true && _currentSession?.IsOpen == true)
                return _currentSession;

            _currentSession?.Dispose();
            _currentSession = _sessionFactory.OpenSession();
            return _currentSession;
        }

        public void Dispose()
        {
            _currentSession?.Dispose();
            _sessionFactory?.Dispose();
        }
    }
}
