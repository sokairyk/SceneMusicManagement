using NHibernate;
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

        private ISession _session;
        protected ISession Session
        {
            get
            {
                _session = _session ?? _unitOfWork.Session;
                return _session;
            }
            private set
            {
                _session = value;
            }
        }

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
            _unitOfWork.Commit();
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
