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
    public Guid? EnderecoId { get; set; }
    public virtual Endereco? Endereco { get; set; }

    public virtual ICollection<Obras.Obra> Obras { get; set; } = new List<Obras.Obra>();
}
