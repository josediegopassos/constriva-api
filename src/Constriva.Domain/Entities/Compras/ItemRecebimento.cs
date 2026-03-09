using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class ItemRecebimento : TenantEntity
{
    public Guid RecebimentoId { get; set; }
    public Guid ItemPedidoId { get; set; }
    public decimal QuantidadeRecebida { get; set; }
    public decimal PrecoUnitario { get; set; }
    public string? Lote { get; set; }
    public DateTime? Validade { get; set; }
    public bool Aprovado { get; set; } = true;
    public string? MotivoReprovacao { get; set; }

    public virtual RecebimentoCompra Recebimento { get; set; } = null!;
    public virtual ItemPedidoCompra ItemPedido { get; set; } = null!;
}
