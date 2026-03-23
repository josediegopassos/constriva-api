using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Queries;

public record GetObrasDashboardQuery(Guid EmpresaId)
    : IRequest<ObrasDashboardDto>, ITenantRequest;

public class GetObrasDashboardHandler : IRequestHandler<GetObrasDashboardQuery, ObrasDashboardDto>
{
    private readonly IObraRepository _repo;
    public GetObrasDashboardHandler(IObraRepository repo) => _repo = repo;

    public async Task<ObrasDashboardDto> Handle(GetObrasDashboardQuery r, CancellationToken ct)
    {
        var todas = await _repo.GetAllByEmpresaComEnderecoAsync(r.EmpresaId, ct);
        var ativas = todas.Where(o => !o.IsDeleted).ToList();
        var hoje = DateTime.Today;

        var ultimas = ativas.OrderByDescending(o => o.CreatedAt).Take(5)
            .Select(o => new ObraResumoDto(
                o.Id, o.Codigo, o.Nome, o.Tipo, o.Status, o.Status.ToString(),
                o.Endereco?.Cidade, o.Endereco?.Estado, o.DataInicioPrevista, o.DataFimPrevista,
                o.ValorContrato, o.PercentualConcluido,
                o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje,
                o.FotoUrl));

        var emAndamento = ativas.Where(o => o.Status == StatusObraEnum.EmAndamento).ToList();
        return new ObrasDashboardDto(
            ativas.Count,
            emAndamento.Count,
            ativas.Count(o => o.Status == StatusObraEnum.Concluida),
            ativas.Count(o => o.Status == StatusObraEnum.Paralisada),
            ativas.Sum(o => o.ValorContrato),
            emAndamento.Any() ? (decimal)emAndamento.Average(o => (double)o.PercentualConcluido) : 0m,
            emAndamento.Count(o => o.DataFimPrevista < hoje),
            ultimas);
    }
}

