using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.SST;

public class RegistroAcidente : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? FuncionarioId { get; set; }
    public string NomeFuncionario { get; set; } = null!;
    public string? EmpresaFuncionario { get; set; }
    public string? CargoFuncionario { get; set; }
    public TipoAcidenteEnum Tipo { get; set; }
    public DateTime DataHoraAcidente { get; set; }
    public string Local { get; set; } = null!;
    public string DescricaoAcidente { get; set; } = null!;
    public string? PartesCorpoAfetadas { get; set; }
    public string? NaturezaLesao { get; set; }
    public string? CausaImediata { get; set; }
    public string? CausaBasica { get; set; }
    public string? MedidasCorretivas { get; set; }
    public bool AfastamentoMedico { get; set; } = false;
    public int? DiasAfastamento { get; set; }
    public string? NumeroCAT { get; set; }
    public DateTime? DataCAT { get; set; }
    public string? TratamentoMedico { get; set; }
    public bool BoPoliciaRegistrado { get; set; } = false;
    public string? NumeroBO { get; set; }
    public Guid? InvestigadoPor { get; set; }
    public DateTime? DataInvestigacao { get; set; }
    public string? FotoUrl { get; set; }
    public string? Observacoes { get; set; }

    public virtual ICollection<TestemunhaAcidente> Testemunhas { get; set; } = new List<TestemunhaAcidente>();
    public virtual ICollection<AcaoCorretivaSst> AcoesCorretivas { get; set; } = new List<AcaoCorretivaSst>();
}
