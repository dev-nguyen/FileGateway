using FileGateway.Application.Commands;
using FileGateway.Application.DTOs;
using FileGateway.Application.Queries;

namespace FileGateway.Application.Services.Abstractions;

public interface IFileSecretService
{
    public Task<string> CreateAsync(CreateFileSecretCommand args, CancellationToken cancellationToken = default);
    public Task<FileDownloadResult?> GetAsync(GetFileQuery args, CancellationToken cancellationToken = default);
}
