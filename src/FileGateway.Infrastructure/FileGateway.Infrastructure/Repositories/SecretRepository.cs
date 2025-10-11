using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using FileGateway.Domain.Enums;

namespace FileGateway.Infrastructure;

public class SecretRepository : BaseRepository<Secret, Guid>, ISecretRepository
{
    public SecretRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Secret?> GetSecretByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await GetByConditionAsync(x => x.Token == token && x.Status != FileStatus.Removed, cancellationToken);
    }

    public async Task<Secret?> GetSecretByFileName(string fileName, CancellationToken cancellationToken = default)
    {
        return await GetByConditionAsync(x => x.FileName == fileName && x.Status != FileStatus.Removed, cancellationToken);
    }
}
