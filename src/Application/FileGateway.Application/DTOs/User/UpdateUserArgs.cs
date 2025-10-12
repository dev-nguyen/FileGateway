namespace FileGateway.Application.DTOs;

public record UpdateUserArgs
{
    public string? Password { get; private set; }

    public string? Name { get; private set; }

    public UpdateUserArgs(string? password, string? name)
    {
        Password = password;
        Name = name;
    }
}
