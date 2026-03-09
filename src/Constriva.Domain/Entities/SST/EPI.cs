using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.SST;

public class EPI : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoEPIEnum Tipo { get; set; }
    public string? Fabricante { get; set; }
    public string? Modelo { get; set; }
    public string? NumeroCA { get; set; }
    public DateTime? ValidadeCA { get; set; }
    public string? NormaReferencia { get; set; }
    public int EstoqueAtual { get; set; }
    public int EstoqueMinimo { get; set; }
    public decimal VidaUtilMeses { get; set; }
    public bool Ativo { get; set; } = true;
    public string? ImagemUrl { get; set; }
}
