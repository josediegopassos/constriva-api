using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RegistroAcidenteConfiguration : IEntityTypeConfiguration<RegistroAcidente>
{
    public void Configure(EntityTypeBuilder<RegistroAcidente> b)
    {
        b.ToTable("RegistrosAcidente");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.NomeFuncionario).HasMaxLength(200).IsRequired();
        b.Property(e => e.EmpresaFuncionario).HasMaxLength(200);
        b.Property(e => e.CargoFuncionario).HasMaxLength(100);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Local).HasMaxLength(300).IsRequired();
        b.Property(e => e.DescricaoAcidente).HasMaxLength(4000).IsRequired();
        b.Property(e => e.PartesCorpoAfetadas).HasMaxLength(500);
        b.Property(e => e.NaturezaLesao).HasMaxLength(200);
        b.Property(e => e.CausaImediata).HasMaxLength(1000);
        b.Property(e => e.CausaBasica).HasMaxLength(1000);
        b.Property(e => e.MedidasCorretivas).HasMaxLength(2000);
        b.Property(e => e.NumeroCAT).HasMaxLength(30);
        b.Property(e => e.TratamentoMedico).HasMaxLength(500);
        b.Property(e => e.NumeroBO).HasMaxLength(30);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Testemunhas).WithOne(t => t.Acidente).HasForeignKey(t => t.AcidenteId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.AcoesCorretivas).WithOne(a => a.Acidente).HasForeignKey(a => a.AcidenteId).OnDelete(DeleteBehavior.Cascade);
    }
}
