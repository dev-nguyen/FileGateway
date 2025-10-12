using FileGateway.Application.Commands;
using FileGateway.Application.Queries;

namespace FileGateway.Application.Services.Abstractions;

public interface ITextSecretService
{
    public Task<string> CreateAsync(CreateTextSecretCommand args, CancellationToken cancellationToken = default);
    public Task<string> GetAsync(GetTextQuery args, CancellationToken cancellationToken = default);
}
