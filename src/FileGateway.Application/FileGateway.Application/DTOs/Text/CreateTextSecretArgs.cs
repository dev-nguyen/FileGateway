using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.DTOs;

public record CreateTextSecretArgs
{
    public bool DeleteAfterDownload { get; set; }

    [Required(ErrorMessage = "Content cannot be null or empty.")]
    public string Content { get; set; } = string.Empty;
}
