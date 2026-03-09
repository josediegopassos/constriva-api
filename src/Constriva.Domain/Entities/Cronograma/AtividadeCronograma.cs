using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Cronograma;

public class AtividadeCronograma : TenantEntity
{
    public Guid CronogramaId { get; set; }
    public Guid? AtividadePaiId { get; set; }
    public Guid? FaseObraId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public int Nivel { get; set; } = 1;
    public int Ordem { get; set; }
    public bool EAgrupadoa { get; set; } = false;
    public bool EMarcador { get; set; } = false;
    public StatusAtividadeEnum Status { get; set; } = StatusAtividadeEnum.NaoIniciada;
    public int DuracaoDias { get; set; }
    public decimal PercentualConcluido { get; set; } = 0;

    // Datas planejadas
    public DateTime DataInicioPlanejada { get; set; }
    public DateTime DataFimPlanejada { get; set; }

    // Datas reais
    public DateTime? DataInicioReal { get; set; }
    public DateTime? DataFimReal { get; set; }

    // Datas previstas (reprogramação)
    public DateTime? DataInicioReprogramada { get; set; }
    public DateTime? DataFimReprogramada { get; set; }

    // EVM (Earned Value Management)
    public decimal BCWS { get; set; } = 0;   // Budget Cost Work Scheduled
    public decimal BCWP { get; set; } = 0;   // Budget Cost Work Performed
    public decimal ACWP { get; set; } = 0;   // Actual Cost Work Performed

    public decimal SV => BCWP - BCWS;        // Schedule Variance
    public decimal CV => BCWP - ACWP;        // Cost Variance
    public decimal SPI => BCWS == 0 ? 0 : BCWP / BCWS;  // Schedule Performance Index
    public decimal CPI => ACWP == 0 ? 0 : BCWP / ACWP;  // Cost Performance Index

    public decimal CustoOrcado { get; set; }
    public decimal CustoRealizado { get; set; }

    public bool NoCaminhosCritico { get; set; } = false;
    public int Folga { get; set; } = 0;

    public string? ResponsavelId { get; set; }
    public string? Cor { get; set; }
    public string? Observacoes { get; set; }

    public virtual CronogramaObra Cronograma { get; set; } = null!;
    public virtual AtividadeCronograma? AtividadePai { get; set; }
    public virtual ICollection<AtividadeCronograma> SubAtividades { get; set; } = new List<AtividadeCronograma>();
    public virtual ICollection<VinculoAtividade> Predecessoras { get; set; } = new List<VinculoAtividade>();
    public virtual ICollection<VinculoAtividade> Sucessoras { get; set; } = new List<VinculoAtividade>();
    public virtual ICollection<RecursoAtividade> Recursos { get; set; } = new List<RecursoAtividade>();
}
