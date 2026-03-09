using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class ArquivoDocumento : TenantEntity
{
    public Guid DocumentoId { get; set; }
    public string NomeArquivo { get; set; } = null!;
    public string TipoArquivo { get; set; } = null!;
    public string Url { get; set; } = null!;
    public long TamanhoBytes { get; set; }
    public int NumeroRevisao { get; set; }
    public bool Atual { get; set; } = true;
    public Guid UploadPor { get; set; }
    public string? HashArquivo { get; set; }
    public virtual DocumentoGED Documento { get; set; } = null!;
}
