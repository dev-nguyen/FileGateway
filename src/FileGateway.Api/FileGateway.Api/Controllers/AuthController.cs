using Asp.Versioning;
using FileGateway.Application.Commands;
using FileGateway.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileGateway.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = Ensure.IsNotNull(mediator);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand request)
    {
        var apiResult = new ApiResult<string>();
        try
        {
            apiResult.Success = true;
            apiResult.Data = await _mediator.Send(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.Message = ex.Message;
        }
        return Ok(apiResult);
    }

    [HttpPost("regiter")]
    public async Task<IActionResult> Register(RegisterUserCommand request)
    {
        var apiResult = new ApiResult<string>();
        try
        {
            await _mediator.Send(request);
            apiResult.Success = true;
            apiResult.Message = $"{request.Email} is created successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.Message = ex.Message;
        }
        return Ok(apiResult);
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordCommand request)
    {
        var apiResult = new ApiResult<string>();
        try
        {
            await _mediator.Send(request);
            apiResult.Success = true;
            apiResult.Message = $"Password is changed successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            apiResult.Message = ex.Message;
        }
        return Ok(apiResult);
    }
}
