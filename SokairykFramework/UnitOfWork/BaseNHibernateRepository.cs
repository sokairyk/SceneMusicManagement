using NHibernate;
using SokairykFramework.UnitOfWork;
using System;
using System.Linq;

namespace SokairykFramework.UnitOfWork
{
    public class BaseNHibernateRepository<T> : IRepository<T>, IDisposable
    {
        private BaseNHibernateUnitOfWork _unitOfWork;
        public BaseNHibernateRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (BaseNHibernateUnitOfWork)unitOfWork;
        }
        
        protected ISession Session { get; /*{ return _unitOfWork.Session; }*/ set; }
        //protected ISession Session => _unitOfWork.Session;

        public IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        public T GetById(int id)
        {
            return Session.Get<T>(id);
        }

        public void Create(T entity)
        {
            Session.Save(entity);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
        }

        public void Delete(int id)
        {
            Session.Delete(Session.Load<T>(id));
        }

        private void CloseSession()
        {
            Session?.Dispose();
            Session = null;
        }

        #region Dispose

        ~BaseNHibernateRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseSession();
            }
        }

        #endregion
    }
}
