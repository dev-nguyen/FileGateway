namespace FileGateway.Application.Abstractions;

public interface IFileStorageService
{
    public Task<bool> UploadFileAsync(Stream stream, string contentType, string filePath, CancellationToken cancellationToken = default);
    public Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
    public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}
