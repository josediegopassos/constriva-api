using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Lens;

public class ItemDocumentoLens : BaseEntity
{
    public Guid DocumentoLensId { get; set; }
    public Guid? ProdutoId { get; set; }
    public int OrdemItem { get; set; }
    public string? Codigo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? Ncm { get; set; }
    public string? Cfop { get; set; }
    public string? Unidade { get; set; }
    public decimal? Quantidade { get; set; }
    public decimal? PrecoUnitario { get; set; }
    public decimal? PrecoTotal { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? AliquotaIcms { get; set; }
    public decimal? AliquotaIpi { get; set; }
    public StatusItemLensEnum Status { get; set; } = StatusItemLensEnum.Pendente;
    public string? MotivoRejeicao { get; set; }
    public string? DescricaoOriginalOcr { get; set; }
    public decimal? QuantidadeOriginalOcr { get; set; }
    public decimal? PrecoUnitarioOriginalOcr { get; set; }
    public decimal? PrecoTotalOriginalOcr { get; set; }
    public Guid? EditadoPorUsuarioId { get; set; }
    public DateTime? EditadoEm { get; set; }

    public virtual DocumentoLens DocumentoLens { get; set; } = null!;
}
