using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record CotacaoAprovadaEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";

    public Guid CotacaoId { get; init; }
    public Guid PropostaCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid ObraId { get; init; }
    public Guid EmpresaId { get; init; }
    public Guid AprovadoPorUsuarioId { get; init; }
    public DateTime AprovadaEm { get; init; }
    public string NumeroCotacao { get; init; } = string.Empty;
    public string NomeFornecedor { get; init; } = string.Empty;
    public string TelefoneFornecedor { get; init; } = string.Empty;
    public decimal ValorTotalAprovado { get; init; }
    public string? CondicoesPagamento { get; init; }
    public int? PrazoEntregaDias { get; init; }
    public DateTime? DataEntregaPrevista { get; init; }
    public IReadOnlyList<ItemAprovadoDto> ItensAprovados { get; init; } = [];
}

public record ItemAprovadoDto
{
    public Guid ItemCotacaoId { get; init; }
    public Guid ItemPropostaCotacaoId { get; init; }
    public string Descricao { get; init; } = string.Empty;
    public string UnidadeMedida { get; init; } = string.Empty;
    public decimal Quantidade { get; init; }
    public decimal PrecoUnitario { get; init; }
    public decimal ValorTotal { get; init; }
    public string? Marca { get; init; }
    public Guid? MaterialId { get; init; }
}
