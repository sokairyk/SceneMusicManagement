using MusicManagementLib.DAL.Repository;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using SokairykFramework.Configuration;
using SokairykFramework.UnitOfWork;
using System.Linq;
using System.Reflection;

namespace MusicManagementLib.Repository
{
    public class ClementineUnitOfWork : BaseNHibernateUnitOfWork, IClementineUnitOfWork
    {
        public ClementineUnitOfWork(IConfigurationManager configurationManager) : base(configurationManager)
        {
        }

        public override ISessionFactory BuildSessionFactory()
        {
            var config = new Configuration()
                        .DataBaseIntegration(db =>
                        {
                            db.ConnectionString = $"Data Source={_configurationManager.GetApplicationSetting("CLEMENTINE_DB_PATH")};Version=3;New=False;Compress=True;";
                            db.Dialect<SQLiteDialect>();
                        });

            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.Namespace.EndsWith(".ClementineDTO")));
            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            return config.BuildSessionFactory();
        }
    }
}