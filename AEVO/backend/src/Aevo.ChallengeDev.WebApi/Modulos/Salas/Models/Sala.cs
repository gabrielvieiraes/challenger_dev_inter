namespace Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;

public class Sala
{
    public required Guid Id { get; init; }
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public required int Capacidade { get; set; }
    public required string FusoHorario { get; set; }
}