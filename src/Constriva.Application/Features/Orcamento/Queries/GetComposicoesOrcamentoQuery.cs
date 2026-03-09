using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetComposicoesOrcamentoQuery(
    Guid OrcamentoId,
    Guid EmpresaId) : IRequest<IEnumerable<ComposicaoOrcamentoDto>>, ITenantRequest;

public class GetComposicoesOrcamentoHandler
    : IRequestHandler<GetComposicoesOrcamentoQuery, IEnumerable<ComposicaoOrcamentoDto>>
{
    private readonly IComposicaoOrcamentoRepository _repo;

    public GetComposicoesOrcamentoHandler(IComposicaoOrcamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ComposicaoOrcamentoDto>> Handle(
        GetComposicoesOrcamentoQuery request, CancellationToken ct)
    {
        var composicoes = await _repo.GetByOrcamentoAsync(request.OrcamentoId, request.EmpresaId, ct);
        return composicoes
            .Where(c => !c.IsDeleted)
            .Select(OrcamentoMapper.ToComposicaoDto);
    }
}

