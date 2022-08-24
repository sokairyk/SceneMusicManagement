using Microsoft.Extensions.Logging;
using Moq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sokairyk.Repository.NHibernate.Tests.DataModel;
using Sokairyk.Repository.Tests;

namespace Sokairyk.Repository.NHibernate.Tests
{
    public class NhibernateRepositoryTests : RepositoryAbstractTests
    {
        private static readonly string _tempDBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.db");
        private static NHibernateSessionManager _sessionManager;

        [SetUp]
        public void Setup()
        {
            if (File.Exists(_tempDBPath))
                File.Delete(_tempDBPath);

            var config = new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = $@"Data Source=""{_tempDBPath}"";Version=3;New=True;";
                    db.Dialect<SQLiteDialect>();
                });

            var mapper = new ModelMapper();
            mapper.AddMappings(new[] { typeof(TestEntityMapping) });
            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            //Create db schema
            new SchemaExport(config).Execute(true, true, false);

            //Instantiate Session Manager
            _sessionManager = new NHibernateSessionManager(config, Mock.Of<ILogger<NHibernateSessionManager>>());
        }

        [TearDown]
        public void Cleanup()
        {
            _sessionManager.Dispose();
            try
            {
                File.Delete(_tempDBPath);
            }
            catch
            {
            }
        }

        protected override IRepository CreateRepository()
        {
            return new NHibernateRepository(_sessionManager, Mock.Of<ILogger<NHibernateRepository>>());
        }

        protected override IUnitOfWork CreateUnitOfWork()
        {
            return new NHibernateUnitOfWork(_sessionManager, Mock.Of<ILogger<NHibernateUnitOfWork>>(), Mock.Of<ILogger<NHibernateRepository>>());
        }
    }
}