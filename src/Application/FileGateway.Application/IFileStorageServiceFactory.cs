using FileGateway.Domain.Enums;

namespace FileGateway.Application.Abstractions;

public interface IFileStorageServiceFactory
{
    public IFileStorageService Create(StorageProvider provider, string? bucketName = null);
}
