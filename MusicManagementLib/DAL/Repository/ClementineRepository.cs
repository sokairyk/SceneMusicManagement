using MusicManagementLib.DAL.Repository;
using SokairykFramework.UnitOfWork;

namespace MusicManagementLib.Repository
{
    public class ClementineRepository<T> : BaseNHibernateRepository<T>, IClementineRepository<T>
    {
        public ClementineRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
