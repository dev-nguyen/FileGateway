using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using MediatR;

namespace FileGateway.Application.Commands;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = Ensure.IsNotNull(userService);
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdateAsync(request, cancellationToken);
    }
}
