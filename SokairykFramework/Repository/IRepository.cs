using System;
using System.Linq;

namespace SokairykFramework.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>();
        T GetById<T>(object id);
        void Create<T>(T entity);
        void Update<T>(T entity);
        void Delete<T>(object id);
        void Delete<T>(T entity);
    }
}
