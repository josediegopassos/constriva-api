using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class DocumentoFuncionario : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public string Tipo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Url { get; set; } = null!;
    public DateTime? DataVencimento { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
