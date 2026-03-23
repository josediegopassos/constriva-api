using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Common;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> b)
    {
        b.ToTable("Notificacoes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.ModuloOrigem).HasMaxLength(50).IsRequired();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Mensagem).HasMaxLength(2000).IsRequired();
    }
}
