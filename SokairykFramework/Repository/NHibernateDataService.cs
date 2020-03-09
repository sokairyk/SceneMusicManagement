using NHibernate;
using SokairykFramework.Configuration;
using System;

namespace SokairykFramework.Repository
{
    public abstract class NHibernateDataService : IDataService
    {
        private static ISessionFactory _sessionFactory;
        protected static IConfigurationManager _configurationManager;
        private NHibernateRepository _repository;
        private NHibernateUnitOfWork _unitOfWork;
        private ISession _currentSession;

        public IRepository Repository
        {
            get
            {
                OpenCommonSession();
                return _repository;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                OpenCommonSession();
                return _unitOfWork;
            }
        }

        public NHibernateDataService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _sessionFactory = _sessionFactory ?? BuildSessionFactory();
            _repository = new NHibernateRepository();
            _unitOfWork = new NHibernateUnitOfWork();

        }

        public abstract ISessionFactory BuildSessionFactory();

        private void OpenCommonSession()
        {
            if (_currentSession?.IsConnected == true && _currentSession?.IsOpen == true) return;

            _currentSession?.Dispose();
            _currentSession = _sessionFactory.OpenSession();
            _unitOfWork.Session = _currentSession;
            _repository.Session = _currentSession;
        }

        public void ExecuteInSeparateUnitOfWork(Action<IRepository> action)
        {
            //Get a new session and create an new unit of work
            using (var newSession = _sessionFactory.OpenSession())
            using (var newUnitOfWork = new NHibernateUnitOfWork())
            {
                //Since the repository is going to be used assing there
                //the new session
                _repository.Session = newSession;
                //..and also to the new UnitOfWork
                newUnitOfWork.Session = newSession;
                newUnitOfWork.BeginTransaction();
                //Execute the action using the private property of avoid
                //triggering the getter and the OpenCommonSession function
                action(_repository);
                newUnitOfWork.Commit();
            }

            //Restore to the repository it's previous session
            _repository.Session = _currentSession;
        }
    }
}
