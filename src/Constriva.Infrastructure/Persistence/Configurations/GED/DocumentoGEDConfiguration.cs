using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DocumentoGEDConfiguration : IEntityTypeConfiguration<DocumentoGED>
{
    public void Configure(EntityTypeBuilder<DocumentoGED> b)
    {
        b.ToTable("DocumentosGED");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(50).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.Titulo).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Versao).HasMaxLength(20);
        b.Property(e => e.NormaReferencia).HasMaxLength(100);
        b.Property(e => e.Palavraschave).HasMaxLength(500);
        b.HasMany(e => e.Arquivos).WithOne(a => a.Documento).HasForeignKey(a => a.DocumentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Revisoes).WithOne(r => r.Documento).HasForeignKey(r => r.DocumentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.FluxoAprovacao).WithOne(f => f.Documento).HasForeignKey(f => f.DocumentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Distribuicoes).WithOne(d => d.Documento).HasForeignKey(d => d.DocumentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Acessos).WithOne(a => a.Documento).HasForeignKey(a => a.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
