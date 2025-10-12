using Asp.Versioning;
using FileGateway.Application;
using FileGateway.Application.Commands;
using FileGateway.Application.DTOs;
using FileGateway.Application.Queries;
using FileGateway.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FileGateway.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class TextSecretController : ControllerBase
{
    private readonly ILogger<TextSecretController> _logger;
    private readonly IMediator _mediator;

    public TextSecretController(ILogger<TextSecretController> logger, IMediator mediator)
    {
        _logger = Ensure.IsNotNull(logger);
        _mediator = Ensure.IsNotNull(mediator);
    }

    [HttpPost("text")]
    public async Task<IActionResult> Create(CreateTextSecretArgs args)
    {
        var apiResult = new ApiResult<string>();
        var request = new CreateTextSecretCommand(args.Content, args.DeleteAfterDownload, GetCurrentUserId(User));

        try
        {
            var token = await _mediator.Send(request);
            apiResult.Success = !string.IsNullOrWhiteSpace(token);
            apiResult.Data = token;
            apiResult.ErrorMessage = string.IsNullOrWhiteSpace(token) ? $"Failed to create content." : string.Empty;
        }
        catch (Exception ex)
        {
            apiResult.ErrorMessage = ex.Message;
            _logger.LogError(ex, ex.Message);
        }

        return Ok(apiResult);
    }

    [AllowAnonymous]
    [HttpGet("text/{token}")]
    public async Task<IActionResult> Get(string token)
    {
        var apiResult = new ApiResult<string>();
        var request = new GetTextQuery(token);

        try
        {
            var content = await _mediator.Send(request);
            apiResult.Success = !string.IsNullOrWhiteSpace(content);
            apiResult.Data = content;
            apiResult.ErrorMessage = string.IsNullOrWhiteSpace(content) ? $"Failed to create content." : string.Empty;
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
