using FileGateway.Application.Commands;

namespace FileGateway.Application.Services.Abstractions;

public interface IUserService
{
    public Task<bool> CreateAsync(RegisterUserCommand args, CancellationToken cancellationToken = default);
    public Task<bool> UpdateAsync(UpdateUserCommand args, CancellationToken cancellationToken = default);
    public Task<string> LoginAsync(LoginUserCommand args, CancellationToken cancellationToken = default);
}
