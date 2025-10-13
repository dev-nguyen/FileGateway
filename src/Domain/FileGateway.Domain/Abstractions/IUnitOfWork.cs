namespace FileGateway.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository User { get; }
    public ISecretRepository Secret { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task TransactionExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}
