using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class InventarioEstoque : TenantEntity
{
    public Guid AlmoxarifadoId { get; set; }
    public string Numero { get; set; } = null!;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string Status { get; set; } = "EmAndamento";
    public Guid ResponsavelId { get; set; }
    public string? Observacoes { get; set; }
    public decimal ValorDiferenca { get; set; }

    public virtual ICollection<ItemInventario> Itens { get; set; } = new List<ItemInventario>();
}
