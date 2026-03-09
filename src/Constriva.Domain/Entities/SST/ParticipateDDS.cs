using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class ParticipateDDS : TenantEntity
{
    public Guid DDSId { get; set; }
    public Guid? FuncionarioId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Funcao { get; set; }
    public string? Empresa { get; set; }
    public string? AssinaturaUrl { get; set; }
    public virtual DDS DDS { get; set; } = null!;
}
