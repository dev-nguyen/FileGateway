using FileGateway.Application.DTOs;
using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Queries;

public class GetFileHandler : IRequestHandler<GetFileQuery, FileDownloadResult?>
{
    private readonly IFileSecretService _fileSecretService;

    public GetFileHandler(IFileSecretService fileSecretService)
    {
        _fileSecretService = Ensure.IsNotNull(fileSecretService);
    }
    public async Task<FileDownloadResult?> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        return await _fileSecretService.GetAsync(request, cancellationToken);
    }
}
