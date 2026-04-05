using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IDocumentoLensRepository
{
    Task<(List<DocumentoLens> Items, int TotalCount)> GetProcessamentosPagedAsync(
        Guid empresaId, Guid? obraId, StatusProcessamentoLensEnum? status,
        TipoDocumentoLensEnum? tipoDocumento, DateTime? de, DateTime? ate,
        int page, int pageSize, CancellationToken ct);

    Task<DocumentoLens?> GetByIdWithItensAsync(Guid id, Guid empresaId, CancellationToken ct);

    Task<DocumentoLens?> GetResultadoWithMatchingAsync(Guid id, Guid empresaId, CancellationToken ct);

    // Analytics
    Task<(int Total, int Sucesso, int Erro, float ConfidenceMedio, int TempoMedio, int TotalItens, int TotalConsolidados)> GetResumoAsync(
        Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);

    Task<List<(TipoDocumentoLensEnum TipoDocumento, int Total, int Sucesso, int Erro, float ConfidenceMedio)>> GetPorTipoAsync(
        Guid empresaId, DateTime? de, DateTime? ate, CancellationToken ct);

    Task<List<(DateTime Data, float ConfidenceMedio, int TotalDocumentos)>> GetTendenciaConfidenceAsync(
        Guid empresaId, DateTime? de, DateTime? ate, CancellationToken ct);

    Task<List<(string Warning, int Frequencia)>> GetWarningsFrequentesAsync(
        Guid empresaId, int limite, CancellationToken ct);
}
