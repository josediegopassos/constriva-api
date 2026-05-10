using Constriva.Domain.Enums;

namespace Constriva.Domain.ValueObjects.WhatsApp;

public class EntradaExtratorValueObject
{
    public required Guid RespostaFornecedorWhatsAppId { get; init; }
    public required Guid CotacaoId { get; init; }
    public required string NumeroCotacao { get; init; }
    public required string NomeFornecedor { get; init; }
    public required TipoConteudoMensagemEnum TipoConteudo { get; init; }
    public string? TextoMensagem { get; init; }
    public byte[]? ConteudoMidia { get; init; }
    public string? MimeTypeMidia { get; init; }
    public required IReadOnlyList<ItemCotacaoReferenciaValueObject> ItensCotacao { get; init; }
}
