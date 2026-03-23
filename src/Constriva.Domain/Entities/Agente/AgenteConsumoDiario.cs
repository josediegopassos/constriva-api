using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteConsumoDiario : TenantEntity
{
    public DateTime Data { get; set; }
    public long TokensInput { get; set; }
    public long TokensOutput { get; set; }
    public int TotalRequisicoes { get; set; }
}
