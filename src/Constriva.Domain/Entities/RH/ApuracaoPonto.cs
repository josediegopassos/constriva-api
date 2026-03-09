using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class ApuracaoPonto : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public DateTime DataReferencia { get; set; }
    public decimal HorasNormais { get; set; }
    public decimal HorasExtras50 { get; set; }
    public decimal HorasExtras100 { get; set; }
    public decimal HorasNoturnas { get; set; }
    public decimal HorasFeriado { get; set; }
    public decimal HorasFalta { get; set; }
    public decimal HorasAtraso { get; set; }
    public bool Fechado { get; set; } = false;
    public Guid? FechadoPor { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
