using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Models;
using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Aevo.ChallengeDev.WebApi.Utils;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;

public record GetAgendamento(Guid AgendamentoId);

public class GetAgendamentoHandler(Context context) : ICaseHandler<GetAgendamento, AgendamentoView>
{
    public async Task<Result<AgendamentoView>> Handle(GetAgendamento req, CancellationToken ct)
    {
        try
        {
            var agendamento = context.Agendamentos
                                    .Include(a => a.Usuario)
                                    .Include(a => a.Sala)
                                    .Where(x => x.Id == req.AgendamentoId)
                                    .FirstOrDefault();
            if (agendamento == null)
            {
                throw new Exception("Não foi possivel localizar o agendamento, tente novamente mais tarde");
            }

            return new AgendamentoView()
            {
                Id = agendamento.Id,
                Inicio = TimezoneUtil.ConvertDateToTimezone(agendamento.Inicio, agendamento.Timezone),
                Fim = TimezoneUtil.ConvertDateToTimezone(agendamento.Fim, agendamento.Timezone),
                Sala = new SalaSimplificadaView()
                {
                    Id = agendamento.SalaId,
                    Nome = agendamento.Sala?.Nome
                },
                Usuario = new UsuarioSimplificadoView()
                {
                    Id = agendamento.UsuarioId,
                    Nome = agendamento.Usuario?.Nome
                },
                Timezone = agendamento.Timezone,
            };
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possível recuperar os agendamentos, tente novamente mais tarde.");
        }
    }
}