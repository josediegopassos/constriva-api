using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class ItemRequisicao : TenantEntity
{
    public Guid RequisicaoId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal QuantidadeSolicitada { get; set; }
    public decimal QuantidadeAtendida { get; set; } = 0;
    public string? Observacao { get; set; }
    public decimal? PrecoReferencia { get; set; }

    public virtual RequisicaoMaterial Requisicao { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}
