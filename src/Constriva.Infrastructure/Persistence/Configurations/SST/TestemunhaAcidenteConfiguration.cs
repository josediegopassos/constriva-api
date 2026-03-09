using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class TestemunhaAcidenteConfiguration : IEntityTypeConfiguration<TestemunhaAcidente>
{
    public void Configure(EntityTypeBuilder<TestemunhaAcidente> b)
    {
        b.ToTable("TestemunhasAcidente");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Funcao).HasMaxLength(100);
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Depoimento).HasMaxLength(2000);
        b.HasOne(e => e.Acidente).WithMany(a => a.Testemunhas).HasForeignKey(e => e.AcidenteId).OnDelete(DeleteBehavior.Cascade);
    }
}
