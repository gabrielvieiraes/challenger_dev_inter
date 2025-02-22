namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos;

public record UsuarioSimplificadoView
{
    public required Guid Id { get; init; }
    public required string Nome { get; init; }
}

public record SalaSimplificadaView
{
    public required Guid Id { get; init; }
    public required string Nome { get; init; }
}

public class AgendamentoView
{
    public required Guid Id { get; init; }
    public required UsuarioSimplificadoView Usuario { get; init; }
    public required SalaSimplificadaView Sala { get; init; }
    public required DateTime Inicio { get; init; }
    public required DateTime Fim { get; init; }
    public required string Timezone { get; set; }
}