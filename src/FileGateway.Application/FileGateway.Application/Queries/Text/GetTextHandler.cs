using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Queries;

public class GetTextHandler : IRequestHandler<GetTextQuery, string>
{
    private readonly ITextSecretService _textSecretService;

    public GetTextHandler(ITextSecretService textSecretService)
    {
        _textSecretService = Ensure.IsNotNull(textSecretService);
    }

    public async Task<string> Handle(GetTextQuery request, CancellationToken cancellationToken)
    {
        return await _textSecretService.GetAsync(request, cancellationToken);
    }
}
