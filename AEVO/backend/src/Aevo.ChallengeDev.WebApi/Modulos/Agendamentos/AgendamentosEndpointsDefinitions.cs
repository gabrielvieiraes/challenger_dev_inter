using Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;
using Aevo.CommonLib.Results.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos;

public static class AgendamentosEndpointsDefinitions
{
    /// <summary>
    /// Endpoint para criar um novo agendamento em uma sala.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de criação de agendamento.</param>
    /// <param name="salaId">ID da sala onde o agendamento será realizado.</param>
    /// <param name="body">Corpo da requisição contendo as informações do agendamento.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna o ID do agendamento criado.</returns>
    [SwaggerOperation(
        Summary = "Criar um novo agendamento",
        Description = "Este endpoint permite criar um agendamento para uma sala específica. Para isso, são necessários os dados como horário de início, fim, fuso horário, e o ID do usuário que está criando o agendamento.",
        Tags = new[] { "Agendamentos" }
    )]
    [SwaggerResponse(201, "Agendamento criado com sucesso.", typeof(CriarAgendamentoResponse))]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados fornecidos.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> CriarAgendamentoEndpoint([FromServices] CriarAgendamentoHandler handler,
        [FromRoute] Guid salaId, [FromBody] CriarAgendamento body, CancellationToken ct = default)
    {
        return (await handler.Handle(body, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para editar um agendamento existente.
    /// </summary>
    /// <param name="handler">Manipulador para processar a requisição de edição de agendamento.</param>
    /// <param name="agendamentoId">ID do agendamento a ser editado.</param>
    /// <param name="body">Corpo da requisição contendo as novas informações do agendamento.</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Retorna um status de sucesso ou erro ao editar o agendamento.</returns>
    [SwaggerOperation(
        Summary = "Editar um agendamento existente",
        Description = "Este endpoint permite editar os detalhes de um agendamento já existente, incluindo horário de início, fim, sala e usuário. O agendamento será editado apenas se o usuário que faz a requisição for o mesmo que criou o agendamento.",
        Tags = new[] { "Agendamentos" }
    )]
    [SwaggerResponse(200, "Agendamento editado com sucesso.")]
    [SwaggerResponse(400, "Requisição inválida. Verifique os dados fornecidos.")]
    [SwaggerResponse(403, "Não autorizado. O usuário não pode editar este agendamento.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> EditarAgendamentoEndpoint([FromServices] EditarAgendamentoHandler handler,
        [FromRoute] Guid agendamentoId, [FromBody] EditarAgendamento body, CancellationToken ct = default)
    {
        return (await handler.Handle(body, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para excluir um agendamento.
    /// </summary>
    /// <param name="handler">Manipulador que processa a requisição para excluir o agendamento.</param>
    /// <param name="agendamentoId">ID do agendamento a ser excluído.</param>
    /// <param name="usuarioId">ID do usuário que solicita a exclusão.</param>
    /// <param name="ct">Token de cancelamento para cancelar a requisição.</param>
    /// <returns>Retorna um status de sucesso ou erro ao tentar excluir o agendamento.</returns>
    [SwaggerOperation(
        Summary = "Excluir um agendamento",
        Description = "Este endpoint permite excluir um agendamento existente. A exclusão só pode ser realizada pelo usuário que criou o agendamento.",
        Tags = new[] { "Agendamentos" }
    )]
    [SwaggerResponse(200, "Agendamento excluído com sucesso.")]
    [SwaggerResponse(400, "Requisição inválida. O usuário não tem permissão para excluir o agendamento.")]
    [SwaggerResponse(404, "Agendamento não encontrado.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> ExcluirAgendamentoEndpoint([FromServices] ExcluiAgendamentoHandler handler,
        [FromRoute] Guid agendamentoId, [FromRoute] Guid usuarioId, CancellationToken ct = default)
    {
        return (await handler.Handle(new ExcluirAgendamento() { AgendamentoId = agendamentoId, UsuarioId = usuarioId }, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para recuperar todos os agendamentos de uma sala específica.
    /// </summary>
    /// <param name="handler">Manipulador que processa a requisição para listar os agendamentos da sala.</param>
    /// <param name="salaId">ID da sala para a qual os agendamentos são solicitados.</param>
    /// <param name="ct">Token de cancelamento para cancelar a requisição.</param>
    /// <returns>Lista de agendamentos da sala solicitada.</returns>
    [SwaggerOperation(
        Summary = "Recuperar agendamentos de uma sala",
        Description = "Este endpoint retorna todos os agendamentos de uma sala específica, ordenados pela data de início.",
        Tags = new[] { "Agendamentos", "Salas" }
    )]
    [SwaggerResponse(200, "Lista de agendamentos da sala retornada com sucesso.", typeof(List<AgendamentoView>))]
    [SwaggerResponse(400, "Requisição inválida.")]
    [SwaggerResponse(404, "Sala não encontrada.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> GetAgendamentosSalaEndpoint([FromServices] GetAgendamentosSalaHandler handler,
        [FromRoute] Guid salaId, CancellationToken ct = default)
    {
        return (await handler.Handle(new GetAgendamentosSala(salaId), ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para recuperar todos os agendamentos de um usuário específico (usuário logado).
    /// </summary>
    /// <param name="handler">Manipulador que processa a requisição para listar os agendamentos do usuário logado.</param>
    /// <param name="body">Objeto que contém o ID do usuário logado que solicitou os agendamentos.</param>
    /// <param name="ct">Token de cancelamento para cancelar a requisição.</param>
    /// <returns>Lista de agendamentos do usuário logado.</returns>
    [SwaggerOperation(
        Summary = "Recuperar agendamentos do usuário logado",
        Description = "Este endpoint retorna todos os agendamentos de um usuário específico, ordenados pela data de início.",
        Tags = new[] { "Agendamentos", "Usuários" }
    )]
    [SwaggerResponse(200, "Lista de agendamentos do usuário retornada com sucesso.", typeof(List<AgendamentoView>))]
    [SwaggerResponse(400, "Requisição inválida.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> GetAgendamentosUsuarioLogadoEndpoint(
        [FromServices] GetAgendamentosUsuarioLogadoHandler handler, [FromBody] GetAgendamentosUsuarioLogado body, CancellationToken ct = default)
    {
        return (await handler.Handle(body, ct)).ToApiResult();
    }

    /// <summary>
    /// Endpoint para recuperar um agendamento específico pelo seu ID.
    /// </summary>
    /// <param name="handler">Manipulador que processa a requisição para recuperar o agendamento.</param>
    /// <param name="agendamentoId">ID do agendamento que será recuperado.</param>
    /// <param name="ct">Token de cancelamento para cancelar a requisição.</param>
    /// <returns>Detalhes do agendamento solicitado.</returns>
    [SwaggerOperation(
        Summary = "Recuperar um agendamento específico pelo ID",
        Description = "Este endpoint retorna as informações de um agendamento específico, incluindo o horário de início, término, sala, e usuário associado.",
        Tags = new[] { "Agendamentos" }
    )]
    [SwaggerResponse(200, "Detalhes do agendamento retornados com sucesso.", typeof(AgendamentoView))]
    [SwaggerResponse(404, "Agendamento não encontrado.")]
    [SwaggerResponse(500, "Erro interno ao processar a requisição.")]
    private static async Task<IResult> GetAgendamentoEndpoint([FromServices] GetAgendamentoHandler handler,
        [FromRoute] Guid agendamentoId, CancellationToken ct = default)
    {
        return (await handler.Handle(new GetAgendamento(agendamentoId), ct)).ToApiResult();
    }


    public static void MapAgendamentosEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var agendamentosEndpoints = endpoints.MapGroup("agendamentos")
            .RequireAuthorization();

        agendamentosEndpoints.MapPost("", GetAgendamentosUsuarioLogadoEndpoint);

        agendamentosEndpoints.MapGet("salas/{salaId:guid}", GetAgendamentosSalaEndpoint);

        agendamentosEndpoints.MapPost("salas/{salaId:guid}", CriarAgendamentoEndpoint);

        agendamentosEndpoints.MapPut("{agendamentoId:guid}", EditarAgendamentoEndpoint);

        agendamentosEndpoints.MapDelete("{usuarioId:guid}/{agendamentoId:guid}", ExcluirAgendamentoEndpoint);

        agendamentosEndpoints.MapGet("{agendamentoId:guid}", GetAgendamentoEndpoint);
    }
}