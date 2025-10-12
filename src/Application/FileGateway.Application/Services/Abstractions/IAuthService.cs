using FileGateway.Domain.Entities;

namespace FileGateway.Application.Services.Abstractions;

public interface IAuthService
{
    public string GenerateToken(User user);
}
