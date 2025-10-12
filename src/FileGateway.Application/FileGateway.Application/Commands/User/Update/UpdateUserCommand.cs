using FileGateway.Application.DTOs;
using MediatR;

namespace FileGateway.Application.Commands;

public record UpdateUserCommand : UpdateUserArgs, IRequest<bool>
{
    public string Email { get; private set; }
    public UpdateUserCommand(string email, string? password, string? name)
        : base(password, name)
    {
        Email = email;
    }
}
