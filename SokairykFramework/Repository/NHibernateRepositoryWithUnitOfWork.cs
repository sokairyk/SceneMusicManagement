using NHibernate;
using SokairykFramework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SokairykFramework.Repository
{
    public abstract class NHibernateRepositoryWithUnitOfWork : NHibernateRepository, IRepositoryWithUnitOfWork
    {
        private static ISessionFactory _sessionFactory;
        protected static IConfigurationManager _configurationManager;
        private NHibernateUnitOfWork _unitOfWork;

        public NHibernateRepositoryWithUnitOfWork(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _sessionFactory = _sessionFactory ?? BuildSessionFactory();
            _unitOfWork = new NHibernateUnitOfWork();
            BeginTransaction();
        }

        public abstract ISessionFactory BuildSessionFactory();

        public void BeginTransaction()
        {
            Session?.Dispose();
            _unitOfWork?.Session?.Dispose();
            Session = _unitOfWork.Session = _sessionFactory.OpenSession();
            _unitOfWork?.BeginTransaction();
        }

        public void Commit()
        {
            _unitOfWork?.Commit();
        }
    }
}
