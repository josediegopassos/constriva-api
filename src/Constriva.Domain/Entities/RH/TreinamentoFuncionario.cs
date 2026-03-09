using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class TreinamentoFuncionario : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public string NomeTreinamento { get; set; } = null!;
    public string? NormaRelacionada { get; set; }
    public DateTime DataRealizacao { get; set; }
    public DateTime? DataVencimento { get; set; }
    public decimal CargaHoraria { get; set; }
    public string? Instrutor { get; set; }
    public string? Local { get; set; }
    public string? CertificadoUrl { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
