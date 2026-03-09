using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Cronograma;

public class VinculoAtividade : TenantEntity
{
    public Guid AtividadeOrigemId { get; set; }
    public Guid AtividadeDestinoId { get; set; }
    public TipoVinculoEnum Tipo { get; set; } = TipoVinculoEnum.FS;
    public int Lag { get; set; } = 0;  // dias de defasagem

    public virtual AtividadeCronograma AtividadeOrigem { get; set; } = null!;
    public virtual AtividadeCronograma AtividadeDestino { get; set; } = null!;
}
