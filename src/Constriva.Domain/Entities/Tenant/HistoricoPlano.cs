using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Tenant;

public class HistoricoPlano : BaseEntity
{
    public Guid EmpresaId { get; set; }
    public PlanoEmpresaEnum PlanoAnterior { get; set; }
    public PlanoEmpresaEnum PlanoNovo { get; set; }
    public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
    public string? Motivo { get; set; }
    public Guid AlteradoPor { get; set; }
    public virtual Empresa Empresa { get; set; } = null!;
}
