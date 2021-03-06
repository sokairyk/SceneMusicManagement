﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SokairykFramework.Configuration;
using SokairykFramework.Repository;

namespace SokairykFramework.Tests.NHibernateRepository
{
    public class NhibernateRepositoryTests
    {
        private static readonly string _tempDBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.db");
        private static ISessionFactory _sessionFactory;

        [SetUp]
        public void Setup()
        {
            if (File.Exists(_tempDBPath))
                File.Delete(_tempDBPath);

            var config = new NHibernate.Cfg.Configuration()
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = $@"Data Source=""{_tempDBPath}"";Version=3;New=True;";
                    db.Dialect<SQLiteDialect>();
                });

            var mapper = new ModelMapper();
            mapper.AddMappings(new[] {typeof(TestEntityMapping)});
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

        [Test]
        public async Task UnitOfWorkTest()
        {
            var dataService = new TestDataService(null);
            var newEntry = new TestEntity {Id = 1, TextField = "This is a test", PrecisionField = 56.31m};
            var newEntry2 = new TestEntity {Id = 2, TextField = "This is another test"};

            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //With no transaction started there should be nothing to be commited
            await dataService.Repository.CreateAsync(newEntry);
            await dataService.UnitOfWork.CommitAsync();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //With proper use of begin / commit there should be no issue
            dataService.UnitOfWork.BeginTransaction();
            await dataService.Repository.CreateAsync(newEntry);
            await dataService.UnitOfWork.CommitAsync();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 1);

            //After commit is executed nothing else should be persisted
            await dataService.Repository.DeleteAsync(newEntry);
            await dataService.UnitOfWork.CommitAsync();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 1);

            //...until a new begin transaction is initiated
            dataService.UnitOfWork.BeginTransaction();
            await dataService.Repository.DeleteAsync(newEntry);
            await dataService.UnitOfWork.CommitAsync();
            Assert.AreEqual(dataService.Repository.GetAll<TestEntity>().Count(), 0);

            //Using ExecuteInUnitOfWork with an open transaction
            //in any thread should result in an exception
            dataService.UnitOfWork.BeginTransaction();

            Action<TestEntity> executeInUnitOfWorkAction = async ent =>
            {
                try
                {
                    await dataService.ExecuteInUnitOfWorkAsync(async repository => { await repository.CreateAsync(ent); });
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
            var workerThread = new Thread(() => { executeInUnitOfWorkAction(newEntry); });
            workerThread.Name = "Worker Thread";
            workerThread.Start();
            workerThread.Join();

            //Commit to dispose the open tranaction and open a new one
            //when needed
            await dataService.UnitOfWork.CommitAsync();

            executeInUnitOfWorkAction = async ent =>
            {
                try
                {
                    await dataService.ExecuteInUnitOfWorkAsync(async repository => { await repository.CreateAsync(ent); });
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

        private class TestDataService : NHibernateDataService
        {
            public TestDataService(IConfigurationManager configurationManager) : base(configurationManager)
            {
            }

            protected override ISessionFactory BuildSessionFactory()
            {
                return _sessionFactory;
            }
        }
    }
}