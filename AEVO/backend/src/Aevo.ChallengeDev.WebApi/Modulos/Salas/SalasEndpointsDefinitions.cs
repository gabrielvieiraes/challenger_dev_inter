using Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;
using Aevo.CommonLib.Results.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas;

public static class SalasEndpointsDefinitions
{
    private static async Task<IResult> CriarSalaEndpoint([FromServices] CriarSalaHandler handler,
        [FromBody] CriarSala req, CancellationToken ct = default)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }

    private static async Task<IResult> GetSalaEndpoint([FromServices] GetSalaHandler handler,
        [FromRoute] Guid salaId, CancellationToken ct = default)
    {
        return (await handler.Handle(new GetSala(salaId), ct)).ToApiResult();
    }

    private static async Task<IResult> EditarSalaEndpoint([FromServices] EditarSalaHandler handler,
        [FromRoute] Guid salaId, [FromBody] EditarSalaReqBody req, CancellationToken ct = default)
    {
        return (await handler.Handle(req.ToEditarSala(salaId), ct)).ToApiResult();
    }

    private static async Task<IResult> ExcluirSalaEndpoint([FromServices] ExcluirSalaHandler handler,
        [FromRoute] Guid salaId, CancellationToken ct = default)
    {
        return (await handler.Handle(new ExcluirSala(salaId), ct)).ToApiResult();
    }

    private static async Task<IResult> ListarSalasEndpoint([FromServices] ListarSalasHandler handler,
        CancellationToken ct = default)
    {
        return (await handler.Handle(ListarSalas.Instance, ct)).ToApiResult();
    }


    public static void MapSalasEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var salasEndpoints = endpoints.MapGroup("salas")
            .RequireAuthorization();

        salasEndpoints.MapGet("", ListarSalasEndpoint);

        salasEndpoints.MapPost("", CriarSalaEndpoint);

        salasEndpoints.MapPut("{salaId:guid}", EditarSalaEndpoint);

        salasEndpoints.MapDelete("{salaId:guid}", ExcluirSalaEndpoint);

        salasEndpoints.MapGet("{salaId:guid}", GetSalaEndpoint);
    }
}