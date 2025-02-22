using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Utils;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Endpoints;

public record GetAgendamentosUsuarioLogado
{
    public Guid UsuarioId { get; set; }
}

public class GetAgendamentosUsuarioLogadoHandler(Context context) : ICaseHandler<GetAgendamentosUsuarioLogado, List<AgendamentoView>>
{
    public async Task<Result<List<AgendamentoView>>> Handle(GetAgendamentosUsuarioLogado req, CancellationToken ct)
    {
        try
        {
            var output = new List<AgendamentoView>();

            var agendamentos = context.Agendamentos
                                    .Include(a => a.Usuario)
                                    .Include(a => a.Sala)
                                    .Where(x => x.UsuarioId == req.UsuarioId)
                                    .OrderBy(x => x.Inicio)
                                    .ToList();

            foreach (var agendamento in agendamentos)
            {
                output.Add(new AgendamentoView()
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
                    Timezone = agendamento.Timezone
                }
                );
            }

            return output;
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possível recuperar os agendamentos, tente novamente mais tarde.");
        }
    }
}