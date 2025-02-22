using Aevo.ChallengeDev.WebApi.Core;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;

public record GetSala(Guid SalaId);

public class GetSalaHandler(Context context) : ICaseHandler<GetSala, SalaView>
{
    public async Task<Result<SalaView>> Handle(GetSala req, CancellationToken ct)
    {
        var sala = await context.Salas
            .Where(s => s.Id == req.SalaId)
            .FirstOrDefaultAsync(cancellationToken: ct);

        if (sala == null)
        {
            throw new Exception("Não foi possivel localizar a sala, tente novamente mais tarde");
        }

        var output = new SalaView
        {
            Id = sala.Id,
            Nome = sala.Nome,
            Descricao = sala.Descricao,
            Capacidade = sala.Capacidade,
            FusoHorario = sala.FusoHorario
        };

        return output;
    }
}