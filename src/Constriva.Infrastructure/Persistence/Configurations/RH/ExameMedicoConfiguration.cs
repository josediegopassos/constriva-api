using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ExameMedicoConfiguration : IEntityTypeConfiguration<ExameMedico>
{
    public void Configure(EntityTypeBuilder<ExameMedico> b)
    {
        b.ToTable("ExamesMedicos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.TipoExame).HasMaxLength(30).IsRequired();
        b.Property(e => e.Resultado).HasMaxLength(30).IsRequired();
        b.Property(e => e.Medico).HasMaxLength(200);
        b.Property(e => e.CRM).HasMaxLength(20);
        b.Property(e => e.DocumentoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Funcionario).WithMany(f => f.ExamesMedicos).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
