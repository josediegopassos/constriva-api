using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class DDS : TenantEntity  // Diálogo Diário de Segurança
{
    public Guid ObraId { get; set; }
    public string Numero { get; set; } = null!;
    public DateTime DataRealizacao { get; set; }
    public string Tema { get; set; } = null!;
    public string? Conteudo { get; set; }
    public string? Ministrador { get; set; }
    public decimal DuracaoMinutos { get; set; }
    public string? Local { get; set; }
    public int NumeroParticipantes { get; set; }
    public Guid RegistradoPor { get; set; }
    public string? FotoUrl { get; set; }
    public virtual ICollection<ParticipateDDS> Participantes { get; set; } = new List<ParticipateDDS>();
}
