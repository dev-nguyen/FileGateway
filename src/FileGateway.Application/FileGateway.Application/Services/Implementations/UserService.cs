using FileGateway.Application.Abstractions;
using FileGateway.Application.Commands;
using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;

namespace FileGateway.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IAuthService _authService;

    public UserService(IUnitOfWork unitOfWork, IPasswordService passwordService, IAuthService authService)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _userRepository = Ensure.IsNotNull(_unitOfWork.User);
        _passwordService = Ensure.IsNotNull(passwordService);
        _authService = Ensure.IsNotNull(authService);
    }

    public async Task<bool> CreateAsync(RegisterUserCommand args, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(args.Email, cancellationToken);
        if (user is not null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        user = new User
        {
            Email = args.Email,
            Name = args.Name,
            PasswordHash = _passwordService.HashPassword(args.Password)
        };
        await _userRepository.CreateAsync(user, cancellationToken);
        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<string> LoginAsync(LoginUserCommand args, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(args.Email, cancellationToken);
        if (user is null || !_passwordService.VerifyPassword(args.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email or Password is incorrect.");
        }
        return _authService.GenerateToken(user);
    }

    public async Task<bool> UpdateAsync(UpdateUserCommand args, CancellationToken cancellationToken = default)
    {
        var shouldUpdate = false;
        var user = await _userRepository.GetByEmailAsync(args.Email, cancellationToken);
        if (user is null)
        {
            return false;
        }

        if (ShouldUpdatePassword(user, args.Password))
        {
            user.PasswordHash = _passwordService.HashPassword(args.Password!);
            user.SecurityStamp = Guid.NewGuid().ToString("N");
            shouldUpdate = true;
        }

        if (!string.IsNullOrWhiteSpace(args.Name))
        {
            user.Name = args.Name;
            shouldUpdate = true;
        }

        if (shouldUpdate)
        {
            await _userRepository.UpdateAsync(user.Id, user, cancellationToken);
            return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }
        return false;
    }

    private bool ShouldUpdatePassword(User user, string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        return !_passwordService.VerifyPassword(password, user.PasswordHash);
    }
}
