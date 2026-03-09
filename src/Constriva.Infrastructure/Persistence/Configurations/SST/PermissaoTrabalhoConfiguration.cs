using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class PermissaoTrabalhoConfiguration : IEntityTypeConfiguration<PermissaoTrabalho>
{
    public void Configure(EntityTypeBuilder<PermissaoTrabalho> b)
    {
        b.ToTable("PermissoesTrabalho");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.TipoTrabalho).HasMaxLength(100).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Local).HasMaxLength(200);
        b.Property(e => e.DescricaoServico).HasMaxLength(2000);
        b.Property(e => e.RiscosIdentificados).HasMaxLength(2000);
        b.Property(e => e.MedidasControle).HasMaxLength(2000);
        b.Property(e => e.EPIsNecessarios).HasMaxLength(1000);
        b.Property(e => e.EPCsNecessarios).HasMaxLength(1000);
        b.Property(e => e.ProcedimentosEmergencia).HasMaxLength(2000);
        b.Property(e => e.NomeExecutante).HasMaxLength(200);
        b.Property(e => e.ObservacoesFechamento).HasMaxLength(2000);
        b.HasMany(e => e.Checklist).WithOne(i => i.Permissao).HasForeignKey(i => i.PermissaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
