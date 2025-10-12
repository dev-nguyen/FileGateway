using FileGateway.Domain.Entities;

namespace FileGateway.Domain.Abstractions
{
    public interface ISecretRepository : IRepository<Secret, Guid>
    {
        public Task<Secret?> GetSecretByTokenAsync(string token, CancellationToken cancellationToken = default);
        public Task<Secret?> GetSecretByFileName(string fileName, CancellationToken cancellationToken = default);
    }
}
