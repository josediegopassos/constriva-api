using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class RecebimentoCompra : TenantEntity
{
    public Guid PedidoId { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
    public string? NumeroNF { get; set; }
    public string? SerieNF { get; set; }
    public DateTime? DataNF { get; set; }
    public decimal ValorNF { get; set; }
    public string? Observacoes { get; set; }
    public Guid AlmoxarifadoId { get; set; }
    public Guid RecebidoPor { get; set; }

    public virtual PedidoCompra Pedido { get; set; } = null!;
    public virtual ICollection<ItemRecebimento> Itens { get; set; } = new List<ItemRecebimento>();
}
