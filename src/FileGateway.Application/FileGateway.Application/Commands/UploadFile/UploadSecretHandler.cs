using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using FileGateway.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Commands;

public class UploadSecretHandler : IRequestHandler<UploadSecretCommand, bool>
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;
    private readonly IFileStorageServiceFactory _storageServiceFactory;

    public UploadSecretHandler(ILogger<UploadSecretHandler> logger, IUnitOfWork unitOfWork, IFileStorageServiceFactory storageServiceFactory)
    {
        _logger = Ensure.IsNotNull(logger);
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(_unitOfWork.Secret);
        _storageServiceFactory = Ensure.IsNotNull(storageServiceFactory);
    }

    public async Task<bool> Handle(UploadSecretCommand request, CancellationToken cancellationToken)
    {
        var storageService = GetStorage(request);
        var secret = await _secretRepository.GetSecretByFileName(request.FileName, cancellationToken);
        if (secret is null)
        {
            secret = new Secret
            {
                FileName = request.FileName,
                Size = request.FileSize,
                Provider = request.Provider,
                ContentType = request.ContentType,
                SecretType = SecretType.File,
                DeleteAfterDownload = request.DeleteAfterDownload,
                Owner = request.Owner
            };
            if (request.Provider == StorageProvider.S3)
            {
                secret.BucketName = request.BucketName;
            }

            secret.StoragePath = $@"Uploads\{secret.Token}{Path.GetExtension(request.FileName)}";
            var uploadPath = Path.Combine(request.AbsolutePath, secret.StoragePath);

            if (await UploadFileAsync(storageService, request, uploadPath, cancellationToken) && !await SaveSecretAsync(secret, cancellationToken))
            {
                await DeleteFileAsync(storageService, secret.StoragePath, cancellationToken);
                return false;
            }
            return true;
        }
        else
        {
            if (secret.Size != request.FileSize)
            {
                secret.Size = request.FileSize;
                await UploadFileAsync(storageService, request, secret.StoragePath, cancellationToken);
            }
            else
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("File already exists with the same size. Skipping upload.");
                }
            }

            secret.DeleteAfterDownload = request.DeleteAfterDownload;
            return await SaveSecretAsync(secret, cancellationToken);
        }
    }

    private IFileStorageService GetStorage(UploadSecretCommand request)
    {
        return _storageServiceFactory.GetStorageProvider(
            request.Provider,
            request.Provider == StorageProvider.S3 ? request.BucketName : null
        );
    }

    private async Task<bool> UploadFileAsync(IFileStorageService storage, UploadSecretCommand request, string filePath, CancellationToken cancellationToken)
    {
        var success = false;
        if (request.FileStream.CanSeek)
            request.FileStream.Position = 0;
        try
        {
            success = await storage.UploadFileAsync(request.FileStream, request.ContentType, filePath, cancellationToken);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation("Uploaded '{FileName}' to {Provider} successfully.", request.FileName, request.Provider);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload '{FileName}' to {Provider}.", request.FileName, request.Provider);
        }

        return success;
    }

    private async static Task<bool> DeleteFileAsync(IFileStorageService storage, string filePath, CancellationToken cancellationToken)
    {
        return await storage.DeleteFileAsync(filePath, cancellationToken);
    }

    private async Task<bool> SaveSecretAsync(Secret secret, CancellationToken cancellationToken)
    {
        var success = false;
        try
        {
            await _secretRepository.CreateAsync(secret, cancellationToken);
            success = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save '{FileName}' to database.", secret.FileName);
        }
        return success;
    }
}
