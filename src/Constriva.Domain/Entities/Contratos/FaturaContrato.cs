using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Contratos;

public class FaturaContrato : TenantEntity
{
    public Guid ContratoId { get; set; }
    public Guid? MedicaoId { get; set; }
    public string Numero { get; set; } = null!;
    public string? NumeroNF { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public decimal ValorFatura { get; set; }
    public decimal ValorPago { get; set; }
    public StatusLancamentoEnum Status { get; set; } = StatusLancamentoEnum.Previsto;
    public string? Observacoes { get; set; }

    public virtual Contrato Contrato { get; set; } = null!;
}
