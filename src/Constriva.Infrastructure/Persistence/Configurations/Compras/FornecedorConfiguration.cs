using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> b)
    {
        b.ToTable("Fornecedores");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.RazaoSocial).HasMaxLength(200).IsRequired();
        b.Property(e => e.NomeFantasia).HasMaxLength(200);
        b.Property(e => e.Documento).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Documento }).IsUnique();
        b.Property(e => e.InscricaoEstadual).HasMaxLength(30);
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Celular).HasMaxLength(20);
        b.Property(e => e.Site).HasMaxLength(300);
        b.Property(e => e.Contato).HasMaxLength(200);
        b.Property(e => e.TipoPessoaEnum).HasConversion<int>();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Classificacao).HasMaxLength(50);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.BancoNome).HasMaxLength(100);
        b.Property(e => e.BancoAgencia).HasMaxLength(20);
        b.Property(e => e.BancoConta).HasMaxLength(30);
        b.Property(e => e.PixChave).HasMaxLength(150);
        b.Property(e => e.Logradouro).HasMaxLength(300);
        b.Property(e => e.Numero).HasMaxLength(20);
        b.Property(e => e.Complemento).HasMaxLength(100);
        b.Property(e => e.Bairro).HasMaxLength(100);
        b.Property(e => e.Cidade).HasMaxLength(100);
        b.Property(e => e.Estado).HasMaxLength(2);
        b.Property(e => e.Cep).HasMaxLength(10);
    }
}
