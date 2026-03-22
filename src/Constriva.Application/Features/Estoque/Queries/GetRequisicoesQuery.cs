using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Estoque.Commands;

namespace Constriva.Application.Features.Estoque;

public record GetRequisicoesQuery(Guid EmpresaId, Guid? ObraId = null, StatusRequisicaoEnum? Status = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<RequisicaoDto>>, ITenantRequest;

public class GetRequisicoesHandler : IRequestHandler<GetRequisicoesQuery, PaginatedResult<RequisicaoDto>>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public GetRequisicoesHandler(IEstoqueRepository repo, IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<PaginatedResult<RequisicaoDto>> Handle(GetRequisicoesQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetRequisicoesPagedAsync(r.EmpresaId, r.ObraId, r.Status, r.Page, r.PageSize, ct);

        var solicitanteIds = items.Select(i => i.SolicitanteId).Distinct();
        var usuarios = await _usuarioRepo.FindAsync(u => solicitanteIds.Contains(u.Id), ct);
        var nomes = usuarios.ToDictionary(u => u.Id, u => u.Nome);

        return new PaginatedResult<RequisicaoDto>
        {
            Items = items.Select(req => CreateRequisicaoHandler.ToDto(req, nomes.GetValueOrDefault(req.SolicitanteId, ""))),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
