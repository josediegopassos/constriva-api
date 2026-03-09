using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ApuracaoPontoConfiguration : IEntityTypeConfiguration<ApuracaoPonto>
{
    public void Configure(EntityTypeBuilder<ApuracaoPonto> b)
    {
        b.ToTable("ApuracoesPonto");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.FuncionarioId, e.DataReferencia }).IsUnique();
        b.Property(e => e.HorasNormais).HasPrecision(5, 2);
        b.Property(e => e.HorasExtras50).HasPrecision(5, 2);
        b.Property(e => e.HorasExtras100).HasPrecision(5, 2);
        b.Property(e => e.HorasNoturnas).HasPrecision(5, 2);
        b.Property(e => e.HorasFeriado).HasPrecision(5, 2);
        b.Property(e => e.HorasFalta).HasPrecision(5, 2);
        b.Property(e => e.HorasAtraso).HasPrecision(5, 2);
        b.HasOne(e => e.Funcionario).WithMany().HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
