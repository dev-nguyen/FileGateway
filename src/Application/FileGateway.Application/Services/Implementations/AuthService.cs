using FileGateway.Application.Services.Abstractions;
using FileGateway.Domain;
using FileGateway.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileGateway.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly JwtSettings _settings;

    public AuthService(JwtSettings settings)
    {
        _settings = Ensure.IsNotNull(settings);
    }
    public string GenerateToken(User user)
    {
        var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("security_stamp", user.SecurityStamp)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _settings.Issuer,
            _settings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
