using FileGateway.Domain.Abstractions;
using System.Security.Claims;

namespace FileGateway.Api.Middleware;

public class SecurityStampValidatorMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityStampValidatorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tokenStamp = context.User.FindFirstValue("security_stamp");

        var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
        var userRepository = unitOfWork.User;

        if (userIdClaim is not null && tokenStamp is not null)
        {
            var userId = Guid.Parse(userIdClaim);
            var user = await userRepository.GetByIdAsync(userId);

            if (user is null || user.SecurityStamp != tokenStamp)
            {
                throw new UnauthorizedAccessException("Token invalidated.");
            }
        }

        await _next(context);
    }
}
