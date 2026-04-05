using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class ItemCotacao : TenantEntity
{
    public Guid CotacaoId { get; set; }
    public Guid? MaterialId { get; set; }
    public string Descricao { get; set; } = null!;
    public string UnidadeMedida { get; set; } = null!;
    public decimal Quantidade { get; set; }
    public string? Especificacao { get; set; }
    public decimal? PrecoReferencia { get; set; }
    public int Ordem { get; set; }

    public virtual Cotacao Cotacao { get; set; } = null!;
    public virtual Estoque.Material Material { get; set; } = null!;
}
