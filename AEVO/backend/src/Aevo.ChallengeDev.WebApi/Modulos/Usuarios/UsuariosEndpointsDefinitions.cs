using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Endpoints;
using Aevo.CommonLib.Results.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Aevo.ChallengeDev.WebApi.Modulos.Usuarios;

public static class UsuariosEndpointsDefinitions
{
    private static async Task<IResult> RegistrarUsuarioEndpoint([FromServices] RegistrarUsuarioHandler handler,
        [FromBody] RegistrarUsuario req, CancellationToken ct = default)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }
    
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