using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class ItemPropostaCotacao : TenantEntity
{
    public Guid PropostaId { get; set; }
    public Guid ItemCotacaoId { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Quantidade { get; set; }
    public decimal ValorTotal => PrecoUnitario * Quantidade;
    public string? Marca { get; set; }
    public string? Observacao { get; set; }
    public bool Disponivel { get; set; } = true;
    public bool MenorPreco { get; set; } = false;

    public virtual PropostaCotacao Proposta { get; set; } = null!;
    public virtual ItemCotacao ItemCotacao { get; set; } = null!;
}
