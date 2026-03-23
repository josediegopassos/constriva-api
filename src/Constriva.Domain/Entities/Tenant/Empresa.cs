using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Tenant;

public class Empresa : BaseEntity
{
    public string RazaoSocial { get; set; } = null!;
    public string NomeFantasia { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? InscricaoEstadual { get; set; }
    public string? InscricaoMunicipal { get; set; }
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public string? Site { get; set; }
    public string? LogoUrl { get; set; }
    public StatusEmpresaEnum Status { get; set; } = StatusEmpresaEnum.Trial;
    public PlanoEmpresaEnum Plano { get; set; } = PlanoEmpresaEnum.Basico;
    public DateTime? DataVencimento { get; set; }
    public int MaxUsuarios { get; set; } = 5;
    public int MaxObras { get; set; } = 3;
    public int MaxStorageMb { get; set; } = 1024;
    public bool PrimeiroAcesso { get; set; } = true;

    // Endereço
    public string Logradouro { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string Cep { get; set; } = null!;

    // Módulos habilitados
    public bool ModuloObras { get; set; } = true;
    public bool ModuloEstoque { get; set; } = true;
    public bool ModuloCronograma { get; set; } = true;
    public bool ModuloOrcamento { get; set; } = true;
    public bool ModuloCompras { get; set; } = true;
    public bool ModuloQualidade { get; set; } = true;
    public bool ModuloContratos { get; set; } = true;
    public bool ModuloRH { get; set; } = true;
    public bool ModuloFinanceiro { get; set; } = true;
    public bool ModuloSST { get; set; } = true;
    public bool ModuloGED { get; set; } = true;
    public bool ModuloClientes { get; set; } = true;
    public bool ModuloFornecedores { get; set; } = true;
    public bool ModuloAgente { get; set; } = false;
    public bool ModuloRelatorios { get; set; } = true;

    public string? ConfiguracoesJson { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<HistoricoPlano> HistoricoPlanos { get; set; } = new List<HistoricoPlano>();
}
