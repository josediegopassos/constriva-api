using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class EstoqueSaldo : TenantEntity
{
    public Guid AlmoxarifadoId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal SaldoAtual { get; set; } = 0;
    public decimal SaldoReservado { get; set; } = 0;
    public decimal SaldoDisponivel => SaldoAtual - SaldoReservado;
    public decimal CustoMedio { get; set; } = 0;
    public DateTime UltimaMovimentacao { get; set; } = DateTime.UtcNow;

    public virtual Almoxarifado Almoxarifado { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}
