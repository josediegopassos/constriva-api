using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Contratos;

public class AditvoContrato : TenantEntity
{
    public Guid ContratoId { get; set; }
    public string Numero { get; set; } = null!;
    public string Tipo { get; set; } = null!;  // Prazo, Valor, PrazoValor
    public string Justificativa { get; set; } = null!;
    public DateTime DataAssinatura { get; set; }
    public decimal ValorAditivo { get; set; }
    public int? ProrrogacaoDias { get; set; }
    public DateTime? NovaDataVigencia { get; set; }
    public string? ArquivoUrl { get; set; }
    public virtual Contrato Contrato { get; set; } = null!;
}
