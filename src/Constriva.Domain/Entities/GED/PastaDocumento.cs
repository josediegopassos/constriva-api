using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class PastaDocumento : TenantEntity
{
    public Guid? ObraId { get; set; }
    public Guid? PastaPaiId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Cor { get; set; }
    public bool Ativo { get; set; } = true;
    public string? CaminhoCompleto { get; set; }
    public bool AcessoPublico { get; set; } = false;

    public virtual PastaDocumento? PastaPai { get; set; }
    public virtual ICollection<PastaDocumento> SubPastas { get; set; } = new List<PastaDocumento>();
    public virtual ICollection<DocumentoGED> Documentos { get; set; } = new List<DocumentoGED>();
}
