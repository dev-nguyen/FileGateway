using Asp.Versioning;
using FileGateway.Application.Commands;
using FileGateway.Application.Queries;
using FileGateway.Domain;
using FileGateway.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FileGateway.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class SecretController : ControllerBase
{
    private readonly ILogger<SecretController> _logger;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;

    public SecretController(ILogger<SecretController> logger, IMediator mediator, IWebHostEnvironment env)
    {
        _logger = Ensure.IsNotNull(logger);
        _mediator = Ensure.IsNotNull(mediator);
        _env = Ensure.IsNotNull(env);
    }

    [Authorize]
    [HttpPost("file/local")]
    public async Task<IActionResult> UploadFileLocal(IFormFile file, bool deleteAfterDownload)
    {
        var apiResult = new ApiResult<string>();
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        try
        {
            var userId = GetCurrentUserId(HttpContext);
            if (userId == Guid.Empty)
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }

            var request = BuildUploadInfo(file, userId, StorageProvider.Local, deleteAfterDownload);
            var result = await _mediator.Send(request);

            apiResult.Success = true;
            if (result)
            {
                apiResult.Data = "File is uploaded successfully.";
            }
            else
            {
                apiResult.Data = $"Failed to upload file: {file.FileName}.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }

    [Authorize]
    [HttpPost("file/s3")]
    public async Task<IActionResult> UploadFileS3(IFormFile file, string bucketName, bool deleteAfterDownload)
    {
        var apiResult = new ApiResult<string>();
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        try
        {
            var userId = GetCurrentUserId(HttpContext);
            if (userId == Guid.Empty)
            {
                return Unauthorized("Invalid or missing user ID in token.");
            }
            var request = BuildUploadInfo(file, userId, StorageProvider.S3, deleteAfterDownload, bucketName);
            var result = await _mediator.Send(request);

            apiResult.Success = true;
            if (result)
            {
                apiResult.Data = "File is uploaded successfully.";
            }
            else
            {
                apiResult.Data = $"Failed to upload file: {file.FileName}.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }

    [AllowAnonymous]
    [HttpGet("file/local/{token}")]
    public async Task<IActionResult> DownloadLocalFile([Required] string token)
    {
        var secreteFileFileQuery = new SecretFileQuery
        {
            Token = token,
            FolderPath = _env.ContentRootPath
        };
        var (stream, item) = await _mediator.Send(secreteFileFileQuery);
        if (stream is null && item is null)
        {
            return Ok(new ApiResult<string>
            {
                Success = true,
                Data = "File not found."
            });
        }

        if (item is not null && item.Status == FileStatus.InProcess)
        {
            return Ok(new ApiResult<string>
            {
                Success = true,
                Data = "Server is busy. Please try again later."
            });
        }
        return File(stream!, item!.ContentType, item.FileName);
    }

    [Authorize]
    [HttpPost("text")]
    public async Task<IActionResult> CreateText([Required(ErrorMessage = "Content cannot null or empty.")] string content, bool deleteAfterDownload)
    {
        var apiResult = new ApiResult<string>();

        var userId = GetCurrentUserId(HttpContext);
        if (userId == Guid.Empty)
        {
            return Unauthorized("Invalid or missing user ID in token.");
        }

        var request = new CreateTextCommand
        {
            Content = content,
            DeleteAfterDownload = deleteAfterDownload,
            Onwer = userId
        };
        try
        {
            await _mediator.Send(request);
            apiResult.Success = true;
            apiResult.Data = "Content is created successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }


    private UploadSecretCommand BuildUploadInfo(IFormFile file, Guid owner, StorageProvider storageProvider, bool deleteAfterDownload, string? bucketName = null)
    {
        var provider = new FileExtensionContentTypeProvider();
        provider.TryGetContentType(file.FileName, out var contentType);
        return new UploadSecretCommand
        {
            FileStream = file.OpenReadStream(),
            FileSize = file.Length,
            ContentType = contentType ?? string.Empty,
            DeleteAfterDownload = deleteAfterDownload,
            FileName = file.FileName,
            AbsolutePath = _env.ContentRootPath,
            Provider = storageProvider,
            BucketName = bucketName ?? string.Empty,
            Owner = owner
        };
    }

    private static Guid GetCurrentUserId(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Guid.Empty;
        }

        return userId;
    }
}
