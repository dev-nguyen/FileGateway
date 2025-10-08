using FileGateway.Domain.Entities;

namespace FileGateway.Application.Abstractions;

public interface IAuthService
{
    public string GenerateToken(User user);
}
