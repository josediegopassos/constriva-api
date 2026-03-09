using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Financeiro;

public class CentroCusto : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoCentroCustoEnum Tipo { get; set; }
    public Guid? ObraId { get; set; }
    public Guid? CentroPaiId { get; set; }
    public bool Ativo { get; set; } = true;
    public virtual CentroCusto? CentroPai { get; set; }
    public virtual ICollection<CentroCusto> SubCentros { get; set; } = new List<CentroCusto>();
    public virtual ICollection<LancamentoFinanceiro> Lancamentos { get; set; } = new List<LancamentoFinanceiro>();
}
