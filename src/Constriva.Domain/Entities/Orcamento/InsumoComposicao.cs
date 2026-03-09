using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Orcamento;

public class InsumoComposicao : TenantEntity
{
    public Guid ComposicaoId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public TipoInsumoEnum Tipo { get; set; }
    public string UnidadeMedida { get; set; } = null!;
    public decimal Coeficiente { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal CustoTotal => Coeficiente * PrecoUnitario;
    public FontePrecoEnum FontePrecoEnum { get; set; } = FontePrecoEnum.Manual;
    public Guid? MaterialId { get; set; }

    public virtual ComposicaoOrcamento Composicao { get; set; } = null!;
}
