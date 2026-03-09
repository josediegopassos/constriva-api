using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class FVS : TenantEntity  // Ficha de Verificação de Serviço
{
    public Guid ObraId { get; set; }
    public Guid? FaseObraId { get; set; }
    public string Numero { get; set; } = null!;
    public string Servico { get; set; } = null!;
    public string? EtapaConstrucao { get; set; }
    public string? Pavimento { get; set; }
    public string? Area { get; set; }
    public DateTime DataVerificacao { get; set; }
    public bool Aprovado { get; set; } = false;
    public Guid? ResponsavelId { get; set; }
    public Guid? FiscalId { get; set; }
    public string? Observacoes { get; set; }
    public string? AssinaturaUrl { get; set; }

    public virtual ICollection<ItemFVS> Itens { get; set; } = new List<ItemFVS>();
}
