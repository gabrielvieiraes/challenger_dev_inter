using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;

namespace Aevo.ChallengeDev.WebApi.Modulos.Agendamentos.Models
{
    public class Agendamento
    {
        public required Guid Id { get; init; }
        public required Guid UsuarioId { get; set; }
        public required Guid SalaId { get; set; }
        public required DateTime Inicio { get; set; }
        public required DateTime Fim { get; set; }
        public DateTime CreationTime { get; init; }
        public required string Timezone { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Sala Sala { get; set; } = null!;
    }
}
