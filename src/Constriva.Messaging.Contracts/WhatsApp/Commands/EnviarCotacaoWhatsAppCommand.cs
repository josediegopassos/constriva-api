using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Commands;

public record EnviarCotacaoWhatsAppCommand : ICommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";
    public Guid CotacaoId { get; init; }
    public Guid FornecedorCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid EmpresaId { get; init; }
    public string NumeroCotacao { get; init; } = string.Empty;
    public string TituloCotacao { get; init; } = string.Empty;
    public string NomeFornecedor { get; init; } = string.Empty;
    public string TelefoneWhatsApp { get; init; } = string.Empty;
    public DateTime DataLimiteResposta { get; init; }
    public string UrlFormulario { get; init; } = string.Empty;
    public IReadOnlyList<ItemCotacaoDto> Itens { get; init; } = [];
    public string? MensagemPersonalizada { get; init; }
}

public record ItemCotacaoDto
{
    public Guid ItemCotacaoId { get; init; }
    public string Descricao { get; init; } = string.Empty;
    public string UnidadeMedida { get; init; } = string.Empty;
    public decimal Quantidade { get; init; }
    public string? Especificacao { get; init; }
    public decimal? PrecoReferencia { get; init; }
}
