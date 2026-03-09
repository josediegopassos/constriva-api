using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Compras;

public class PropostaCotacao : TenantEntity
{
    public Guid CotacaoId { get; set; }
    public Guid FornecedorId { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
    public DateTime? DataValidade { get; set; }
    public string? CondicoesPagamento { get; set; }
    public int? PrazoEntrega { get; set; }
    public string? Observacoes { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Vencedora { get; set; } = false;

    public virtual Cotacao Cotacao { get; set; } = null!;
    public virtual Fornecedor Fornecedor { get; set; } = null!;
    public virtual ICollection<ItemPropostaCotacao> Itens { get; set; } = new List<ItemPropostaCotacao>();
}
