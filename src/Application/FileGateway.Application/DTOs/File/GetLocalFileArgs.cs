using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.DTOs;

public record GetLocalFileArgs
{
    [Required(ErrorMessage = "Token cannot be null or empty.")]
    public string Token { get; set; } = string.Empty;
}
