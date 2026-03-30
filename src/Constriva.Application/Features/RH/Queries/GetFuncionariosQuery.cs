using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.RH;

public record GetFuncionariosQuery(
    Guid EmpresaId, string? Search = null, Guid? ObraId = null,
    StatusFuncionarioEnum? Status = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<FuncionarioDto>>, ITenantRequest;

public class GetFuncionariosHandler : IRequestHandler<GetFuncionariosQuery, PaginatedResult<FuncionarioDto>>
{
    private readonly IRHRepository _repo;
    public GetFuncionariosHandler(IRHRepository repo) => _repo = repo;

    public async Task<PaginatedResult<FuncionarioDto>> Handle(GetFuncionariosQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetFuncionariosPagedAsync(
            r.EmpresaId, r.Search, r.ObraId, r.Status, r.Page, r.PageSize, ct);
        return new PaginatedResult<FuncionarioDto>
        {
            Items = items.Select(f => new FuncionarioDto(
                f.Id, f.Matricula, f.Nome, f.NomeSocial, f.Cpf, f.Email, f.Telefone,
                f.CargoId, f.Cargo?.Nome, f.DepartamentoId, f.Departamento?.Nome,
                f.ObraAtualId, f.ObraAtual?.Nome,
                f.TipoContratacaoEnum, f.DataAdmissao, f.SalarioBase, f.Status)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
