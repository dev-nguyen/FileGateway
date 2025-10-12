using FileGateway.Domain.Entities;

namespace FileGateway.Domain.Abstractions;

public interface IUserRepository : IRepository<User, Guid>
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
