using NHibernate;
using SokairykFramework.Configuration;
using System;
using System.Threading;

namespace SokairykFramework.Repository
{
    public abstract class NHibernateDataService : IDataService
    {
        private static ISessionFactory _sessionFactory;
        protected static IConfigurationManager _configurationManager;
        private NHibernateRepository _repository;
        private NHibernateUnitOfWork _unitOfWork;
        private static ISession _commonSession;
        private static readonly object lockToken = new object();

        public IRepository Repository
        {
            get
            {
                SetCommonSession();
                return _repository;
            }
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                SetCommonSession();
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

        private void SetCommonSession()
        {
            if (_commonSession?.IsConnected == true && _commonSession?.IsOpen == true) return;

            _commonSession?.Dispose();
            _commonSession = _sessionFactory.OpenSession();
            _repository.Session = _unitOfWork.Session = _commonSession;
        }

        public void ExecuteInUnitOfWork(Action<IRepository> action)
        {
            lock (lockToken)
            {
                if (_commonSession?.Transaction?.IsActive == true)
                    throw new Exception("An existing transaction is already in progress. Cannot execute action.");

                SetCommonSession();
                _unitOfWork.BeginTransaction();
                action(_repository);
                _unitOfWork.Commit();
            }
        }
    }
}
