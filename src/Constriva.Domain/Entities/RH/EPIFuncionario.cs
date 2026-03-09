using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class EPIFuncionario : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public Guid EPIId { get; set; }
    public DateTime DataEntrega { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public decimal Quantidade { get; set; }
    public string? NumeroCA { get; set; }
    public string? AssinaturaUrl { get; set; }
    public string? Observacoes { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
