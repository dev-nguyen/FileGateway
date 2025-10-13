using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Commands;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserService _userService;

    public LoginUserHandler(IUserService userService)
    {
        _userService = Ensure.IsNotNull(userService);
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.LoginAsync(request, cancellationToken);
    }
}
