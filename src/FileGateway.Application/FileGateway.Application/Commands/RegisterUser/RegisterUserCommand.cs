using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.Commands;

public record RegisterUserCommand : IRequest
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email cannot be null or empty.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password cannot be null or empty.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; private set; } = string.Empty;

    public RegisterUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
