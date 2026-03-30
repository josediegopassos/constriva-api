using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DadosBancariosConfiguration : IEntityTypeConfiguration<DadosBancarios>
{
    public void Configure(EntityTypeBuilder<DadosBancarios> b)
    {
        b.ToTable("DadosBancarios");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasOne(e => e.Banco).WithMany().HasForeignKey(e => e.BancoId).OnDelete(DeleteBehavior.SetNull);
        b.Property(e => e.Agencia).HasMaxLength(20);
        b.Property(e => e.Conta).HasMaxLength(30);
        b.Property(e => e.PixChave).HasMaxLength(150);
    }
}
