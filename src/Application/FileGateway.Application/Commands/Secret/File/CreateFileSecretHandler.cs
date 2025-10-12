using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Commands;

public class CreateFileSecretHandler : IRequestHandler<CreateFileSecretCommand, string>
{
    private readonly IFileSecretService _fileSecretService;

    public CreateFileSecretHandler(IFileSecretService fileSecretService)
    {
        _fileSecretService = Ensure.IsNotNull(fileSecretService);
    }
    public async Task<string> Handle(CreateFileSecretCommand request, CancellationToken cancellationToken)
    {
        return await _fileSecretService.CreateAsync(request, cancellationToken);
    }
}
