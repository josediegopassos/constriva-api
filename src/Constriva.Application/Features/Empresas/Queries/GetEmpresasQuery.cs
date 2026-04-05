using FluentValidation;
using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Empresas.DTOs;

namespace Constriva.Application.Features.Empresas;

public record GetEmpresasQuery(int Page = 1, int PageSize = 20, string? Search = null, StatusEmpresaEnum? Status = null) : IRequest<PaginatedResult<EmpresaDto>>;

public class GetEmpresasHandler : IRequestHandler<GetEmpresasQuery, PaginatedResult<EmpresaDto>>
{
    private readonly IEmpresaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public GetEmpresasHandler(IEmpresaRepository repo, IUsuarioRepository usuarioRepo) { _repo = repo; _usuarioRepo = usuarioRepo; }

    public async Task<PaginatedResult<EmpresaDto>> Handle(GetEmpresasQuery r, CancellationToken ct)
    {
        var query = _repo.Query().Where(e => !e.IsDeleted);
        if (r.Status.HasValue) query = query.Where(e => e.Status == r.Status);
        if (!string.IsNullOrEmpty(r.Search))
            query = query.Where(e => e.RazaoSocial.Contains(r.Search) || e.NomeFantasia.Contains(r.Search) || e.Cnpj.Contains(r.Search));

        var total = query.Count();
        var items = query.OrderByDescending(e => e.CreatedAt)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize).ToList();

        var dtos = items.Select(e => MapDto(e, 0, 0)).ToList();
        return new PaginatedResult<EmpresaDto> { Items = dtos, TotalCount = total, Page = r.Page, PageSize = r.PageSize };
    }

    private static EmpresaDto MapDto(Empresa e, int totalUsuarios, int totalObras) => new(
        e.Id, e.RazaoSocial, e.NomeFantasia, e.Cnpj, e.Email, e.Telefone, e.LogoUrl,
        e.Status, e.Plano, e.DataVencimento, e.MaxUsuarios, e.MaxObras, e.Status == StatusEmpresaEnum.Ativa,
        e.Cidade, e.Estado,
        new(e.ModuloObras, e.ModuloEstoque, e.ModuloCronograma, e.ModuloOrcamento,
            e.ModuloCompras, e.ModuloQualidade, e.ModuloContratos, e.ModuloRH,
            e.ModuloFinanceiro, e.ModuloSST, e.ModuloGED, e.ModuloRelatorios, e.ModuloClientes, e.ModuloFornecedores, e.ModuloAgente, e.ModuloLens),
        totalUsuarios, totalObras, e.CreatedAt);
}
