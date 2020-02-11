using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MusicManagementLib.DAL.DTO;
using NHibernate;
using SokairykFramework.Configuration;
using SokairykFramework.UnitOfWork;

namespace MusicManagementLib.Repository
{
    public class ClementineUnitOfWork : BaseNHibernateUnitOfWork, IUnitOfWork
    {
        public override ISessionFactory BuildSessionFactory()
        {
            return Fluently.Configure()
            .Database(
                SQLiteConfiguration.Standard.UsingFile(new ConfigurationManager().GetApplicationSetting("CLEMENTINE_DB_PATH"))
            )
            .Mappings(m => m.FluentMappings.Add<ClementineSongMapping>())
            .BuildSessionFactory();
        }
    }
}