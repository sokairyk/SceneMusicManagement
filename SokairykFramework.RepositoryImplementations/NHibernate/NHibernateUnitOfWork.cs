using System;
using System.Threading.Tasks;
using NHibernate;
using SokairykFramework.Repository;

namespace SokairykFramework.RepositoryImplementations
{
    public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
    {
        public ISession Session { get; set; }

        public void BeginTransaction()
        {
            if (Session.GetCurrentTransaction()?.IsActive != true)
                Session.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            var currentTransaction = Session.GetCurrentTransaction();
            
            try
            {
                if (currentTransaction?.IsActive == true)
                    await currentTransaction.CommitAsync();
            }
            catch
            {
                if (currentTransaction?.IsActive == true)
                    await currentTransaction.RollbackAsync();

                throw;
            }
            finally
            {
                Session?.Dispose();
            }
        }

        public async Task RollbackAsync()
        {
            var currentTransaction = Session.GetCurrentTransaction();
            
            try
            {
                if (currentTransaction?.IsActive == true)
                    await currentTransaction.RollbackAsync();
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
