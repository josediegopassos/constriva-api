using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IGEDRepository
{
    Task<IEnumerable<PastaDocumento>> GetPastasAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<PastaDocumento?> GetPastaAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<(IEnumerable<DocumentoGED> Items, int Total)> GetDocumentosPagedAsync(
        Guid empresaId, Guid? pastaId, string? search, TipoDocumentoGEDEnum? tipo,
        int page, int pageSize, CancellationToken ct = default);
    Task<DocumentoGED?> GetDocumentoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddDocumentoAsync(DocumentoGED doc, CancellationToken ct = default);
    Task AddPastaAsync(PastaDocumento pasta, CancellationToken ct = default);
}
