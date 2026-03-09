using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.RH;

public class Afastamento : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public TipoAfastamentoEnum Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? DataRetornoPrevisto { get; set; }
    public string? CID { get; set; }
    public string? NumeroCAT { get; set; }
    public string? Observacoes { get; set; }
    public string? DocumentoUrl { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
