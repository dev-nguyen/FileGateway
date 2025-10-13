using FileGateway.Application.DTOs;
using FileGateway.Domain.Enums;
using MediatR;

namespace FileGateway.Application.Queries;

public record GetFileQuery : IRequest<FileDownloadResult?>
{
    public string Token { get; set; }
    public StorageProvider StorageProvider { get; set; }
    public string BucketName { get; set; }
    public string AbsolutePath { get; set; }

    public GetFileQuery(string token, string absolutePath, StorageProvider storageProvider, string bucketName = "")
    {
        Token = token;
        AbsolutePath = absolutePath;
        StorageProvider = storageProvider;
        BucketName = bucketName;
    }
}
