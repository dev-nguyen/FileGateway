namespace FileGateway.Domain.Abstractions;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository User { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
