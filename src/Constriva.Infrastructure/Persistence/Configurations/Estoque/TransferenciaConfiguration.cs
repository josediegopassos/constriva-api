using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class TransferenciaConfiguration : IEntityTypeConfiguration<Transferencia>
{
    public void Configure(EntityTypeBuilder<Transferencia> b)
    {
        b.ToTable("Transferencias");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Valor).HasPrecision(18, 2);
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.Property(e => e.Comprovante).HasMaxLength(1000);
        b.HasOne(e => e.ContaOrigem).WithMany(c => c.TransferenciasOrigem).HasForeignKey(e => e.ContaOrigemId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.ContaDestino).WithMany(c => c.TransferenciasDestino).HasForeignKey(e => e.ContaDestinoId).OnDelete(DeleteBehavior.Restrict);
    }
}
