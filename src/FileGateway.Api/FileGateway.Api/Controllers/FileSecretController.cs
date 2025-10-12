using Asp.Versioning;
using FileGateway.Application;
using FileGateway.Application.Commands;
using FileGateway.Application.Queries;
using FileGateway.Domain;
using FileGateway.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;

namespace FileGateway.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class FileSecretController : ControllerBase
{
    private readonly ILogger<FileSecretController> _logger;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;

    public FileSecretController(ILogger<FileSecretController> logger, IMediator mediator, IWebHostEnvironment env)
    {
        _logger = Ensure.IsNotNull(logger);

        _mediator = Ensure.IsNotNull(mediator);
        _env = Ensure.IsNotNull(env);
    }

    [HttpPost("upload/local")]
    public async Task<ActionResult> UploadLocal(IFormFile file, bool deleteAfterDownload)
    {
        var provider = new FileExtensionContentTypeProvider();
        provider.TryGetContentType(file.FileName, out var contentType);

        var request = new CreateFileSecretCommand(file.OpenReadStream(),
            file.FileName,
            contentType ?? string.Empty,
            _env.ContentRootPath,
            deleteAfterDownload,
            StorageProvider.Local,
            GetCurrentUserId(User));

        var apiResult = new ApiResult<string>();
        try
        {
            var token = await _mediator.Send(request);
            apiResult.Success = true;
            apiResult.Data = token;
            apiResult.ErrorMessage = string.IsNullOrWhiteSpace(token) ? $"Failed to upload {file.FileName}." : string.Empty;
        }
        catch (Exception ex)
        {
            apiResult.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }

        return Ok(apiResult);
    }

    [HttpPost("upload/aws")]
    public async Task<ActionResult> UploadAws(IFormFile file, string bucketName, bool deleteAfterDownload)
    {
        var provider = new FileExtensionContentTypeProvider();
        provider.TryGetContentType(file.FileName, out var contentType);

        var request = new CreateFileSecretCommand(file.OpenReadStream(),
            file.FileName,
            contentType ?? string.Empty,
            _env.ContentRootPath,
            deleteAfterDownload,
            StorageProvider.Local,
            GetCurrentUserId(User),
            bucketName);

        var apiResult = new ApiResult<string>();
        try
        {
            var token = await _mediator.Send(request);
            apiResult.Success = true;
            apiResult.Data = token;
            apiResult.ErrorMessage = string.IsNullOrWhiteSpace(token) ? $"Failed to upload {file.FileName}." : string.Empty;
        }
        catch (Exception ex)
        {
            apiResult.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }

        return Ok(apiResult);
    }

    [AllowAnonymous]
    [HttpGet("file/local/{token}")]
    public async Task<IActionResult> DownloadLocal(string token)
    {
        var request = new GetFileQuery(token, _env.ContentRootPath, StorageProvider.Local);

        var apiResult = new ApiResult<string>();
        try
        {
            var result = await _mediator.Send(request);
            if (result is null)
            {
                apiResult.ErrorMessage = $"Failed to download file with token: {token}";
                return Ok(apiResult);
            }
            return File(result.FileStream, result.ContentType);
        }
        catch (Exception ex)
        {
            apiResult.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }
        return Ok(apiResult);
    }

    [AllowAnonymous]
    [HttpGet("file/aws/{token}/{bucketName}")]
    public async Task<IActionResult> DownloadAws(string token, string bucketName)
    {
        var request = new GetFileQuery(token, _env.ContentRootPath, StorageProvider.S3, bucketName);

        var apiResult = new ApiResult<string>();
        try
        {
            var result = await _mediator.Send(request);
            if (result is null)
            {
                apiResult.ErrorMessage = $"Failed to download file with token: {token}";
                return Ok(apiResult);
            }
            return File(result.FileStream, result.ContentType);
        }
        catch (Exception ex)
        {
            apiResult.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }
        return Ok(apiResult);
    }


    private static Guid GetCurrentUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Guid.Empty;
        }

        return userId;
    }
}
