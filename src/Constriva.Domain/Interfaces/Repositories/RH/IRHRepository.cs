using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IRHRepository
{
    Task<(IEnumerable<Funcionario> Items, int Total)> GetFuncionariosPagedAsync(
        Guid empresaId, string? search, Guid? obraId, StatusFuncionarioEnum? status,
        int page, int pageSize, CancellationToken ct = default);
    Task<Funcionario?> GetFuncionarioByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddFuncionarioAsync(Funcionario func, CancellationToken ct = default);
    Task<bool> CpfExisteAsync(Guid empresaId, string cpf, CancellationToken ct = default);
    Task<IEnumerable<RegistroPonto>> GetPontosAsync(Guid empresaId, Guid? funcionarioId, DateTime? inicio, DateTime? fim, CancellationToken ct = default);
    Task AddPontoAsync(RegistroPonto ponto, CancellationToken ct = default);
    Task<IEnumerable<FolhaPagamento>> GetFolhasAsync(Guid empresaId, string? competencia, CancellationToken ct = default);
    Task<IEnumerable<Cargo>> GetCargosAsync(Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Departamento>> GetDepartamentosAsync(Guid empresaId, CancellationToken ct = default);
    Task<RegistroPonto?> GetPontoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<FolhaPagamento> GerarFolhaAsync(Guid empresaId, string competencia, Guid? funcionarioId, CancellationToken ct = default);
}
