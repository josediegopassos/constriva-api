using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.SST;

public class PermissaoTrabalho : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Numero { get; set; } = null!;
    public string TipoTrabalho { get; set; } = null!;  // AlturaEspacoConfinado, Eletrico, QuenteEtc
    public StatusPermissaoTrabalhoEnum Status { get; set; } = StatusPermissaoTrabalhoEnum.Aberta;
    public string? Local { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string? DescricaoServico { get; set; }
    public string? RiscosIdentificados { get; set; }
    public string? MedidasControle { get; set; }
    public string? EPIsNecessarios { get; set; }
    public string? EPCsNecessarios { get; set; }
    public string? ProcedimentosEmergencia { get; set; }
    public Guid? ExecutanteId { get; set; }
    public string? NomeExecutante { get; set; }
    public Guid? EmissorId { get; set; }
    public Guid? AprovadorId { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public Guid? SupervisorId { get; set; }
    public string? ObservacoesFechamento { get; set; }
    public DateTime? DataEncerramento { get; set; }

    public virtual ICollection<ItemCheclistPT> Checklist { get; set; } = new List<ItemCheclistPT>();
}
