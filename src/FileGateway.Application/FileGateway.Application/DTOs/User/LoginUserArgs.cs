namespace FileGateway.Application.DTOs;

public record LoginUserArgs : BaseUserArgs
{
    public LoginUserArgs(string email, string password)
        : base(email, password)
    {
    }
}
