using System.ComponentModel.DataAnnotations;

namespace FileGateway.Application.DTOs;

public record GetAwsFileArgs
{
    [Required(ErrorMessage = "Token cannot be null or empty.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "BucketName cannot be null or empty.")]
    public string BucketName { get; set; } = string.Empty;
}
