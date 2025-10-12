namespace FileGateway.Application.DTOs;

public record RegisterUserArgs : BaseUserArgs
{
    public string Name { get; set; }

    public RegisterUserArgs(string email, string password, string name)
        : base(email, password)
    {
        Name = name;
    }
}
