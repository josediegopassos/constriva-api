using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> b)
    {
        b.ToTable("ContasBancarias");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.BancoNome).HasMaxLength(100).IsRequired();
        b.Property(e => e.BancoCodigo).HasMaxLength(10);
        b.Property(e => e.Agencia).HasMaxLength(20).IsRequired();
        b.Property(e => e.Conta).HasMaxLength(30).IsRequired();
        b.Property(e => e.TipoConta).HasMaxLength(30).IsRequired();
        b.Property(e => e.SaldoInicial).HasPrecision(18, 2);
        b.Property(e => e.SaldoAtual).HasPrecision(18, 2);
        b.Property(e => e.PixChave).HasMaxLength(150);
    }
}
