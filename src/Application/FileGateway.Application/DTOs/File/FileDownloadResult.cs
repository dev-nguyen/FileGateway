using FileGateway.Domain.Entities;

namespace FileGateway.Application.DTOs;

public record FileDownloadResult
{
    public Stream FileStream { get; set; }
    public Secret Secret { get; set; }

    public FileDownloadResult(Stream fileStream, Secret secret)
    {
        FileStream = fileStream;
        Secret = secret;
    }
}
