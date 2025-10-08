using FileGateway.Application.Abstractions;

namespace FileGateway.Infrastructure;

public class PasswordService : IPasswordService
{
    public string HasPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
