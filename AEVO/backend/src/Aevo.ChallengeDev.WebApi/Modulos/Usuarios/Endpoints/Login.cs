using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Core.Auth;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;

public class Login
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Idioma { get; init; }
}

public record LoginResponse
{
    public required string AccessToken { get; init; }
    public required Guid UsuarioId { get; init; }
    public required string Idioma { get; init; }
}

public class LoginHandler(Context context, IAuthService authService) : ICaseHandler<Login, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(Login req, CancellationToken ct)
    {
        var user = await context.Usuarios
            .Where(x => x.Email == req.Email)
            .Select(u => new
            {
                u.Id,
                u.PasswordHash,
                u.Idioma
            })
            .FirstOrDefaultAsync(cancellationToken: ct);

        if (user == null)
        {
            return Result.Unauthorized();
        }

        var correctPassword = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);

        if (!correctPassword)
        {
            return Result.Unauthorized();
        }

        var token = authService.GenerateAccessToken(user.Id);

        return new LoginResponse
        {
            AccessToken = token,
            UsuarioId = user.Id,
            Idioma = user.Idioma
        };
    }
}