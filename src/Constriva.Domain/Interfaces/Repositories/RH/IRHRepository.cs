using Constriva.Domain.Entities.Common;
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
    Task<Cargo?> GetCargoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddCargoAsync(Cargo cargo, CancellationToken ct = default);
    Task<int> GetCargosCountAsync(Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Departamento>> GetDepartamentosAsync(Guid empresaId, CancellationToken ct = default);
    Task<Departamento?> GetDepartamentoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddDepartamentoAsync(Departamento departamento, CancellationToken ct = default);
    Task<IEnumerable<Funcionario>> GetFuncionariosAtivosAsync(Guid empresaId, CancellationToken ct = default);
    Task<int> GetFuncionariosCountAsync(Guid empresaId, CancellationToken ct = default);
    Task AddEnderecoAsync(Endereco endereco, CancellationToken ct = default);
    Task AddDadosBancariosAsync(DadosBancarios dados, CancellationToken ct = default);
    Task<IEnumerable<Banco>> GetBancosAsync(CancellationToken ct = default);
    Task<RegistroPonto?> GetPontoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<FolhaPagamento> GerarFolhaAsync(Guid empresaId, string competencia, Guid? funcionarioId, CancellationToken ct = default);
    Task<IEnumerable<FolhaFuncionario>> GetFolhasFuncionarioAsync(Guid funcionarioId, Guid empresaId, CancellationToken ct = default);
}
