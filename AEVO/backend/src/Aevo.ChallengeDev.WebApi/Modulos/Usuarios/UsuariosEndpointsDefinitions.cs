using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;
using Aevo.CommonLib.Results.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Aevo.ChallengeDev.WebApi.Modulos.Usuarios;

public static class UsuariosEndpointsDefinitions
{
    /// <summary>
    /// Endpoint para registrar um novo usuário no sistema.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de registro de usuário.</param>
    /// <param name="req">Os dados necessários para o registro do usuário.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna a resposta com o ID do usuário e o idioma.</returns>
    [SwaggerOperation(
        Summary = "Registrar um novo usuário",
        Description = "Este endpoint permite o registro de um novo usuário no sistema. O usuário deve fornecer informações como email, nome, idioma, fuso horário e senha.",
        Tags = new[] { "Usuários" }
    )]
    [SwaggerResponse(200, "Usuário registrado com sucesso.", typeof(RegistrarUsuarioResponse))]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados informados.", typeof(CommonLib.Results.AppError))]
    [SwaggerResponse(409, "Já existe um usuário cadastrado com esse email.", typeof(CommonLib.Results.AppError))]
    private static async Task<IResult> RegistrarUsuarioEndpoint([FromServices] RegistrarUsuarioHandler handler,
        [FromBody] RegistrarUsuario req, CancellationToken ct = default)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para realizar o login de um usuário no sistema.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de login.</param>
    /// <param name="req">Dados necessários para o login do usuário.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna a resposta com o token de acesso e dados do usuário.</returns>
    [SwaggerOperation(
        Summary = "Login de um usuário",
        Description = "Este endpoint permite que um usuário faça login no sistema fornecendo seu e-mail, senha e idioma.",
        Tags = new[] { "Usuários" }
    )]
    [SwaggerResponse(200, "Login realizado com sucesso.", typeof(LoginResponse))]
    [SwaggerResponse(401, "Credenciais inválidas.")]
    private static async Task<IResult> LoginEndpoint([FromServices] LoginHandler handler,
        [FromBody] Login req, CancellationToken ct = default)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }

    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("usuarios", RegistrarUsuarioEndpoint)
            .AllowAnonymous();

        endpoints.MapPost("usuarios/login", LoginEndpoint)
            .AllowAnonymous();
    }
}