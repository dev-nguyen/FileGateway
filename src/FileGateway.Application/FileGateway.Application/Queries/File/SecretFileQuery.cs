using FileGateway.Domain.Entities;
using FileGateway.Domain.Enums;
using MediatR;

namespace FileGateway.Application.Queries;

public record SecretFileQuery : IRequest<(Stream?, Secret?)>
{
    public string Token { get; set; } = string.Empty;
    public StorageProvider Provider { get; set; } = StorageProvider.Local;
    public string? BucketName { get; set; }
    public string FolderPath { get; set; } = string.Empty;
}
