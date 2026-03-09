using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque;

public record GetSaldosEstoqueQuery(Guid EmpresaId, Guid? AlmoxarifadoId = null) : IRequest<IEnumerable<SaldoEstoqueDto>>, ITenantRequest;

public class GetSaldosEstoqueHandler : IRequestHandler<GetSaldosEstoqueQuery, IEnumerable<SaldoEstoqueDto>>
{
    private readonly IEstoqueRepository _repo;
    public GetSaldosEstoqueHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<IEnumerable<SaldoEstoqueDto>> Handle(GetSaldosEstoqueQuery r, CancellationToken ct)
    {
        var saldos = await _repo.GetSaldosAsync(r.EmpresaId, r.AlmoxarifadoId, ct);
        return saldos.Select(s => new SaldoEstoqueDto(
            s.Id, s.AlmoxarifadoId, s.Almoxarifado?.Nome ?? "",
            s.MaterialId, s.Material?.Nome ?? "", s.Material?.Codigo ?? "", s.Material?.UnidadeMedida ?? "",
            s.SaldoAtual, s.SaldoReservado, s.SaldoAtual - s.SaldoReservado,
            s.CustoMedio, s.UltimaMovimentacao));
    }
}
