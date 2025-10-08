using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Commands;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IPasswordService _passwordService;
    private readonly ILogger _logger;

    public LoginUserHandler(ILogger<LoginUserHandler> logger, IUnitOfWork unitOfWork, IAuthService authService, IPasswordService passwordService)
    {
        _userRepository = Ensure.IsNotNull(unitOfWork.User);
        _authService = Ensure.IsNotNull(authService);
        _passwordService = Ensure.IsNotNull(passwordService);

        _logger = Ensure.IsNotNull(logger);
    }
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogError("Email or Password is incorrect.");
            throw new NotFoundException("Email or Password is incorrect.");
        }

        return _authService.GenerateToken(user);
    }
}
