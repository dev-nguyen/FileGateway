using FileGateway.Application.DTOs;
using MediatR;

namespace FileGateway.Application.Commands;

public record RegisterUserCommand : RegisterUserArgs, IRequest<bool>
{
    public RegisterUserCommand(string email, string password, string name) : base(email, password, name)
    {
    }
}
