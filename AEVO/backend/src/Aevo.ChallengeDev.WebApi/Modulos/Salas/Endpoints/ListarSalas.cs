using Aevo.ChallengeDev.WebApi.Core;
using Aevo.CommonLib.Results;
using Microsoft.EntityFrameworkCore;

namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Endpoints;

public record ListarSalas
{
    private ListarSalas()
    {
    }

    public static ListarSalas Instance = new();
}

public record SalaView
{
    public required Guid Id { get; init; }
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public required int Capacidade { get; set; }
    public required string FusoHorario { get; set; }
}

public class ListarSalasHandler(Context context) : ICaseHandler<ListarSalas, SalaView[]>
{
    public async Task<Result<SalaView[]>> Handle(ListarSalas req, CancellationToken ct)
    {
        return await context.Salas
            .Select(x => new SalaView
            {
                Id = x.Id,
                Nome = x.Nome,
                Descricao = x.Descricao,
                Capacidade = x.Capacidade,
                FusoHorario = x.FusoHorario
            })
            .ToArrayAsync(cancellationToken: ct);
    }
}