namespace FileGateway.Application.DTOs;

public record FileDownloadResult
{
    public Stream FileStream { get; set; }
    public string ContentType { get; set; }

    public FileDownloadResult(Stream fileStream, string contenType)
    {
        FileStream = fileStream;
        ContentType = contenType;
    }
}
