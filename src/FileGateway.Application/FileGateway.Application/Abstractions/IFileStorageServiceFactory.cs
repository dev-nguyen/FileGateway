using FileGateway.Domain.Enums;

namespace FileGateway.Application.Abstractions;

public interface IFileStorageServiceFactory
{
    public IFileStorageService GetStorageProvider(StorageProvider provider, string? bucketName = null);
}
