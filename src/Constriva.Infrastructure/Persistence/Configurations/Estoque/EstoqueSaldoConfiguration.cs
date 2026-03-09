using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EstoqueSaldoConfiguration : IEntityTypeConfiguration<EstoqueSaldo>
{
    public void Configure(EntityTypeBuilder<EstoqueSaldo> b)
    {
        b.ToTable("EstoqueSaldos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.AlmoxarifadoId, e.MaterialId }).IsUnique();
        b.Property(e => e.SaldoAtual).HasPrecision(18, 4);
        b.Property(e => e.SaldoReservado).HasPrecision(18, 4);
        b.Property(e => e.CustoMedio).HasPrecision(18, 4);
        b.Ignore(e => e.SaldoDisponivel);
        b.HasOne(e => e.Almoxarifado).WithMany(a => a.Saldos).HasForeignKey(e => e.AlmoxarifadoId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Material).WithMany(m => m.Saldos).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
