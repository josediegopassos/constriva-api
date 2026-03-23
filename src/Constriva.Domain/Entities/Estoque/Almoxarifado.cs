using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class Almoxarifado : TenantEntity
{
    public Guid? ObraId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public Guid? EnderecoId { get; set; }
    public virtual Endereco? Endereco { get; set; }
    public Guid? ResponsavelId { get; set; }
    public bool Ativo { get; set; } = true;
    public bool Principal { get; set; } = false;

    public virtual ICollection<EstoqueSaldo> Saldos { get; set; } = new List<EstoqueSaldo>();
    public virtual ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
}
