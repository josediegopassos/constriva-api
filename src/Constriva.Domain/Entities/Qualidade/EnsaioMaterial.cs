using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Qualidade;

public class EnsaioMaterial : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? MaterialId { get; set; }
    public string Numero { get; set; } = null!;
    public TipoEnsaioEnum Tipo { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataColeta { get; set; }
    public DateTime? DataEnsaio { get; set; }
    public string? Laboratorio { get; set; }
    public string? NormaReferencia { get; set; }
    public string? Resultado { get; set; }
    public bool Aprovado { get; set; } = true;
    public decimal? ValorObtido { get; set; }
    public decimal? ValorMinimo { get; set; }
    public decimal? ValorMaximo { get; set; }
    public string? Laudo { get; set; }
    public string? LaudoUrl { get; set; }
    public string? LocalColeta { get; set; }
    public string? Observacoes { get; set; }
}
