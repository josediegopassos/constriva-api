using Constriva.Messaging.Models.Lens;

namespace Constriva.Messaging.Services.Lens;

public interface IConstrivaLensService
{
    Task<LensExtracaoResposta> ProcessarDocumentoAsync(string caminhoArquivo, string tipoDocumento, CancellationToken ct);
    Task<bool> VerificarSaudeAsync(CancellationToken ct);
}
