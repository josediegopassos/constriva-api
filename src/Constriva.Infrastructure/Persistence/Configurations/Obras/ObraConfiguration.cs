using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ObraConfiguration : IEntityTypeConfiguration<Obra>
{
    public void Configure(EntityTypeBuilder<Obra> b)
    {
        b.ToTable("Obras");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.TipoContrato).HasConversion<int>();
        b.Property(e => e.NomeCliente).HasMaxLength(300);
        b.Property(e => e.ResponsavelTecnico).HasMaxLength(200);
        b.Property(e => e.CreaResponsavel).HasMaxLength(50);
        b.Property(e => e.NumeroART).HasMaxLength(50);
        b.Property(e => e.NumeroRRT).HasMaxLength(50);
        b.Property(e => e.NumeroAlvara).HasMaxLength(50);
        b.Property(e => e.ValorContrato).HasPrecision(18, 2);
        b.Property(e => e.ValorOrcado).HasPrecision(18, 2);
        b.Property(e => e.ValorRealizado).HasPrecision(18, 2);
        b.Property(e => e.PercentualConcluido).HasPrecision(5, 2);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.Logradouro).HasMaxLength(300);
        b.Property(e => e.Numero).HasMaxLength(20);
        b.Property(e => e.Complemento).HasMaxLength(100);
        b.Property(e => e.Bairro).HasMaxLength(100);
        b.Property(e => e.Cidade).HasMaxLength(100);
        b.Property(e => e.Estado).HasMaxLength(2);
        b.Property(e => e.Cep).HasMaxLength(10);
        b.HasMany(e => e.Fases).WithOne(f => f.Obra).HasForeignKey(f => f.ObraId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Anexos).WithOne(a => a.Obra).HasForeignKey(a => a.ObraId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Historicos).WithOne(h => h.Obra).HasForeignKey(h => h.ObraId).OnDelete(DeleteBehavior.Cascade);
    }
}
