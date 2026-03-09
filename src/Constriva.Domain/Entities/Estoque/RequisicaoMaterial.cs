using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Estoque;

public class RequisicaoMaterial : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? FaseObraId { get; set; }
    public Guid AlmoxarifadoId { get; set; }
    public string Numero { get; set; } = null!;
    public DateTime DataRequisicao { get; set; } = DateTime.UtcNow;
    public DateTime? DataNecessidade { get; set; }
    public StatusRequisicaoEnum Status { get; set; } = StatusRequisicaoEnum.Pendente;
    public string Motivo { get; set; } = null!;
    public Guid SolicitanteId { get; set; }
    public Guid? AprovadorId { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? MotivoRejeicao { get; set; }
    public string? Observacoes { get; set; }

    public virtual ICollection<ItemRequisicao> Itens { get; set; } = new List<ItemRequisicao>();
}
