using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AfastamentoConfiguration : IEntityTypeConfiguration<Afastamento>
{
    public void Configure(EntityTypeBuilder<Afastamento> b)
    {
        b.ToTable("Afastamentos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.CID).HasMaxLength(10);
        b.Property(e => e.NumeroCAT).HasMaxLength(30);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.DocumentoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Funcionario).WithMany(f => f.Afastamentos).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
