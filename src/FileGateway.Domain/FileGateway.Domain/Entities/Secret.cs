using FileGateway.Domain.Enums;

namespace FileGateway.Domain.Entities;

public record Secret
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = Guid.NewGuid().ToString("N");
    public string Content { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public StorageProvider StorageProvider { get; set; }
    public bool DeleteAfterDownload { get; set; }
    public FileStatus Status { get; set; } = FileStatus.Available;
    public DateTime CreatedAt { get; set; }
    public Guid Owner { get; set; }
    public string BucketName { get; set; } = string.Empty;


    public bool CanBeDownloaded() => Status == FileStatus.Available;
    public void MarkInProcess() => Status = FileStatus.InProcess;
    public void MarkRemoved() => Status = FileStatus.Removed;
    public void MarkAvailable() => Status = FileStatus.Available;
}
