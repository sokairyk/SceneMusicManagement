using NHibernate;
using SokairykFramework.UnitOfWork;

namespace SokairykFramework.UnitOfWork
{
    public abstract class BaseNHibernateUnitOfWork : IUnitOfWork
    {
        private ITransaction _transaction;
        private static ISessionFactory _sessionFactory;

        public ISession Session { get; set; }

        public BaseNHibernateUnitOfWork()
        {
            if (_sessionFactory == null)
                _sessionFactory = BuildSessionFactory();
            Session = _sessionFactory.OpenSession();
        }

        public abstract ISessionFactory BuildSessionFactory();

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
            finally
            {
                Session.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            }
            finally
            {
                Session.Dispose();
            }
        }

    }
}
