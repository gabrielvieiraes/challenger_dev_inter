using Aevo.ChallengeDev.WebApi.Core;
using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Aevo.CommonLib.Results;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;

public record EditarSalaReqBody
{
    public required string Nome { get; init; }
    public string? Descricao { get; init; }
    public required int Capacidade { get; init; }
    public required string FusoHorario { get; init; }

    public EditarSala ToEditarSala(Guid salaId)
    {
        return new EditarSala()
        {
            SalaId = salaId,
            Nome = Nome,
            Descricao = Descricao,
            Capacidade = Capacidade,
            FusoHorario = FusoHorario,
        };
    }
}

public record EditarSala : EditarSalaReqBody
{
    public required Guid SalaId { get; init; }
}


public class EditarSalaHandler(Context context) : ICaseHandler<EditarSala, Unit>
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

    public async Task<Result<Unit>> Handle(EditarSala req, CancellationToken ct)
    {
        var sala = await context.Salas.FindAsync([req.SalaId], ct);

        if (sala == null)
        {
            return Result.NotFound();
        }
        
        sala.Nome = req.Nome;
        sala.Descricao = req.Descricao;
        sala.Capacidade = req.Capacidade;
        sala.FusoHorario = req.FusoHorario;
        
        await context.SaveChangesAsync(ct);

        return Result.Ok();
    }
}