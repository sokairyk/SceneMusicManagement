using NHibernate;
using System;
using System.Linq;

namespace SokairykFramework.Repository
{
    public class NHibernateRepository : IRepository
    {
        public ISession Session { get; set; }

        public IQueryable<T> GetAll<T>()
        {
            return Session.Query<T>();
        }

        public T GetById<T>(object id)
        {
            return Session.Get<T>(id);
        }

        public void Create<T>(T entity)
        {
            Session.Save(entity);
        }

        public void Update<T>(T entity)
        {
            Session.Update(entity);
        }

        public void Delete<T>(T entity)
        {
            Session.Delete(entity);
        }

        public void Delete<T>(object id)
        {
            Session.Delete(Session.Load<T>(id));
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
