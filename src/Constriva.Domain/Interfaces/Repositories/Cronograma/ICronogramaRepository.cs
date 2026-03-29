using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Domain.Interfaces.Repositories;

public interface ICronogramaRepository
{
    Task<CronogramaObra?> GetByObraAsync(Guid obraId, Guid empresaId, CancellationToken ct = default);
    Task<CronogramaObra?> GetWithAtividadesAsync(Guid obraId, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<CronogramaObra>> GetAllWithAtividadesAsync(Guid empresaId, CancellationToken ct = default);
    Task<CronogramaObra?> GetByIdDetalhadoAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddCronogramaAsync(CronogramaObra cronograma, CancellationToken ct = default);
    Task<AtividadeCronograma?> GetAtividadeByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddAtividadeAsync(AtividadeCronograma atividade, CancellationToken ct = default);
    Task<int> GetMaxOrdemAsync(Guid cronogramaId, CancellationToken ct = default);
    Task<IEnumerable<CurvaSPonto>> GetCurvaSAsync(Guid cronogramaId, Guid empresaId, CancellationToken ct = default);
    Task RemoveCurvaSAsync(Guid cronogramaId, Guid empresaId, CancellationToken ct = default);
    Task AddCurvaSRangeAsync(IEnumerable<CurvaSPonto> pontos, CancellationToken ct = default);
    Task<IEnumerable<VinculoAtividade>> GetVinculosAsync(Guid cronogramaId, CancellationToken ct = default);
}
