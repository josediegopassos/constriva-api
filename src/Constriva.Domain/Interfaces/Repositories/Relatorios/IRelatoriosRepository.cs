using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Entities.Obras;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IRelatoriosRepository
{
    Task<IEnumerable<Obra>> GetObrasParaRelatorioAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<IEnumerable<LancamentoFinanceiro>> GetLancamentosParaRelatorioAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<IEnumerable<AtividadeCronograma>> GetAtividadesParaKPIAsync(Guid empresaId, Guid obraId, CancellationToken ct = default);
}
