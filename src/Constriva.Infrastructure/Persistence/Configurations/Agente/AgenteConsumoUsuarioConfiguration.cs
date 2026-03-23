using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteConsumoUsuarioConfiguration : IEntityTypeConfiguration<AgenteConsumoUsuario>
{
    public void Configure(EntityTypeBuilder<AgenteConsumoUsuario> b)
    {
        b.ToTable("AgenteConsumoUsuario");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.EmpresaId, e.UsuarioId, e.Ano, e.Mes }).IsUnique();
    }
}
