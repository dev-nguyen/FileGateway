using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using FileGateway.Domain.Enums;
using MediatR;

namespace FileGateway.Application.Queries;

public class DownloadSecretFileHandler : IRequestHandler<SecretFileQuery, (Stream?, Secret?)>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;
    private readonly IFileStorageServiceFactory _serviceFactory;

    public DownloadSecretFileHandler(IUnitOfWork unitOfWork, IFileStorageServiceFactory serviceFactory)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(unitOfWork.Secret);
        _serviceFactory = Ensure.IsNotNull(serviceFactory);
    }
    public async Task<(Stream?, Secret?)> Handle(SecretFileQuery request, CancellationToken cancellationToken)
    {
        var secret = await _secretRepository.GetSecretByTokenAsync(request.Token, cancellationToken);
        if (secret is null)
        {
            return (null, null);
        }
        else if (secret.Status == FileStatus.InProcess)
        {
            return (null, secret);
        }

        var storageService = GetStorage(request);
        var filePath = Path.Combine(request.FolderPath, secret.StoragePath);
        var stream = await storageService.GetFileAsync(filePath, cancellationToken);

        var trackingStream = new TrackingStream(stream, async () =>
        {
            //Download completed
            if (secret.DeleteAfterDownload)
            {
                secret.Status = FileStatus.Removed;
                await _secretRepository.UpdateAsync(secret.Id, secret, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        });

        return (trackingStream, secret);
    }

    private IFileStorageService GetStorage(SecretFileQuery request)
    {
        return _serviceFactory.GetStorageProvider(
            request.Provider,
            request.Provider == StorageProvider.S3 ? request.BucketName : null
        );
    }
}
