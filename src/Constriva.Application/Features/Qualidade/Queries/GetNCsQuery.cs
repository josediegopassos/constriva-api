using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.Qualidade;

public record GetNCsQuery(
    Guid EmpresaId, Guid? ObraId = null, StatusNaoConformidadeEnum? Status = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<NaoConformidadeDto>>, ITenantRequest;

public class GetNCsHandler : IRequestHandler<GetNCsQuery, PaginatedResult<NaoConformidadeDto>>
{
    private readonly IQualidadeRepository _repo;
    public GetNCsHandler(IQualidadeRepository repo) => _repo = repo;

    public async Task<PaginatedResult<NaoConformidadeDto>> Handle(GetNCsQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetNCsPagedAsync(
            r.EmpresaId, r.ObraId, r.Status, r.Page, r.PageSize, ct);
        return new PaginatedResult<NaoConformidadeDto>
        {
            Items = items.Select(nc => new NaoConformidadeDto(
                nc.Id, nc.ObraId, nc.Numero, nc.Titulo, nc.Descricao,
                nc.Status, nc.Gravidade, nc.LocalizacaoObra,
                nc.CausaRaiz, nc.AcaoCorretiva,
                nc.DataAbertura, nc.DataPrazoConclusao, nc.DataEncerramento)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
