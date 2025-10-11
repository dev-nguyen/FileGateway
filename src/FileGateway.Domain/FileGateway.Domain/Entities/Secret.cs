using FileGateway.Domain.Enums;

namespace FileGateway.Domain.Entities;

public record Secret : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = Guid.NewGuid().ToString("N");
    public string TextContent { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public SecretType SecretType { get; set; }
    public FileStatus Status { get; set; }
    public StorageProvider Provider { get; set; }
    public string ContentType { get; set; } = "application/octet-stream";
    public string BucketName { get; set; } = string.Empty;
    public bool DeleteAfterDownload { get; set; } = false;
    public long Size { get; set; }
}
