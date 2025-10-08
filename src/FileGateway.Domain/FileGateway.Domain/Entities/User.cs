using System.ComponentModel.DataAnnotations;

namespace FileGateway.Domain.Entities;

public record User
{
    /// <summary>
    /// User Id
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email cannot be null or empty.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Name of user
    /// </summary>
    [Required(ErrorMessage = "Name cannot be null or empty.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Passwor hashed
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Date and time the user was created
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time the user was last modified
    /// </summary>
    public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///  A random value that must change whenever a user's credentials change (password changed, login removed)
    /// </summary>
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}
