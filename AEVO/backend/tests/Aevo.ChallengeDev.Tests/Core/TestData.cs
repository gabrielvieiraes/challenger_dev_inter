using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;

namespace Aevo.ChallengeDev.Tests.Core;

public static class SalasDeTestePredefinidas
{
    public static readonly Sala SalaReuniaoSP = new()
    {
        Id = Guid.NewGuid(),
        Nome = "Sala de Reunião SP",
        Descricao = "Sala de reuniões na sede de São Paulo",
        Capacidade = 10,
        FusoHorario = "America/Sao_Paulo"
    };

    public static readonly Sala SalaTreinamentoNY = new()
    {
        Id = Guid.NewGuid(),
        Nome = "Sala de Treinamento NY",
        Descricao = "Sala para treinamentos e workshops em Nova York",
        Capacidade = 20,
        FusoHorario = "America/New_York"
    };

    public static readonly Sala AuditórioLondres = new()
    {
        Id = Guid.NewGuid(),
        Nome = "Auditório Londres",
        Descricao = "Auditório principal na filial de Londres",
        Capacidade = 50,
        FusoHorario = "Europe/London"
    };

    public static readonly Sala SalaExecutivaTóquio = new()
    {
        Id = Guid.NewGuid(),
        Nome = "Sala Executiva Tóquio",
        Descricao = "Sala de reuniões para executivos em Tóquio",
        Capacidade = 8,
        FusoHorario = "Asia/Tokyo"
    };

    public static readonly Sala SalaConferenciaBerlim = new()
    {
        Id = Guid.NewGuid(),
        Nome = "Sala de Conferência Berlim",
        Descricao = "Sala para conferências internacionais em Berlim",
        Capacidade = 15,
        FusoHorario = "Europe/Berlin"
    };

    public static List<Sala> ObterTodasAsSalas()
    {
        return
        [
            SalaReuniaoSP,
            SalaTreinamentoNY,
            AuditórioLondres,
            SalaExecutivaTóquio,
            SalaConferenciaBerlim
        ];
    }
}

public class UsuarioDeTeste : Usuario
{
    public required string Password { get; init; }

    
    public static UsuarioDeTeste CriarUsuario(string nome, string email, string idioma, string fusoHorario, string senha)
    {
        return new UsuarioDeTeste
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Email = email,
            Idioma = idioma,
            FusoHorario = fusoHorario,
            Password = senha,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(senha)
        };
    }
} 

public static class UsuariosDeTestePredefinidos
{
    public static readonly UsuarioDeTeste JoaoSilva = UsuarioDeTeste.CriarUsuario(
        "João Silva", "joao.silva@example.com", "pt-BR", "America/Sao_Paulo", "SenhaSegura123");

    public static readonly UsuarioDeTeste MariaSantos = UsuarioDeTeste.CriarUsuario(
        "Maria Santos", "maria.santos@example.com", "pt-BR", "America/Sao_Paulo", "SenhaForte456");

    public static readonly UsuarioDeTeste JohnDoe = UsuarioDeTeste.CriarUsuario(
        "John Doe", "john.doe@example.com", "en-US", "America/New_York", "StrongPass789");

    public static readonly UsuarioDeTeste EmilyJohnson = UsuarioDeTeste.CriarUsuario(
        "Emily Johnson", "emily.johnson@example.com", "en-US", "America/Los_Angeles", "MySecurePwd321");

    public static readonly UsuarioDeTeste AliceMuller = UsuarioDeTeste.CriarUsuario(
        "Alice Müller", "alice.muller@example.com", "de-DE", "Europe/Berlin", "Passwort123");

    public static readonly UsuarioDeTeste PeterSchmidt = UsuarioDeTeste.CriarUsuario(
        "Peter Schmidt", "peter.schmidt@example.com", "de-DE", "Europe/Berlin", "SehrSicher456");

    public static readonly UsuarioDeTeste YukiTanaka = UsuarioDeTeste.CriarUsuario(
        "Yuki Tanaka", "yuki.tanaka@example.com", "ja-JP", "Asia/Tokyo", "TanakaPassword");

    public static readonly UsuarioDeTeste HiroshiKobayashi = UsuarioDeTeste.CriarUsuario(
        "Hiroshi Kobayashi", "hiroshi.kobayashi@example.com", "ja-JP", "Asia/Tokyo", "HiroshiSecure");

    public static readonly UsuarioDeTeste SophieDubois = UsuarioDeTeste.CriarUsuario(
        "Sophie Dubois", "sophie.dubois@example.com", "fr-FR", "Europe/Paris", "MotDePasse123");

    public static readonly UsuarioDeTeste PierreLemoine = UsuarioDeTeste.CriarUsuario(
        "Pierre Lemoine", "pierre.lemoine@example.com", "fr-FR", "Europe/Paris", "SuperSecure987");

    public static List<Usuario> ObterTodosOsUsuarios()
    {
        return
        [
            JoaoSilva,
            MariaSantos,
            JohnDoe,
            EmilyJohnson,
            AliceMuller,
            PeterSchmidt,
            YukiTanaka,
            HiroshiKobayashi,
            SophieDubois,
            PierreLemoine
        ];
    }
}