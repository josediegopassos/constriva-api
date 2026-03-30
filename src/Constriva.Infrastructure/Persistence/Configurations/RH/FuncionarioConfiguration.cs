using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> b)
    {
        b.ToTable("Funcionarios");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Matricula).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Matricula }).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.NomeSocial).HasMaxLength(200);
        b.Property(e => e.Cpf).HasMaxLength(11).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Cpf }).IsUnique();
        b.Property(e => e.Rg).HasMaxLength(20);
        b.Property(e => e.OrgaoExpedidor).HasMaxLength(20);
        b.Property(e => e.Cnh).HasMaxLength(20);
        b.Property(e => e.CategoriaCnh).HasMaxLength(5);
        b.Property(e => e.Ctps).HasMaxLength(20);
        b.Property(e => e.SeriCtps).HasMaxLength(10);
        b.Property(e => e.Pis).HasMaxLength(20);
        b.Property(e => e.Email).HasMaxLength(200).IsRequired();
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Celular).HasMaxLength(20);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.Property(e => e.Genero).HasMaxLength(20);
        b.Property(e => e.EstadoCivil).HasMaxLength(20);
        b.Property(e => e.Escolaridade).HasMaxLength(50);
        b.Property(e => e.Naturalidade).HasMaxLength(100);
        b.Property(e => e.Nacionalidade).HasMaxLength(100);
        b.HasOne(e => e.Endereco).WithMany().HasForeignKey(e => e.EnderecoId).OnDelete(DeleteBehavior.SetNull);
        b.Property(e => e.TipoContratacaoEnum).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.MotivoDemissao).HasMaxLength(500);
        b.Property(e => e.SalarioBase).HasPrecision(18, 2);
        b.Property(e => e.HoraExtra50).HasPrecision(18, 2);
        b.Property(e => e.HoraExtra100).HasPrecision(18, 2);
        b.HasOne(e => e.DadosBancarios).WithMany().HasForeignKey(e => e.DadosBancariosId).OnDelete(DeleteBehavior.SetNull);
        b.HasOne(e => e.Cargo).WithMany(c => c.Funcionarios).HasForeignKey(e => e.CargoId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Departamento).WithMany(d => d.Funcionarios).HasForeignKey(e => e.DepartamentoId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.ObraAtual).WithMany().HasForeignKey(e => e.ObraAtualId).OnDelete(DeleteBehavior.SetNull);
    }
}
