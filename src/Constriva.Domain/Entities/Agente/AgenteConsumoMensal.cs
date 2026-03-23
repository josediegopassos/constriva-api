using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteConsumoMensal : TenantEntity
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public long TokensUtilizados { get; set; }
    public long TokensLimite { get; set; }
    public long TokensAvulsosUtilizados { get; set; }
    public bool Alerta80Enviado { get; set; }
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
