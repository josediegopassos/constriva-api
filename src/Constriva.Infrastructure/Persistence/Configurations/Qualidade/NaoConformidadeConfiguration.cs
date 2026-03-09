using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class NaoConformidadeConfiguration : IEntityTypeConfiguration<NaoConformidade>
{
    public void Configure(EntityTypeBuilder<NaoConformidade> b)
    {
        b.ToTable("NaoConformidades");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Titulo).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Gravidade).HasConversion<int>();
        b.Property(e => e.LocalizacaoObra).HasMaxLength(200);
        b.Property(e => e.CausaRaiz).HasMaxLength(2000);
        b.Property(e => e.AcaoCorretiva).HasMaxLength(2000);
        b.Property(e => e.AcaoPreventiva).HasMaxLength(2000);
        b.Property(e => e.FotoAntesUrl).HasMaxLength(1000);
        b.Property(e => e.FotoDepoisUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.CustoNC).HasPrecision(18, 2);
        b.HasMany(e => e.Acoes).WithOne(a => a.NaoConformidade).HasForeignKey(a => a.NaoConformidadeId).OnDelete(DeleteBehavior.Cascade);
    }
}
