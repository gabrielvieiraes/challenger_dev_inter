using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;

public record CriarSala
{
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public required int Capacidade { get; init; }
    public required string FusoHorario { get; init; }
}

public record CriarSalaResponse
{
    public required Guid SalaId { get; init; }
}

public class CriarSalaHandler(Context context) : ICaseHandler<CriarSala, CriarSalaResponse>
{
    public async Task<Result<CriarSalaResponse>> Handle(CriarSala req, CancellationToken ct)
    {
        var sala = new Sala()
        {
            Id = Guid.NewGuid(),
            Nome = req.Nome,
            Descricao = req.Descricao,
            Capacidade = req.Capacidade,
            FusoHorario = req.FusoHorario
        };
        
        context.Salas.Add(sala);
        
        await context.SaveChangesAsync(ct);

        return new CriarSalaResponse()
        {
            SalaId = sala.Id,
        };
    }
}