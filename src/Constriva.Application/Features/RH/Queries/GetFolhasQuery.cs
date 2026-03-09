using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetFolhasQuery(Guid EmpresaId, string? Competencia = null)
    : IRequest<IEnumerable<FolhaPagamentoDto>>, ITenantRequest;

public class GetFolhasHandler : IRequestHandler<GetFolhasQuery, IEnumerable<FolhaPagamentoDto>>
{
    private readonly IRHRepository _repo;
    public GetFolhasHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<FolhaPagamentoDto>> Handle(GetFolhasQuery r, CancellationToken ct)
    {
        var items = await _repo.GetFolhasAsync(r.EmpresaId, r.Competencia, ct);
        return items.Select(f => new FolhaPagamentoDto(
            f.Id, f.Competencia, Guid.Empty, "",
            f.ValorTotalBruto, 0m, f.ValorTotalDescontos, f.ValorTotalLiquido));
    }
}
