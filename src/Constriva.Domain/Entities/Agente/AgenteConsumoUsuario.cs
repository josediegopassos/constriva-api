using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteConsumoUsuario : TenantEntity
{
    public Guid UsuarioId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public long TokensUtilizados { get; set; }
    public int TotalRequisicoes { get; set; }
}
