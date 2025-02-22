using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Aevo.ChallengeDev.WebApi.Core.Auth;

public interface IAuthService
{
    string GenerateAccessToken(Guid userId);
}

public class AuthService(IOptions<AuthConfig> authConfig, TimeProvider timeProvider) : IAuthService
{
    public string GenerateAccessToken(Guid userId)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiry = now.AddHours(authConfig.Value.AccessTokenHoursLifetime);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Value.AccessTokenKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "localhost",
            audience: "localhost",
            claims: claims,
            expires: expiry,
            signingCredentials: credentials,
            notBefore: now
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}