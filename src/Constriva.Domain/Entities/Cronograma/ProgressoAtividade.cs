using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Cronograma;

public class ProgressoAtividade : TenantEntity
{
    public Guid AtividadeId { get; set; }
    public DateTime DataRegistro { get; set; }
    public decimal PercentualAnterior { get; set; }
    public decimal PercentualAtual { get; set; }
    public string? Observacoes { get; set; }
    public Guid RegistradoPor { get; set; }
    public string? FotoUrl { get; set; }
    public virtual AtividadeCronograma Atividade { get; set; } = null!;
}
