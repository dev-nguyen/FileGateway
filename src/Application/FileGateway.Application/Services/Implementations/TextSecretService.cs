using FileGateway.Application.Commands;
using FileGateway.Application.Queries;
using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Services.Implementations;

public class TextSecretService : ITextSecretService
{
    private readonly ILogger<TextSecretService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;

    public TextSecretService(ILogger<TextSecretService> logger, IUnitOfWork unitOfWork)
    {
        _logger = Ensure.IsNotNull(logger);

        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(_unitOfWork.Secret);
    }
    public async Task<string> CreateAsync(CreateTextSecretCommand args, CancellationToken cancellationToken = default)
    {
        var secret = new Secret
        {
            Content = args.Content,
            DeleteAfterDownload = args.DeleteAfterDownload,
            Owner = args.Owner
        };
        await _secretRepository.CreateAsync(secret, cancellationToken);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        if (result && _logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Created text secret with Token: {Token}", secret.Token);
        }
        return secret.Token;
    }

    public async Task<string> GetAsync(GetTextQuery args, CancellationToken cancellationToken = default)
    {
        var secretText = await _secretRepository.GetSecretByTokenAsync(args.Token, cancellationToken);
        if (secretText is null || !secretText.CanBeDownloaded())
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Failed to get content because status is set to Remove or invalid token. Token: {Token}", args.Token);
            }
            return string.Empty;
        }

        if (secretText.DeleteAfterDownload)
        {
            secretText.MarkRemoved();
            await _secretRepository.UpdateAsync(secretText.Id, secretText, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Secret with token {Token} retrieved successfully.", args.Token);
        }

        return secretText.Content;
    }
}
