using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class ExameMedico : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public string TipoExame { get; set; } = null!;  // Admissional, Periodico, Demissional, Retorno
    public DateTime DataExame { get; set; }
    public DateTime? DataVencimento { get; set; }
    public string Resultado { get; set; } = null!;  // Apto, Inapto, AptoComRestricao
    public string? Medico { get; set; }
    public string? CRM { get; set; }
    public string? DocumentoUrl { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
