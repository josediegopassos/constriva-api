using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Orcamento;

public class GrupoOrcamento : TenantEntity
{
    public Guid OrcamentoId { get; set; }
    public Guid? GrupoPaiId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public int Ordem { get; set; }
    public decimal ValorTotal { get; set; } = 0;
    public decimal PercentualTotal { get; set; } = 0;

    public virtual Orcamento Orcamento { get; set; } = null!;
    public virtual GrupoOrcamento? GrupoPai { get; set; }
    public virtual ICollection<GrupoOrcamento> SubGrupos { get; set; } = new List<GrupoOrcamento>();
    public virtual ICollection<ItemOrcamento> Itens { get; set; } = new List<ItemOrcamento>();
}
