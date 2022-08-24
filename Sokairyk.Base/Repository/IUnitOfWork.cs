namespace Sokairyk.Repository
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task ExecuteInUnitOfWorkAsync(Action<IRepository> action, CancellationToken cancellationToken = default);
    }
}
