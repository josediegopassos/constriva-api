using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Orcamento;

public class ItemOrcamento : TenantEntity
{
    public Guid OrcamentoId { get; set; }
    public Guid GrupoId { get; set; }
    public Guid? ComposicaoId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public FontePrecoEnum Fonte { get; set; } = FontePrecoEnum.Manual;
    public string? CodigoFonte { get; set; }
    public string UnidadeMedida { get; set; } = null!;
    public decimal Quantidade { get; set; }
    public decimal CustoUnitario { get; set; }
    public decimal CustoTotal => Quantidade * CustoUnitario;
    public decimal CustoComBDI { get; set; }
    public decimal BDI { get; set; }
    public int Ordem { get; set; }

    public virtual GrupoOrcamento Grupo { get; set; } = null!;
    public virtual ComposicaoOrcamento? Composicao { get; set; }
}
