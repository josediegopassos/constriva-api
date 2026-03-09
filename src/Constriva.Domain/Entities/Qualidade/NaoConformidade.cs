using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Qualidade;

public class NaoConformidade : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? InspecaoId { get; set; }
    public Guid? FaseObraId { get; set; }
    public string Numero { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public StatusNaoConformidadeEnum Status { get; set; } = StatusNaoConformidadeEnum.Aberta;
    public GravidadeNCEnum Gravidade { get; set; }
    public string? LocalizacaoObra { get; set; }
    public string? CausaRaiz { get; set; }
    public string? AcaoCorretiva { get; set; }
    public string? AcaoPreventiva { get; set; }
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataPrazoConclusao { get; set; }
    public DateTime? DataEncerramento { get; set; }
    public Guid? ResponsavelId { get; set; }
    public Guid? VerificadoPor { get; set; }
    public string? FotoAntesUrl { get; set; }
    public string? FotoDepoisUrl { get; set; }
    public string? Observacoes { get; set; }
    public decimal? CustoNC { get; set; }

    public virtual ICollection<AcaoNC> Acoes { get; set; } = new List<AcaoNC>();
}
