using FileGateway.Application.Abstractions;
using FileGateway.Application.Commands;
using FileGateway.Application.DTOs;
using FileGateway.Application.Queries;
using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using FileGateway.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Services.Implementations;

public class FileSecretService : IFileSecretService
{
    private readonly ILogger<FileSecretService> _logger;
    private readonly IFileStorageServiceFactory _storageServiceFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;

    private const string UploadFolder = "Uploads";

    public FileSecretService(ILogger<FileSecretService> logger, IFileStorageServiceFactory storageServiceFactory, IUnitOfWork unitOfWork)
    {
        _logger = Ensure.IsNotNull(logger);

        _storageServiceFactory = Ensure.IsNotNull(storageServiceFactory);
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(_unitOfWork.Secret);
    }

    public async Task<string> CreateAsync(CreateFileSecretCommand args, CancellationToken cancellationToken = default)
    {
        var secretFile = await ValidAndCreateSecretInfoAsync(args, cancellationToken);
        if (args.StorageProvider == StorageProvider.S3)
        {
            secretFile.BucketName = args.BucketName ?? string.Empty;
        }

        secretFile.StoragePath = Path.Combine(UploadFolder, $"{secretFile.Token}{Path.GetExtension(secretFile.FileName)}");
        var uploadPath = Path.Combine(args.AbsolutePath, secretFile.StoragePath);
        var storageService = _storageServiceFactory.Create(args.StorageProvider);

        using (args.FileStream)
        {
            if (!await storageService.UploadFileAsync(args.FileStream, uploadPath, args.ContentType, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Failed to upload file: {FileName}", args.FileName);
                return string.Empty;
            }
        }

        await _secretRepository.CreateAsync(secretFile, cancellationToken);
        if (await _unitOfWork.SaveChangesAsync(cancellationToken) == 0)
        {
            _logger.LogError("Failed to save file: {FileName}", args.FileName);
            await storageService.DeleteFileAsync(uploadPath, cancellationToken);
            return string.Empty;
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Created secret for file {FileName}, Token: {Token}", secretFile.FileName, secretFile.Token);
        }
        return secretFile.Token;
    }

    public async Task<FileDownloadResult?> GetAsync(GetFileQuery args, CancellationToken cancellationToken = default)
    {
        var secretFile = await _secretRepository.GetSecretByTokenAsync(args.Token, cancellationToken);
        if (secretFile is null || !secretFile.CanBeDownloaded())
        {
            _logger.LogWarning("Failed to get file because file is remove or wrong token. Token: {Token}", args.Token);
            return null;
        }
        if (secretFile.DeleteAfterDownload)
        {
            await ChangeStatusAsync(secretFile, FileStatus.InProcess, cancellationToken);
        }


        var uploadPath = Path.Combine(args.AbsolutePath, secretFile.StoragePath);
        var storageService = _storageServiceFactory.Create(args.StorageProvider);
        var streamFile = await storageService.GetFileAsync(uploadPath, cancellationToken);

        var trackingStream = new TrackingStream(streamFile, async () =>
        {
            //Download completed
            if (secretFile.DeleteAfterDownload)
            {
                await ChangeStatusAsync(secretFile, FileStatus.Removed, cancellationToken);
                await storageService.DeleteFileAsync(uploadPath, cancellationToken);

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Deleted file after download. Token: {Token}", args.Token);
                }
            }
        });
        return new FileDownloadResult(trackingStream, secretFile.ContentType);
    }

    private async Task ChangeStatusAsync(Secret secretFile, FileStatus status, CancellationToken cancellationToken)
    {
        switch (status)
        {
            case FileStatus.InProcess:
                secretFile.MarkInProcess();
                break;
            case FileStatus.Removed:
                secretFile.MarkRemoved();
                break;
        }

        await _secretRepository.UpdateAsync(secretFile.Id, secretFile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Secret> ValidAndCreateSecretInfoAsync(CreateFileSecretCommand args, CancellationToken cancellationToken)
    {
        var secretFile = await _secretRepository.GetSecretByFileName(args.FileName, cancellationToken);
        if (secretFile is not null)
        {
            throw new InvalidOperationException($"Secret file {args.FileName} already exists.");
        }

        return new Secret
        {
            FileName = args.FileName,
            DeleteAfterDownload = args.DeleteAfterDownload,
            ContentType = args.ContentType,
            Owner = args.Owner
        };
    }
}
