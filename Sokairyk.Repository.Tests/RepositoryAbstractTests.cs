using Microsoft.Extensions.Logging;
using Moq;
using Sokairyk.Repository.NHibernate.Tests.DataModel;
using Sokairyk.Repository.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokairyk.Repository.Tests
{
    public abstract class RepositoryAbstractTests
    {
        protected abstract IRepository CreateRepository();
        protected abstract IUnitOfWork CreateUnitOfWork();

        [Test]
        public async Task UnitOfWorkTransactionHandlingTest()
        {
            var repository = CreateRepository();
            var unitOfWork = CreateUnitOfWork();

            var newEntry = new TestEntity { Id = 1, TextField = "This is a test", PrecisionField = 56.31m };

            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 0);

            //With no transaction started there should be nothing to be commited
            await repository.CreateAsync(newEntry);
            await unitOfWork.CommitAsync();
            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 0);

            //With proper use of begin / commit there should be no issue
            await unitOfWork.BeginTransactionAsync();
            await repository.CreateAsync(newEntry);
            await unitOfWork.CommitAsync();
            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 1);

            //After commit is executed nothing else should be persisted
            await repository.DeleteAsync(newEntry);
            await unitOfWork.CommitAsync();
            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 1);

            //...until a new begin transaction is initiated
            await unitOfWork.BeginTransactionAsync();
            await repository.DeleteAsync(newEntry);
            await unitOfWork.CommitAsync();
            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 0);
        }

        [Test]
        public async Task ExecuteInUnitOfWorkTest()
        {
            var newEntry = new TestEntity { Id = 1, TextField = "This is a test", PrecisionField = 56.31m };
            var newEntry2 = new TestEntity { Id = 2, TextField = "This is another test" };

            var repository = CreateRepository();
            var unitOfWork = CreateUnitOfWork();

            //Using ExecuteInUnitOfWork with an open transaction
            //in any thread should result in an exception
            await unitOfWork.BeginTransactionAsync();

            Action<TestEntity> executeInUnitOfWorkAction = async ent =>
            {
                try
                {
                    await unitOfWork.ExecuteInUnitOfWorkAsync(async (repo) => { await repo.CreateAsync(ent); });
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
            await unitOfWork.CommitAsync();

            executeInUnitOfWorkAction = async ent =>
            {
                try
                {
                    await unitOfWork.ExecuteInUnitOfWorkAsync(async (repo) => { await repo.CreateAsync(ent); });
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            };

            //Using ExecuteInUnitOfWork of the same NHibernateUnitOfWork instance should 
            //be thread safe and having multiple threads using this call should have them
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

            Assert.AreEqual(repository.GetAll<TestEntity>().Count(), 2);
        }
    }
}
