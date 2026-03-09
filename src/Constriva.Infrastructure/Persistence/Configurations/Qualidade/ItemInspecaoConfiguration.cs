using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemInspecaoConfiguration : IEntityTypeConfiguration<ItemInspecao>
{
    public void Configure(EntityTypeBuilder<ItemInspecao> b)
    {
        b.ToTable("ItensInspecao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.RespostaSimNao).HasMaxLength(5);
        b.Property(e => e.RespostaTexto).HasMaxLength(1000);
        b.Property(e => e.RespostaNumero).HasPrecision(18, 4);
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Inspecao).WithMany(i => i.Itens).HasForeignKey(e => e.InspecaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
