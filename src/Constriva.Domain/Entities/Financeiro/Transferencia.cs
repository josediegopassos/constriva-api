using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Financeiro;

public class Transferencia : TenantEntity
{
    public Guid ContaOrigemId { get; set; }
    public Guid ContaDestinoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataTransferencia { get; set; }
    public string? Descricao { get; set; }
    public string? Comprovante { get; set; }
    public Guid RealizadoPor { get; set; }

    public virtual ContaBancaria ContaOrigem { get; set; } = null!;
    public virtual ContaBancaria ContaDestino { get; set; } = null!;
}
