using Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;
using Aevo.CommonLib.Results.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas;

public static class SalasEndpointsDefinitions
{
    /// <summary>
    /// Endpoint para criar uma nova sala no sistema.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de criação de sala.</param>
    /// <param name="req">Dados necessários para a criação da sala.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna a resposta com o ID da sala criada.</returns>
    [SwaggerOperation(
        Summary = "Criar uma nova sala",
        Description = "Este endpoint permite a criação de uma nova sala no sistema. O usuário deve fornecer o nome, a capacidade e o fuso horário da sala.",
        Tags = new[] { "Salas" }
    )]
    [SwaggerResponse(200, "Sala criada com sucesso.", typeof(CriarSalaResponse))]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados informados.")]
    private static async Task<IResult> CriarSalaEndpoint([FromServices] CriarSalaHandler handler,
        [FromBody] CriarSala req, CancellationToken ct = default)
    {
        return (await handler.Handle(req, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para obter os detalhes de uma sala específica.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de recuperação dos detalhes da sala.</param>
    /// <param name="salaId">ID da sala que se deseja obter os detalhes.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna os detalhes da sala, incluindo nome, capacidade e fuso horário.</returns>
    [SwaggerOperation(
        Summary = "Obter detalhes de uma sala",
        Description = "Este endpoint permite recuperar os detalhes de uma sala específica utilizando seu ID. Retorna informações como nome, capacidade e fuso horário.",
        Tags = new[] { "Salas" }
    )]
    [SwaggerResponse(200, "Detalhes da sala recuperados com sucesso.", typeof(SalaView))]
    [SwaggerResponse(404, "Sala não encontrada.")]
    [SwaggerResponse(500, "Erro interno no servidor.")]
    private static async Task<IResult> GetSalaEndpoint([FromServices] GetSalaHandler handler,
        [FromRoute] Guid salaId, CancellationToken ct = default)
    {
        return (await handler.Handle(new GetSala(salaId), ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para editar os detalhes de uma sala existente.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de edição de sala.</param>
    /// <param name="salaId">ID da sala que será editada.</param>
    /// <param name="req">Dados a serem atualizados para a sala.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna um resultado indicando o sucesso ou falha da operação.</returns>
    [SwaggerOperation(
        Summary = "Editar os detalhes de uma sala",
        Description = "Este endpoint permite editar os detalhes de uma sala existente. É necessário fornecer o ID da sala e os dados atualizados, como nome, descrição, capacidade e fuso horário.",
        Tags = new[] { "Salas" }
    )]
    [SwaggerResponse(200, "Sala editada com sucesso.")]
    [SwaggerResponse(404, "Sala não encontrada.")]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados informados.")]
    private static async Task<IResult> EditarSalaEndpoint([FromServices] EditarSalaHandler handler,
        [FromRoute] Guid salaId, [FromBody] EditarSalaReqBody req, CancellationToken ct = default)
    {
        return (await handler.Handle(req.ToEditarSala(salaId), ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para excluir uma sala específica.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de exclusão de sala.</param>
    /// <param name="salaId">ID da sala que será excluída.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna um resultado indicando o sucesso ou falha da operação.</returns>
    [SwaggerOperation(
        Summary = "Excluir uma sala",
        Description = "Este endpoint permite excluir uma sala do sistema. Para realizar a exclusão, é necessário fornecer o ID da sala.",
        Tags = new[] { "Salas" }
    )]
    [SwaggerResponse(204, "Sala excluída com sucesso.")]
    [SwaggerResponse(404, "Sala não encontrada.")]
    [SwaggerResponse(400, "Requisição inválida.")]
    private static async Task<IResult> ExcluirSalaEndpoint([FromServices] ExcluirSalaHandler handler,
        [FromRoute] Guid salaId, CancellationToken ct = default)
    {
        return (await handler.Handle(new ExcluirSala(salaId), ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para listar todas as salas.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de listagem de salas.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna uma lista com os detalhes de todas as salas cadastradas.</returns>
    [SwaggerOperation(
        Summary = "Listar todas as salas",
        Description = "Este endpoint permite listar todas as salas cadastradas no sistema. Ele retorna uma lista com detalhes como nome, descrição, capacidade e fuso horário de cada sala.",
        Tags = new[] { "Salas" }
    )]
    [SwaggerResponse(200, "Lista de salas retornada com sucesso.", typeof(SalaView[]))]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
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