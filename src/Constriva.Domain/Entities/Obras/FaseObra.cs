using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Obras;

public class FaseObra : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? FasePaiId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public int Ordem { get; set; }
    public StatusFaseEnum Status { get; set; } = StatusFaseEnum.NaoIniciada;
    public DateTime DataInicioPrevista { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime? DataInicioReal { get; set; }
    public DateTime? DataFimReal { get; set; }
    public decimal PercentualConcluido { get; set; } = 0;
    public decimal ValorPrevisto { get; set; }
    public decimal ValorRealizado { get; set; }
    public Guid? ResponsavelId { get; set; }
    public string? Cor { get; set; }

    public virtual Obra Obra { get; set; } = null!;
    public virtual FaseObra? FasePai { get; set; }
    public virtual ICollection<FaseObra> SubFases { get; set; } = new List<FaseObra>();
}
