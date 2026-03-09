using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Orcamento;

public class OrcamentoHistorico : TenantEntity
{
    public Guid OrcamentoId { get; set; }
    public string Descricao { get; set; } = null!;
    public decimal ValorAnterior { get; set; }
    public decimal ValorNovo { get; set; }
    public Guid UsuarioId { get; set; }
    public virtual Orcamento Orcamento { get; set; } = null!;
}
