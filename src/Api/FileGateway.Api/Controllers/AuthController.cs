using Asp.Versioning;
using FileGateway.Application;
using FileGateway.Application.Commands;
using FileGateway.Application.DTOs;
using FileGateway.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        _logger = Ensure.IsNotNull(logger);
        _mediator = Ensure.IsNotNull(mediator);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserArgs args)
    {
        var apiResult = new ApiResult<string>();
        try
        {
            var request = new LoginUserCommand(args.Email, args.Password);
            var token = await _mediator.Send(request);

            apiResult.Success = true;
            apiResult.Data = token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed.");
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserArgs args)
    {
        var apiResult = new ApiResult<string>();

        try
        {
            var request = new RegisterUserCommand(args.Email, args.Password, args.Name);
            var success = await _mediator.Send(request);

            apiResult.Success = success;
            apiResult.Data = success ? $"{args.Email} is created successfully." : $"Failed to create {args.Email}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user.");
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserArgs args)
    {
        var apiResult = new ApiResult<string>();
        try
        {
            var currentEmail = User.FindFirst(ClaimTypes.Email)?.Value ??
                throw new UnauthorizedAccessException("Please login before executing this action.");

            var request = new UpdateUserCommand(currentEmail, args.Password, args.Name);
            var success = await _mediator.Send(request);

            apiResult.Success = success;
            apiResult.Data = success ? $"{currentEmail} is updated successfully." : $"Failed to update {currentEmail}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user.");
            apiResult.ErrorMessage = ex.Message;
        }

        return Ok(apiResult);
    }
}
