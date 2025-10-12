using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Commands;

public class CreateTextSecretHandler : IRequestHandler<CreateTextSecretCommand, string>
{
    private readonly ITextSecretService _textSecretService;

    public CreateTextSecretHandler(ITextSecretService textSecretService)
    {
        _textSecretService = Ensure.IsNotNull(textSecretService);
    }
    public async Task<string> Handle(CreateTextSecretCommand request, CancellationToken cancellationToken)
    {
        return await _textSecretService.CreateAsync(request, cancellationToken);
    }
}
