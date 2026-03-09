using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RequisicaoMaterialConfiguration : IEntityTypeConfiguration<RequisicaoMaterial>
{
    public void Configure(EntityTypeBuilder<RequisicaoMaterial> b)
    {
        b.ToTable("RequisicoesMaterial");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Motivo).HasMaxLength(500).IsRequired();
        b.Property(e => e.MotivoRejeicao).HasMaxLength(500);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Itens).WithOne(i => i.Requisicao).HasForeignKey(i => i.RequisicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
