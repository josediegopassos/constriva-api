using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras;

public class ObraHistorico : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Acao { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string? ValorAnterior { get; set; }
    public string? ValorNovo { get; set; }
    public Guid UsuarioId { get; set; }
    public virtual Obra Obra { get; set; } = null!;
}
