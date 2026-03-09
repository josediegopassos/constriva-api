using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetOrcamentoDetalheQuery(
    Guid Id,
    Guid EmpresaId) : IRequest<OrcamentoDetalheDto?>, ITenantRequest;

public class GetOrcamentoDetalheHandler
    : IRequestHandler<GetOrcamentoDetalheQuery, OrcamentoDetalheDto?>
{
    private readonly IOrcamentoRepository _repo;

    public GetOrcamentoDetalheHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<OrcamentoDetalheDto?> Handle(
        GetOrcamentoDetalheQuery request, CancellationToken ct)
    {
        var orcamento = await _repo.GetWithGruposItensAsync(request.Id, request.EmpresaId, ct);
        return orcamento == null ? null : OrcamentoMapper.ToDetalheDto(orcamento);
    }
}

