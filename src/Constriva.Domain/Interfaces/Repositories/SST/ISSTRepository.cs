using Constriva.Domain.Entities.SST;

namespace Constriva.Domain.Interfaces.Repositories;

public interface ISSTRepository
{
    Task<(IEnumerable<DDS> Items, int Total)> GetDDSPagedAsync(
        Guid empresaId, Guid? obraId, int page, int pageSize, CancellationToken ct = default);
    Task<DDS?> GetDDSByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddDDSAsync(DDS dds, CancellationToken ct = default);
    Task<IEnumerable<RegistroAcidente>> GetAcidentesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<RegistroAcidente?> GetAcidenteByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddAcidenteAsync(RegistroAcidente acidente, CancellationToken ct = default);
    Task<SSTIndicadoresData> GetIndicadoresAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<IEnumerable<EPI>> GetEPIsAsync(Guid empresaId, CancellationToken ct = default);
    Task<EPI?> GetEPIByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddEPIAsync(EPI epi, CancellationToken ct = default);
}

public record SSTIndicadoresData(
    int TotalDDS, int TotalAcidentes, int AcidentesGraves,
    int DiasParados, decimal TaxaFrequencia, decimal TaxaGravidade
);
