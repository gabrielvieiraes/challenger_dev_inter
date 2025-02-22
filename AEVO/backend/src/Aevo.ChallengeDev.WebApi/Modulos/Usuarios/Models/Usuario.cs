namespace Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;

public class Usuario
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Nome { get; init; }
    public required string Idioma { get; init; }
    public required string FusoHorario {get; init; }
    public required string PasswordHash { get; init; }
}