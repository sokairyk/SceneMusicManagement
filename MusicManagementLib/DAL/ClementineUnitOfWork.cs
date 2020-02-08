using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MusicManagementLib.Interfaces;
using NHibernate;
using NHibernate.Cfg;
using System.Configuration;

namespace MusicManagementLib.DAL
{
    public class ClementineUnitOfWork : IUnitOfWork
    {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        static ClementineUnitOfWork()
        {
            _sessionFactory = Fluently.Configure()
            .Database(
                SQLiteConfiguration.Standard.UsingFile(ConfigurationManager.AppSettings["CLEMENTINE_DB_PATH"])
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClementineUnitOfWork>())
            .ExposeConfiguration((NHibernate.Cfg.Configuration config) =>
            {
                //Not interfering with the database schema yet...
                //Using it as is...
            })
            .BuildSessionFactory();
        }

        public ClementineUnitOfWork()
        {
            Session = _sessionFactory.OpenSession();
        }

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
