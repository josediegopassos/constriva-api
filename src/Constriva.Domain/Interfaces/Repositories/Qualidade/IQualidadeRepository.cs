using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IQualidadeRepository
{
    Task<(IEnumerable<Inspecao> Items, int Total)> GetInspecoesPagedAsync(
        Guid empresaId, Guid? obraId, StatusInspecaoEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task AddInspecaoAsync(Inspecao inspecao, CancellationToken ct = default);
    Task<Inspecao?> GetInspecaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<(IEnumerable<NaoConformidade> Items, int Total)> GetNCsPagedAsync(
        Guid empresaId, Guid? obraId, StatusNaoConformidadeEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task AddNCAsync(NaoConformidade nc, CancellationToken ct = default);
    Task<NaoConformidade?> GetNCByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<FVS>> GetFVSsAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<FVS?> GetFVSByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
}
