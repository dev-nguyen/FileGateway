using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.Commands;

public record ChangeUserPasswordCommand : IRequest
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email cannot be null or empty.")]
    [EmailAddress(ErrorMessage = "Email is invalid.")]
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Old Password
    /// </summary>
    [Required(ErrorMessage = "Old password cannot be null or empty.")]
    [MinLength(8, ErrorMessage = "Old password must be at least 8 characters.")]
    public string OldPassword { get; init; } = string.Empty;

    /// <summary>
    /// New Password
    /// </summary>
    [Required(ErrorMessage = "New password cannot be null or empty.")]
    [MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
    public string NewPassword { get; init; } = string.Empty;
}
