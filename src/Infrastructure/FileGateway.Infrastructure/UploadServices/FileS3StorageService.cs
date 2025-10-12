using Amazon.S3;
using Amazon.S3.Model;
using FileGateway.Application.Abstractions;
using FileGateway.Domain;

namespace FileGateway.Infrastructure
{
    public class FileS3StorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public FileS3StorageService(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = Ensure.IsNotNull(s3Client);
            _bucketName = Ensure.IsNotNull(bucketName);
        }

        public async Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }

        public async Task<bool> UploadFileAsync(Stream stream, string filePath, string? contentType = null, CancellationToken cancellationToken = default)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                InputStream = stream,
                ContentType = contentType
            };

            var response = await _s3Client.PutObjectAsync(request, cancellationToken);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            var response = await _s3Client.DeleteObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK || response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
