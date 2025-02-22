using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;
using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;

public record RegistrarUsuario
{
    public required string Email { get; init; }
    public required string Nome { get; init; }
    public required string Idioma { get; init; }
    public required string FusoHorario { get; init; }
    public required string Password { get; init; }
}

public record RegistrarUsuarioResponse
{
    public required Guid UsuarioId { get; init; }
    public required string Idioma { get; init; }
}

public class RegistrarUsuarioHandler(Context context) : ICaseHandler<RegistrarUsuario, RegistrarUsuarioResponse>
{
    public async Task<Result<RegistrarUsuarioResponse>> Handle(RegistrarUsuario req, CancellationToken ct)
    {
        var result = ValidateInput(req);

        if (!result.IsSuccess)
        {
            return Result.Invalid(new AppError() { ErrorMessage = result.AsError().Errors?.ToString() });
        }

        var usuario = new Usuario()
        {
            Email = req.Email,
            Nome = req.Nome,
            Idioma = req.Idioma,
            FusoHorario = req.FusoHorario,
            Id = Guid.NewGuid(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
        };

        context.Usuarios.Add(usuario);

        await context.SaveChangesAsync(ct);

        return new RegistrarUsuarioResponse
        {
            UsuarioId = usuario.Id,
            Idioma = usuario.Idioma
        };
    }

    private Result ValidateInput(RegistrarUsuario req)
    {
        if (req == null)
        {
            return Result.Invalid();
        }

        if (String.IsNullOrEmpty(req.Idioma))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado o idioma." });
        }

        if (String.IsNullOrEmpty(req.Email))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado o email." });
        }

        if (String.IsNullOrEmpty(req.FusoHorario))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado o fuso horario." });
        }

        if (String.IsNullOrEmpty(req.Nome))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado o nome do usuario." });
        }

        if (String.IsNullOrEmpty(req.Password))
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Não foi informado a senha do usuario." });
        }

        var existUserWithSameEmail = context.Usuarios.Any(x => x.Email == req.Email);

        if (existUserWithSameEmail)
        {
            return Result.Invalid(new AppError() { ErrorMessage = "Já existe um usuario cadastrado com esse email" });
        }

        return Result.Ok();
    }
}