using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class TreinamentoFuncionarioConfiguration : IEntityTypeConfiguration<TreinamentoFuncionario>
{
    public void Configure(EntityTypeBuilder<TreinamentoFuncionario> b)
    {
        b.ToTable("TreinamentosFuncionario");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.NomeTreinamento).HasMaxLength(200).IsRequired();
        b.Property(e => e.NormaRelacionada).HasMaxLength(50);
        b.Property(e => e.CargaHoraria).HasPrecision(5, 1);
        b.Property(e => e.Instrutor).HasMaxLength(200);
        b.Property(e => e.Local).HasMaxLength(200);
        b.Property(e => e.CertificadoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Funcionario).WithMany(f => f.Treinamentos).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
