using Aevo.ChallengeDev.WebApi.Modulos.Salas;
using Aevo.ChallengeDev.WebApi.Modulos.Salas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aevo.ChallengeDev.WebApi.Core.EfConfigs;

public class SalaEntityConfiguration : IEntityTypeConfiguration<Sala>
{
    public void Configure(EntityTypeBuilder<Sala> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Nome)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(x => x.Descricao)
            .HasMaxLength(4000)
            .IsRequired(false);
        
        builder.Property(x => x.FusoHorario)
            .HasMaxLength(50)
            .IsRequired();
    }
}