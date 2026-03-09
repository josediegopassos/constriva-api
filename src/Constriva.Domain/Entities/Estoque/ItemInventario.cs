using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class ItemInventario : TenantEntity
{
    public Guid InventarioId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal SaldoSistema { get; set; }
    public decimal SaldoContado { get; set; }
    public decimal Diferenca => SaldoContado - SaldoSistema;
    public decimal CustoUnitario { get; set; }
    public decimal ValorDiferenca => Diferenca * CustoUnitario;
    public string? Observacao { get; set; }
    public bool Ajustado { get; set; } = false;

    public virtual InventarioEstoque Inventario { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}
