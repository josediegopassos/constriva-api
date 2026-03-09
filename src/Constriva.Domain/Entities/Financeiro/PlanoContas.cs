using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Financeiro;

public class PlanoContas : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoLancamentoEnum Tipo { get; set; }
    public Guid? ContaPaiId { get; set; }
    public bool Sintetica { get; set; } = false;
    public bool Ativo { get; set; } = true;
    public virtual PlanoContas? ContaPai { get; set; }
    public virtual ICollection<PlanoContas> SubContas { get; set; } = new List<PlanoContas>();
}
