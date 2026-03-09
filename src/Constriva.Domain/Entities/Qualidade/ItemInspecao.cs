using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class ItemInspecao : TenantEntity
{
    public Guid InspecaoId { get; set; }
    public Guid? ChecklistItemId { get; set; }
    public string Descricao { get; set; } = null!;
    public string? RespostaSimNao { get; set; }  // Sim, Nao, NA
    public string? RespostaTexto { get; set; }
    public decimal? RespostaNumero { get; set; }
    public bool Conforme { get; set; } = true;
    public string? Observacao { get; set; }
    public string? FotoUrl { get; set; }
    public virtual Inspecao Inspecao { get; set; } = null!;
}
