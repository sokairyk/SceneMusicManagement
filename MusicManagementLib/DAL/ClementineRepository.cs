using MusicManagementLib.Interfaces;
using NHibernate;
using System.Linq;

namespace MusicManagementLib.DAL
{
    public class ClementineRepository<T> : IRepository<T>
    {
        private ClementineUnitOfWork _unitOfWork;
        public ClementineRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (ClementineUnitOfWork)unitOfWork;
        }

        protected ISession Session { get { return _unitOfWork.Session; } }

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

    }
}
