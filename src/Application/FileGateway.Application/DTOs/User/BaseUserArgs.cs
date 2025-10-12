using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.DTOs;

public record BaseUserArgs
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email cannot be null or empty.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; private set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password cannot be null or empty.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; private set; }

    public BaseUserArgs(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
