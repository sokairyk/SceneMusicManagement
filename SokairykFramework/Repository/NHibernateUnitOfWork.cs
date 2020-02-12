using NHibernate;
using SokairykFramework.Configuration;
using System;

namespace SokairykFramework.Repository
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        public ISession Session { get; set; }
        private ITransaction _transaction;

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if ((_transaction?.IsActive).Value)
                    _transaction.Commit();
            }
            catch
            {
                if ((_transaction?.IsActive).Value)
                    _transaction.Rollback();

                throw;
            }
            finally
            {
                Session?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                if ((_transaction?.IsActive).Value)
                    _transaction.Rollback();
            }
            finally
            {
                Session?.Dispose();
            }
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
