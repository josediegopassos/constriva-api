using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class ItemPedidoCompra : TenantEntity
{
    public Guid PedidoId { get; set; }
    /// <summary>
    /// Referência ao material do catálogo. Nullable para suportar itens descritos livremente
    /// em pedidos de compra rascunho (sem vínculo a material cadastrado).
    /// Requer migration EF: ItemPedidoCompra.MaterialId nullable.
    /// </summary>
    public Guid? MaterialId { get; set; }
    public string Descricao { get; set; } = null!;
    public string UnidadeMedida { get; set; } = null!;
    public decimal QuantidadePedida { get; set; }
    public decimal QuantidadeRecebida { get; set; } = 0;
    public decimal QuantidadePendente => QuantidadePedida - QuantidadeRecebida;
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal => QuantidadePedida * PrecoUnitario;

    public virtual PedidoCompra Pedido { get; set; } = null!;
    public virtual Estoque.Material? Material { get; set; }
}
