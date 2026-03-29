using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Cronograma;

public class CronogramaObra : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public bool ELinhaDBase { get; set; } = false;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public bool Ativo { get; set; } = true;
    public int Versao { get; set; } = 1;
    public Guid? VersaoBaseadaEm { get; set; }
    public string? Observacoes { get; set; }

    public virtual Obras.Obra Obra { get; set; } = null!;
    public virtual ICollection<AtividadeCronograma> Atividades { get; set; } = new List<AtividadeCronograma>();
    public virtual ICollection<CurvaSPonto> CurvaS { get; set; } = new List<CurvaSPonto>();
}
