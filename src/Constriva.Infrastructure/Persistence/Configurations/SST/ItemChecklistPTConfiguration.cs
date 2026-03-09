using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemChecklistPTConfiguration : IEntityTypeConfiguration<ItemCheclistPT>
{
    public void Configure(EntityTypeBuilder<ItemCheclistPT> b)
    {
        b.ToTable("ItensChecklistPT");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Item).HasMaxLength(500).IsRequired();
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.HasOne(e => e.Permissao).WithMany(p => p.Checklist).HasForeignKey(e => e.PermissaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
