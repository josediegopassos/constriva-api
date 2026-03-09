using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EPIFuncionarioConfiguration : IEntityTypeConfiguration<EPIFuncionario>
{
    public void Configure(EntityTypeBuilder<EPIFuncionario> b)
    {
        b.ToTable("EPIFuncionarios");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Quantidade).HasPrecision(5, 0);
        b.Property(e => e.NumeroCA).HasMaxLength(20);
        b.Property(e => e.AssinaturaUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasOne(e => e.Funcionario).WithMany(f => f.EPIs).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
