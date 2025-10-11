using Amazon.S3;
using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Enums;

namespace FileGateway.Infrastructure;

public class FileStorageServiceFactory : IFileStorageServiceFactory
{
    private readonly IAmazonS3 _s3Client;

    public FileStorageServiceFactory(IAmazonS3 s3Client)
    {
        _s3Client = Ensure.IsNotNull(s3Client);
    }

    public IFileStorageService GetStorageProvider(StorageProvider provider, string? bucketName = null)
    {
        return provider switch
        {
            StorageProvider.Local => new FileLocalStorageService(),
            StorageProvider.S3 => new FileS3StorageService(_s3Client, bucketName!),
            _ => throw new NotSupportedException($"Unsupported storage provider: {provider}")
        };

    }
}
