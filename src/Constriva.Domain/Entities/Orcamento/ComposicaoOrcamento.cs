using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Orcamento;

public class ComposicaoOrcamento : TenantEntity
{
    public Guid OrcamentoId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string UnidadeMedida { get; set; } = null!;
    public FontePrecoEnum Fonte { get; set; } = FontePrecoEnum.Manual;
    public string? CodigoFonte { get; set; }
    public decimal CustoTotal { get; set; } = 0;
    public string? Observacoes { get; set; }

    public virtual ICollection<InsumoComposicao> Insumos { get; set; } = new List<InsumoComposicao>();
}
