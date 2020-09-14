using NHibernate;
using NHibernate.Mapping;
using SokairykFramework.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SokairykFramework.Repository
{
    public abstract class NHibernateDataService : IDataService
    {
        private static ISessionFactory _sessionFactory;
        protected static IConfigurationManager _configurationManager;
        private NHibernateRepository _repository;
        private NHibernateUnitOfWork _unitOfWork;
        private static ISession _commonSession;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

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

        protected NHibernateDataService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _sessionFactory = _sessionFactory ?? BuildSessionFactory();
            _repository = new NHibernateRepository();
            _unitOfWork = new NHibernateUnitOfWork();

        }

        protected abstract ISessionFactory BuildSessionFactory();

        private void SetCommonSession()
        {
            if (_commonSession?.IsConnected == true && _commonSession?.IsOpen == true) return;

            _commonSession?.Dispose();
            _commonSession = _sessionFactory.OpenSession();
            _repository.Session = _unitOfWork.Session = _commonSession;
        }

        public async Task ExecuteInUnitOfWorkAsync(Action<IRepository> action)
        {
            Exception excpetionToRethrow = null;

            await semaphoreSlim.WaitAsync();

            if (_commonSession?.GetCurrentTransaction()?.IsActive == true)
            {
                semaphoreSlim.Release();
                throw new Exception("An existing transaction is already in progress. Cannot execute action.");
            }

            try
            {
                SetCommonSession();
                _unitOfWork.BeginTransaction();
                action(_repository);
                await _unitOfWork.CommitAsync();
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
    }
}
