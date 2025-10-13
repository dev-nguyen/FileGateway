using FileGateway.Domain.Enums;
using MediatR;

namespace FileGateway.Application.Commands;

public record CreateFileSecretCommand : IRequest<string>
{
    public Stream FileStream { get; set; }
    public bool DeleteAfterDownload { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public StorageProvider StorageProvider { get; private set; }
    public Guid Owner { get; private set; }
    public string? BucketName { get; private set; }

    public string AbsolutePath { get; set; }

    public CreateFileSecretCommand(Stream fileStream, string fileName, string contentType, string absolutePath, bool deleteAfterDownload,
        StorageProvider storageProvider, Guid owner, string? bucketName = null)
    {
        FileStream = fileStream;
        FileName = fileName;
        ContentType = contentType;
        DeleteAfterDownload = deleteAfterDownload;
        StorageProvider = storageProvider;
        Owner = owner;
        BucketName = bucketName;
        AbsolutePath = absolutePath;
    }
}
