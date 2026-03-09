using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras;

public class ObraAnexo : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Nome { get; set; } = null!;
    public string TipoArquivo { get; set; } = null!;
    public string Url { get; set; } = null!;
    public long TamanhoBytes { get; set; }
    public string? Descricao { get; set; }
    public Guid UsuarioUploadId { get; set; }
    public virtual Obra Obra { get; set; } = null!;
}
