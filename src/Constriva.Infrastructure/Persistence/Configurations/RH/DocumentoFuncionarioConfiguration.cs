using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DocumentoFuncionarioConfiguration : IEntityTypeConfiguration<DocumentoFuncionario>
{
    public void Configure(EntityTypeBuilder<DocumentoFuncionario> b)
    {
        b.ToTable("DocumentosFuncionario");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.HasOne(e => e.Funcionario).WithMany(f => f.Documentos).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
