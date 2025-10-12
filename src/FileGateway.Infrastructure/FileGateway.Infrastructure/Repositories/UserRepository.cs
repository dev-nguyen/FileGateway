using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;

namespace FileGateway.Infrastructure;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await GetByConditionAsync(u => u.Email == email, cancellationToken);
    }
}
