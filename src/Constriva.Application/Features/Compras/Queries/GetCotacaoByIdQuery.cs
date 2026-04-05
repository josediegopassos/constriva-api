using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetCotacaoByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<CotacaoDto?>, ITenantRequest;

public class GetCotacaoByIdHandler : IRequestHandler<GetCotacaoByIdQuery, CotacaoDto?>
{
    private readonly IComprasRepository _repo;
    public GetCotacaoByIdHandler(IComprasRepository repo) => _repo = repo;

    public async Task<CotacaoDto?> Handle(GetCotacaoByIdQuery request, CancellationToken ct)
    {
        var c = await _repo.GetCotacaoByIdAsync(request.Id, request.EmpresaId, ct);
        return c is null ? null : CotacaoMapper.ToDto(c);
    }
}
