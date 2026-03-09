using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Compras;

public class Fornecedor : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public TipoPessoaEnum TipoPessoaEnum { get; set; }
    public string RazaoSocial { get; set; } = null!;
    public string? NomeFantasia { get; set; }
    public string Documento { get; set; } = null!;  // CPF ou CNPJ
    public string? InscricaoEstadual { get; set; }
    public string Email { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Site { get; set; }
    public string? Contato { get; set; }
    public TipoFornecedorEnum Tipo { get; set; }
    public bool Ativo { get; set; } = true;
    public bool Homologado { get; set; } = false;
    public string? Classificacao { get; set; }
    public int? Prazo { get; set; }
    public string? Observacoes { get; set; }
    public string? BancoNome { get; set; }
    public string? BancoAgencia { get; set; }
    public string? BancoConta { get; set; }
    public string? PixChave { get; set; }

    // Endereço
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }

    public virtual ICollection<PedidoCompra> PedidosCompra { get; set; } = new List<PedidoCompra>();
    public virtual ICollection<PropostaCotacao> Propostas { get; set; } = new List<PropostaCotacao>();
    public virtual ICollection<Contratos.Contrato> Contratos { get; set; } = new List<Contratos.Contrato>();
}
