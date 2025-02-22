using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Aevo.ChallengeDev.WebApi.Core.Auth;

public static class AppAuthDefaults
{
    public const string AuthenticationScheme = "AppAuth";
}

public class AppAuthOptions : AuthenticationSchemeOptions
{
    public static AppAuthOptions Instance = new();
}

public class AppAuthAuthenticationHandler(
    IOptionsMonitor<AppAuthOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    TimeProvider timeProvider,
    IOptions<AuthConfig> authConfig
) : AuthenticationHandler<AppAuthOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = GetTokenFromAuthorizationHeader(Request);
        
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }
        
        var (validatedToken, valid) = ValidateToken(token);

        if (!valid || validatedToken is null)
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, validatedToken.Subject, ClaimValueTypes.String),
            new(JwtRegisteredClaimNames.Sub, validatedToken.Subject, ClaimValueTypes.String),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new GenericPrincipal(identity, null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
    
    private static string? GetTokenFromAuthorizationHeader(HttpRequest request)
    {
        if (!request.Headers.TryGetValue("Authorization", out var value))
            return null;

        var authorizationHeader = value.ToString();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return null;
        }

        if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var token = authorizationHeader["bearer".Length..].Trim();

        return token;
    }

    private (JwtSecurityToken?, bool) ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Value.AccessTokenKey));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = "localhost",
            ValidateAudience = false,
            ValidateLifetime = true,
            LifetimeValidator = (notBefore, expires, _, _) =>
            {
                var now = timeProvider.GetUtcNow().UtcDateTime;
                return now >= notBefore && now <= expires;
            },
            ClockSkew = TimeSpan.Zero,
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            var jwtToken = (validatedToken as JwtSecurityToken)!;
            
            return (jwtToken, true);
        }
        catch (SecurityTokenException)
        {
            return (null, false);
        }
    }
}