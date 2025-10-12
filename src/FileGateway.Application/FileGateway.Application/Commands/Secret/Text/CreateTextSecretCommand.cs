using MediatR;

namespace FileGateway.Application.Commands;

public record class CreateTextSecretCommand : IRequest<string>
{
    public bool DeleteAfterDownload { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public Guid Owner { get; private set; }

    public CreateTextSecretCommand(string content, bool deleteAfterDownload, Guid owner)
    {
        DeleteAfterDownload = deleteAfterDownload;
        Content = content;
        Owner = owner;
    }
}
