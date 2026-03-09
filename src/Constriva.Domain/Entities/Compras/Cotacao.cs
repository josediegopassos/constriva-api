using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Compras;

public class Cotacao : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Numero { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public StatusCotacaoEnum Status { get; set; } = StatusCotacaoEnum.Aberta;
    public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
    public DateTime? DataFechamento { get; set; }
    public DateTime? DataLimiteResposta { get; set; }
    public string? Observacoes { get; set; }
    public string? CondicoesGerais { get; set; }
    public Guid CriadoPor { get; set; }
    public Guid? FornecedorVencedorId { get; set; }

    public virtual ICollection<ItemCotacao> Itens { get; set; } = new List<ItemCotacao>();
    public virtual ICollection<PropostaCotacao> Propostas { get; set; } = new List<PropostaCotacao>();
}
