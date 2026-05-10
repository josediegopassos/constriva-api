using Constriva.Domain.ValueObjects.WhatsApp;

namespace Constriva.Domain.Interfaces.WhatsApp;

public interface IWhatsAppGateway
{
    Task<string> EnviarConviteCotacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        string tituloCotacao,
        DateTime dataLimiteResposta,
        string urlFormulario,
        CancellationToken cancellationToken = default);

    Task<string> EnviarLembreteCotacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        DateTime dataLimiteResposta,
        int numeroTentativa,
        CancellationToken cancellationToken = default);

    Task<string> EnviarConfirmacaoAprovacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        decimal valorTotal,
        int? prazoEntregaDias,
        CancellationToken cancellationToken = default);

    Task<string> EnviarTextoLivreAsync(
        string telefoneDestino,
        string texto,
        string? contextoWaMessageId = null,
        CancellationToken cancellationToken = default);

    Task<MediaInfoValueObject> ObterInfoMediaAsync(
        string waMediaId,
        CancellationToken cancellationToken = default);

    Task<byte[]> DownloadMediaAsync(
        string mediaUrl,
        CancellationToken cancellationToken = default);

    Task MarcarMensagemComoLidaAsync(
        string waMessageId,
        CancellationToken cancellationToken = default);

    bool ValidarAssinaturaWebhook(string payloadRaw, string assinaturaRecebida);

    bool ValidarTokenVerificacao(string mode, string token, string challenge, out string challengeResponse);
}
