using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class TestemunhaAcidente : TenantEntity
{
    public Guid AcidenteId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Funcao { get; set; }
    public string? Telefone { get; set; }
    public string? Depoimento { get; set; }
    public virtual RegistroAcidente Acidente { get; set; } = null!;
}
