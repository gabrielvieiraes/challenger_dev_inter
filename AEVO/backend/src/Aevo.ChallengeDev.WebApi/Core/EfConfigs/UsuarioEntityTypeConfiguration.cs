using Aevo.ChallengeDev.WebApi.Modulos.Usuarios;
using Aevo.ChallengeDev.WebApi.Modulos.Usuarios.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aevo.ChallengeDev.WebApi.Core.EfConfigs;

public class UsuarioEntityTypeConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasMaxLength(256);
        
        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(256);

        builder.Property(x => x.Idioma)
            .HasMaxLength(50);

        builder.Property(x => x.FusoHorario)
            .HasMaxLength(50);
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}