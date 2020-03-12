using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SokairykFramework.Configuration;
using SokairykFramework.Repository;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SokairykFramework.Tests.Repository
{
    public class NhibernateRepositoryTests
    {
        private static readonly string _tempDBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.db");
        private static ISessionFactory _sessionFactory;

        [SetUp]
        public void Setup()
        {
            var config = new NHibernate.Cfg.Configuration()
                        .DataBaseIntegration(db =>
                        {
                            db.ConnectionString = $@"Data Source=""{_tempDBPath}"";Version=3;New=True;";
                            db.Dialect<SQLiteDialect>();
                        });

            var mapper = new ModelMapper();
            mapper.AddMappings(new Type[] { typeof(TestEntityMapping) });
            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            _sessionFactory = config.BuildSessionFactory();
            //Create db schema
            new SchemaExport(config).Execute(true, true, false);
        }

        [TearDown]
        public void Cleanup()
        {
            _sessionFactory.Dispose();
            try
            {
                File.Delete(_tempDBPath);
            }
            catch
            {

            }
        }

        class TestDataService : NHibernateDataService
        {
            public TestDataService(IConfigurationManager configurationManager) : base(configurationManager)
            {
            }

            public override ISessionFactory BuildSessionFactory()
            {
                return _sessionFactory;
            }
        }

        [Test]
        public void UnitOfWorkTest()
        {
            var dataService = new TestDataService(null);
            var newEntry = new TestEntity { Id = 1, TextField = "This is a test", PrecisionField = 56.31m };
            var newEntry2 = new TestEntity { Id = 2, TextField = "This is another test"};

            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //With no transaction started there should be nothing to be commited
            dataService.Repository.Create(newEntry);
            dataService.UnitOfWork.Commit();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //With proper use of begin / commit there should be no issue
            dataService.UnitOfWork.BeginTransaction();
            dataService.Repository.Create(newEntry);
            dataService.UnitOfWork.Commit();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 1);

            //After commit is executed nothing else should be persisted
            dataService.Repository.Delete(newEntry);
            dataService.UnitOfWork.Commit();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 1);

            //...until a new begin transaction is initiated
            dataService.UnitOfWork.BeginTransaction();
            dataService.Repository.Delete(newEntry);
            dataService.UnitOfWork.Commit();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //Using ExecuteInUnitOfWork with an open transaction
            //in any thread should result in an exception
            dataService.UnitOfWork.BeginTransaction();

            Action<TestEntity> executeInUnitOfWorkAction = (TestEntity ent) =>
            {
                try
                {
                    dataService.ExecuteInUnitOfWork(repository =>
                    {
                        repository.Create(ent);
                    });
                    Assert.Fail("Should have raised an exception");
                }
                catch (Exception ex)
                {
                    if (ex is AssertionException)
                        Assert.Fail("Should not be an assertion exception");
                }
            };

            //Main thread Exception
            executeInUnitOfWorkAction(newEntry);

            //Worker thread Exception
            var workerThread = new Thread(() =>
            {
                executeInUnitOfWorkAction(newEntry);
            });
            workerThread.Name = "Worker Thread";
            workerThread.Start();
            workerThread.Join();

            //Commit to dispose the open tranaction and open a new one
            //when needed
            dataService.UnitOfWork.Commit();

            executeInUnitOfWorkAction = (TestEntity ent) =>
            {
                try
                {
                    dataService.ExecuteInUnitOfWork(repository =>
                    {
                        repository.Create(ent);
                    });
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            };

            //Using ExecuteInUnitOfWork of NHibernateDataService should be thread
            //safe and having multiple threads using this call should have them
            //await each other to finish
            workerThread = new Thread(() =>
            {
                executeInUnitOfWorkAction(newEntry);
                Thread.Sleep(500);
            });
            workerThread.Name = "Worker Thread";

            var slowerWorkerThread = new Thread(() =>
            {
                Thread.Sleep(10);
                executeInUnitOfWorkAction(newEntry2);
                Thread.Sleep(1000);
            });
            slowerWorkerThread.Name = "Slower Worker Thread";
            slowerWorkerThread.Start();
            workerThread.Start();

            workerThread.Join();
            slowerWorkerThread.Join();
            
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 2);
        }
    }
}
