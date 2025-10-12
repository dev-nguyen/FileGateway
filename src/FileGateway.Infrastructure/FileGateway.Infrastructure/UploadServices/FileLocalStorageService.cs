using FileGateway.Application.Abstractions;

namespace FileGateway.Infrastructure
{
    public class FileLocalStorageService : IFileStorageService
    {
        public Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(fileStream);
        }

        public async Task<bool> UploadFileAsync(Stream stream, string filePath, string? contentType = null, CancellationToken cancellationToken = default)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream, cancellationToken);
            await fileStream.FlushAsync(cancellationToken);

            return File.Exists(filePath) && new FileInfo(filePath).Length > 0;
        }

        public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.FromResult(File.Exists(filePath));
        }
    }
}
