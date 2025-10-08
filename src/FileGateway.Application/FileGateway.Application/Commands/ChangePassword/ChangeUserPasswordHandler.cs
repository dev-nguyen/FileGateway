using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Commands;

public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger _loger;

    public ChangeUserPasswordHandler(ILogger<ChangeUserPasswordHandler> loger, IUnitOfWork unitOfWork, IPasswordService passwordService)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _userRepository = _unitOfWork.User;
        _passwordService = Ensure.IsNotNull(passwordService);

        _loger = Ensure.IsNotNull(loger);
    }

    public async Task Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null || !_passwordService.VerifyPassword(request.OldPassword, user.PasswordHash))
        {
            _loger.LogError("User not found.");
            throw new NotFoundException("User not found.");
        }

        var newPasswordHash = _passwordService.HasPassword(request.NewPassword);
        if (newPasswordHash == user.PasswordHash)
        {
            _loger.LogError("New password must be different from the old password.");
            throw new InvalidOperationException("New password must be different from the old password.");
        }

        user.PasswordHash = newPasswordHash;
        user.SecurityStamp = Guid.NewGuid().ToString();

        await _userRepository.UpdateAsync(user.Id, user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
