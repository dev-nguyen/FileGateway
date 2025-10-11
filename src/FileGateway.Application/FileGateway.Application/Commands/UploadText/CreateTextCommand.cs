using MediatR;

namespace FileGateway.Application.Commands;

public record CreateTextCommand : IRequest<bool>
{
    public string Content { get; set; } = string.Empty;
    public bool DeleteAfterDownload { get; set; } = false;
    public Guid Onwer { get; set; }
}
