using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class ChecklistModelo : TenantEntity
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public string? EtapaConstrucao { get; set; }
    public bool Ativo { get; set; } = true;
    public virtual ICollection<ChecklistModeloItem> Itens { get; set; } = new List<ChecklistModeloItem>();
    public virtual ICollection<Inspecao> Inspecoes { get; set; } = new List<Inspecao>();
}
