using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IContratoRepository : ITenantRepository<Contrato>
{
    Task<(IEnumerable<Contrato> Items, int Total)> GetPagedAsync(
        Guid empresaId, Guid? obraId, StatusContratoEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<MedicaoContratual>> GetMedicoesAsync(Guid contratoId, Guid empresaId, CancellationToken ct = default);
    Task AddMedicaoAsync(MedicaoContratual medicao, CancellationToken ct = default);
    Task<int> GetCountByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task<decimal> GetTotalMedicoesAsync(Guid contratoId, CancellationToken ct = default);
    Task<MedicaoContratual?> GetMedicaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
}
