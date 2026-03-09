using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Contratos;

public class ContratoAnexo : TenantEntity
{
    public Guid ContratoId { get; set; }
    public string Nome { get; set; } = null!;
    public string TipoArquivo { get; set; } = null!;
    public string Url { get; set; } = null!;
    public long TamanhoBytes { get; set; }
    public Guid UploadPor { get; set; }
    public virtual Contrato Contrato { get; set; } = null!;
}
