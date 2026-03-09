using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class PlanoContasConfiguration : IEntityTypeConfiguration<PlanoContas>
{
    public void Configure(EntityTypeBuilder<PlanoContas> b)
    {
        b.ToTable("PlanoContas");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.HasOne(e => e.ContaPai).WithMany(e => e.SubContas).HasForeignKey(e => e.ContaPaiId).OnDelete(DeleteBehavior.Restrict);
    }
}
