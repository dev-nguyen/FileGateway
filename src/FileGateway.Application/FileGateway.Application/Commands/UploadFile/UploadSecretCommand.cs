using FileGateway.Domain.Enums;
using MediatR;

namespace FileGateway.Application.Commands;

public record UploadSecretCommand : IRequest<bool>
{
    public required Stream FileStream { get; set; }
    public long FileSize { get; set; }
    public string AbsolutePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool DeleteAfterDownload { get; set; }
    public StorageProvider Provider { get; set; }
    public string BucketName { get; set; } = string.Empty;
    public Guid Owner { get; set; }
}
