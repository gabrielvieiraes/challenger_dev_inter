namespace Aevo.ChallengeDev.WebApi.Core.Auth;

public record AuthConfig
{
    public required string AccessTokenKey { get; set; }
    public required int AccessTokenHoursLifetime { get; set; } = 12;
}