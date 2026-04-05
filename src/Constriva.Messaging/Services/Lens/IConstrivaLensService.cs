using Constriva.Messaging.Models.Lens;

namespace Constriva.Messaging.Services.Lens;

/// <summary>
/// Serviço de comunicação com o microserviço Constriva.Lens (OCR).
/// </summary>
public interface IConstrivaLensService
{
    /// <summary>
    /// Processa um documento via OCR no Constriva.Lens.
    /// </summary>
    /// <param name="caminhoArquivo">Caminho completo do arquivo a processar.</param>
    /// <param name="tipoDocumento">Tipo do documento (NFE, NFSE, etc.).</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Resposta com dados extraídos pelo OCR.</returns>
    Task<LensExtracaoResposta> ProcessarDocumentoAsync(string caminhoArquivo, string tipoDocumento, CancellationToken ct);

    /// <summary>
    /// Verifica se o serviço Constriva.Lens está disponível.
    /// </summary>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>True se o serviço está saudável.</returns>
    Task<bool> VerificarSaudeAsync(CancellationToken ct);
}
