namespace FileGateway.Application.Abstractions;

public interface IPasswordService
{
    public string HasPassword(string password);
    public bool VerifyPassword(string password, string passwordHash);
}
