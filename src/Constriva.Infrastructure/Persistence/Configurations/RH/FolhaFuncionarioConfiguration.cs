using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FolhaFuncionarioConfiguration : IEntityTypeConfiguration<FolhaFuncionario>
{
    public void Configure(EntityTypeBuilder<FolhaFuncionario> b)
    {
        b.ToTable("FolhaFuncionarios");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.SalarioBruto).HasPrecision(18, 2);
        b.Property(e => e.HorasExtras).HasPrecision(5, 2);
        b.Property(e => e.ValorHorasExtras).HasPrecision(18, 2);
        b.Property(e => e.AdicionalNoturno).HasPrecision(18, 2);
        b.Property(e => e.Periculosidade).HasPrecision(18, 2);
        b.Property(e => e.Insalubridade).HasPrecision(18, 2);
        b.Property(e => e.OutrasVerbas).HasPrecision(18, 2);
        b.Property(e => e.TotalProventos).HasPrecision(18, 2);
        b.Property(e => e.INSS).HasPrecision(18, 2);
        b.Property(e => e.IRRF).HasPrecision(18, 2);
        b.Property(e => e.ValeTransporte).HasPrecision(18, 2);
        b.Property(e => e.ValeRefeicao).HasPrecision(18, 2);
        b.Property(e => e.OutrosDescontos).HasPrecision(18, 2);
        b.Property(e => e.TotalDescontos).HasPrecision(18, 2);
        b.Property(e => e.SalarioLiquido).HasPrecision(18, 2);
        b.Property(e => e.FGTS).HasPrecision(18, 2);
        b.HasOne(e => e.Folha).WithMany(f => f.Funcionarios).HasForeignKey(e => e.FolhaId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Funcionario).WithMany(f => f.FolhaPagamentos).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Restrict);
    }
}
