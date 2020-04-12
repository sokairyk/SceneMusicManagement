using NHibernate;
using System;
using System.Threading.Tasks;

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

        public async Task CommitAsync()
        {
            try
            {
                if (Session.Transaction.IsActive)
                    await Session.Transaction.CommitAsync();
            }
            catch
            {
                if (Session.Transaction.IsActive)
                    await Session.Transaction.RollbackAsync();

                throw;
            }
            finally
            {
                Session?.Dispose();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (Session.Transaction.IsActive)
                    await Session.Transaction.RollbackAsync();
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
