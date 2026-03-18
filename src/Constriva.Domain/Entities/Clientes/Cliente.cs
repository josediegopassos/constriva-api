using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Clientes;

public class Cliente : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public TipoPessoaEnum TipoPessoa { get; set; }
    public string Nome { get; set; } = null!;
    public string? NomeFantasia { get; set; }
    public string? Documento { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? InscricaoMunicipal { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string? Site { get; set; }
    public StatusClienteEnum Status { get; set; } = StatusClienteEnum.Ativo;
    public string? Observacoes { get; set; }

    // Endereço
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }

    public virtual ICollection<Obras.Obra> Obras { get; set; } = new List<Obras.Obra>();
}
