using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetOrcamentoDashboardQuery(
    Guid ObraId,
    Guid EmpresaId) : IRequest<OrcamentoDashboardDto>, ITenantRequest;

public class GetOrcamentoDashboardHandler
    : IRequestHandler<GetOrcamentoDashboardQuery, OrcamentoDashboardDto>
{
    private readonly IOrcamentoRepository _repo;

    public GetOrcamentoDashboardHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<OrcamentoDashboardDto> Handle(
        GetOrcamentoDashboardQuery request, CancellationToken ct)
    {
        var orcamentos = (await _repo.GetByObraAsync(request.ObraId, request.EmpresaId, ct)).ToList();

        var aprovados = orcamentos.Where(o => o.Status == Domain.Enums.StatusOrcamentoEnum.Aprovado).ToList();
        var ultimoAtualizado = orcamentos.OrderByDescending(o => o.UpdatedAt).FirstOrDefault();

        return new OrcamentoDashboardDto(
            TotalOrcamentos: orcamentos.Count,
            Rascunhos: orcamentos.Count(o => o.Status == Domain.Enums.StatusOrcamentoEnum.Rascunho),
            EmRevisao: orcamentos.Count(o => o.Status == Domain.Enums.StatusOrcamentoEnum.EmAnalise),
            Aprovados: aprovados.Count,
            ValorTotalAprovados: aprovados.Sum(o => o.ValorTotal),
            ValorMedioOrcamentos: orcamentos.Count > 0 ? orcamentos.Average(o => o.ValorTotal) : 0,
            UltimoAtualizado: ultimoAtualizado == null
                ? null
                : OrcamentoMapper.ToResumoDto(ultimoAtualizado, 0));
    }
}

