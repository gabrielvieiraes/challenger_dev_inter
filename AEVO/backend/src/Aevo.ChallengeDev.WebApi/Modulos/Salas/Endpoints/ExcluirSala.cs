using Aevo.ChallengeDev.WebApi.Core;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;

public record ExcluirSala(Guid SalaId);

public class ExcluirSalaHandler(Context context) : ICaseHandler<ExcluirSala, Unit>
{
    public async Task<Result<Unit>> Handle(ExcluirSala req, CancellationToken ct)
    {
        await context.Salas.Where(s => s.Id == req.SalaId)
            .ExecuteDeleteAsync(cancellationToken: ct);

        return Result.NoContent();
    }
}