using FileGateway.Application.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileGateway.Application.Commands;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(ILogger<RegisterUserHandler> logger, IUnitOfWork unitOfWork, IPasswordService passwordService)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _userRepository = Ensure.IsNotNull(_unitOfWork.User);
        _passwordService = Ensure.IsNotNull(passwordService);

        _logger = Ensure.IsNotNull(logger);
    }
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is not null)
        {
            _logger.LogError("Email already exists.");
            throw new InvalidOperationException("Email already exists.");
        }
        user = new User(request.Email, _passwordService.HasPassword(request.Password));
        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
