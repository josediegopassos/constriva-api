using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;

namespace Constriva.API.Hubs;

public interface ILensNotificationService
{
    Task NotificarProcessamentoAtualizado(DocumentoLens doc, string mensagem);
    Task NotificarProcessamentoConcluido(DocumentoLens doc, float confidenceScore, int totalItens, List<string> warnings, string? fornecedorSugerido, string? cnpjFornecedor, decimal? valorTotal, string? dataEmissao, int tempoProcessamentoMs);
    Task NotificarProcessamentoErro(DocumentoLens doc, string codigoErro, string mensagemErro, bool podeReprocessar);
    Task NotificarItemAtualizado(Guid processamentoId, Guid itemId, string acao, Guid usuarioId, string nomeUsuario, Guid? obraId, Guid empresaId);
    Task NotificarConsolidacaoConcluida(DocumentoLens doc, Guid compraId, int totalItensConsolidados, int totalItensRejeitados, decimal valorTotal);
}
