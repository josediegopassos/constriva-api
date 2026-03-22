using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IEstoqueRepository
{
    Task<IEnumerable<EstoqueSaldo>> GetSaldosAsync(Guid empresaId, Guid? almoxarifadoId, CancellationToken ct = default);
    Task<IEnumerable<MovimentacaoEstoque>> GetMovimentacoesAsync(Guid empresaId, Guid? almoxarifadoId, DateTime? inicio, DateTime? fim, CancellationToken ct = default);
    Task<IEnumerable<Almoxarifado>> GetAlmoxarifadosAsync(Guid empresaId, CancellationToken ct = default);
    Task<(IEnumerable<RequisicaoMaterial> Items, int Total)> GetRequisicoesPagedAsync(Guid empresaId, Guid? obraId, StatusRequisicaoEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<RequisicaoMaterial?> GetRequisicaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddRequisicaoAsync(RequisicaoMaterial req, CancellationToken ct = default);
    Task AddMovimentacaoAsync(MovimentacaoEstoque mov, CancellationToken ct = default);
    Task<Almoxarifado?> GetAlmoxarifadoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddAlmoxarifadoAsync(Almoxarifado almoxarifado, CancellationToken ct = default);
    Task<IEnumerable<GrupoMaterial>> GetGruposAsync(Guid empresaId, CancellationToken ct = default);
    Task<EstoqueSaldo?> GetSaldoAsync(Guid almoxarifadoId, Guid materialId, Guid empresaId, CancellationToken ct = default);
    Task AddSaldoAsync(EstoqueSaldo saldo, CancellationToken ct = default);
    Task AddItemRequisicaoAsync(ItemRequisicao item, CancellationToken ct = default);
    Task<MovimentacaoEstoque?> GetMovimentacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task SoftDeleteByMaterialIdAsync(Guid materialId, Guid empresaId, CancellationToken ct = default);
}
