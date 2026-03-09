using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RecursoAtividadeConfiguration : IEntityTypeConfiguration<RecursoAtividade>
{
    public void Configure(EntityTypeBuilder<RecursoAtividade> b)
    {
        b.ToTable("RecursosAtividade");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.TipoRecurso).HasMaxLength(30).IsRequired();
        b.Property(e => e.NomeRecurso).HasMaxLength(200).IsRequired();
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.CustoUnitario).HasPrecision(18, 2);
        b.Ignore(e => e.CustoTotal);
        b.HasOne(e => e.Atividade).WithMany(a => a.Recursos).HasForeignKey(e => e.AtividadeId).OnDelete(DeleteBehavior.Cascade);
    }
}
