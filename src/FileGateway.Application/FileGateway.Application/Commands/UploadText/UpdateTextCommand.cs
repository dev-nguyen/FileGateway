using MediatR;

namespace FileGateway.Application.Commands;

public record UpdateTextCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool DeleteAfterDownload { get; set; } = false;
}
