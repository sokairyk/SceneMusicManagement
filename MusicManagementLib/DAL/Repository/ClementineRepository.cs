using SokairykFramework.UnitOfWork;

namespace MusicManagementLib.Repository
{
    public class ClementineRepository<T> : BaseNHibernateRepository<T>, IRepository<T>
    {
        public ClementineRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
