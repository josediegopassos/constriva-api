using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.RH;

public class Funcionario : TenantEntity
{
    public string Matricula { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? NomeSocial { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = null!;
    public string? Rg { get; set; }
    public string? OrgaoExpedidor { get; set; }
    public string? Cnh { get; set; }
    public string? CategoriaCnh { get; set; }
    public DateTime? ValidadeCnh { get; set; }
    public string? Ctps { get; set; }
    public string? SeriCtps { get; set; }
    public string? Pis { get; set; }
    public string Email { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? FotoUrl { get; set; }
    public string? Genero { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Escolaridade { get; set; }
    public string? Naturalidade { get; set; }
    public string? Nacionalidade { get; set; }

    // Endereço
    public Guid? EnderecoId { get; set; }
    public virtual Endereco? Endereco { get; set; }

    // Contrato de trabalho
    public TipoContratacaoEnum TipoContratacaoEnum { get; set; }
    public StatusFuncionarioEnum Status { get; set; } = StatusFuncionarioEnum.Ativo;
    public Guid? CargoId { get; set; }
    public Guid? DepartamentoId { get; set; }
    public Guid? ObraAtualId { get; set; }
    public DateTime DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public string? MotivoDemissao { get; set; }
    public decimal SalarioBase { get; set; }
    public decimal? HoraExtra50 { get; set; }
    public decimal? HoraExtra100 { get; set; }
    public int JornadaDiaria { get; set; } = 8;

    // Dados bancários
    public string? BancoNome { get; set; }
    public string? BancoAgencia { get; set; }
    public string? BancoConta { get; set; }
    public string? PixChave { get; set; }

    public virtual Cargo? Cargo { get; set; }
    public virtual Departamento? Departamento { get; set; }
    public virtual ICollection<RegistroPonto> RegistrosPonto { get; set; } = new List<RegistroPonto>();
    public virtual ICollection<Afastamento> Afastamentos { get; set; } = new List<Afastamento>();
    public virtual ICollection<FolhaFuncionario> FolhaPagamentos { get; set; } = new List<FolhaFuncionario>();
    public virtual ICollection<DocumentoFuncionario> Documentos { get; set; } = new List<DocumentoFuncionario>();
    public virtual ICollection<ExameMedico> ExamesMedicos { get; set; } = new List<ExameMedico>();
    public virtual ICollection<TreinamentoFuncionario> Treinamentos { get; set; } = new List<TreinamentoFuncionario>();
    public virtual ICollection<EPIFuncionario> EPIs { get; set; } = new List<EPIFuncionario>();
}
