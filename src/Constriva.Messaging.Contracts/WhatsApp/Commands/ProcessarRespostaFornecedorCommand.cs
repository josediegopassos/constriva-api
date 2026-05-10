using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Commands;

public record ProcessarRespostaFornecedorCommand : ICommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";
    public Guid EmpresaId { get; init; }
    public string WaMessageId { get; init; } = string.Empty;
    public string TelefoneOrigem { get; init; } = string.Empty;
    public string TelefoneDestino { get; init; } = string.Empty;
    public DateTime RecebidaEm { get; init; }
    public TipoConteudoWhatsApp TipoConteudo { get; init; }
    public string? TextoMensagem { get; init; }
    public string? MediaId { get; init; }
    public string? MediaMimeType { get; init; }
    public string? MediaNomeArquivo { get; init; }
    public string PayloadWebhookOriginal { get; init; } = string.Empty;
}

public enum TipoConteudoWhatsApp
{
    Texto = 0,
    Imagem = 1,
    Documento = 2,
    Audio = 3,
    Desconhecido = 99
}
