using NHibernate;
using System;

namespace SokairykFramework.Repository
{
    public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
    {
        public ISession Session { get; set; }

        public void BeginTransaction()
        {
            if (!Session.Transaction.IsActive)
                Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (Session.Transaction.IsActive)
                    Session.Transaction.Commit();
            }
            catch
            {
                if (Session.Transaction.IsActive)
                    Session.Transaction.Rollback();

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
                if (Session.Transaction.IsActive)
                    Session.Transaction.Rollback();
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
