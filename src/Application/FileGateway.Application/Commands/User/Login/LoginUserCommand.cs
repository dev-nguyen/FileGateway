using FileGateway.Application.DTOs;
using MediatR;

namespace FileGateway.Application.Commands;

public record LoginUserCommand : BaseUserArgs, IRequest<string>
{
    public LoginUserCommand(string email, string password)
        : base(email, password)
    {

    }
}
