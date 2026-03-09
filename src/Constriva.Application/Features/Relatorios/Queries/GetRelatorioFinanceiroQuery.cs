using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Relatorios.DTOs;

namespace Constriva.Application.Features.Relatorios;

public record GetRelatorioFinanceiroQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<RelatorioFinanceiroDto>, ITenantRequest;

public class GetRelatorioFinanceiroHandler : IRequestHandler<GetRelatorioFinanceiroQuery, RelatorioFinanceiroDto>
{
    private readonly ILancamentoFinanceiroRepository _repo;

    public GetRelatorioFinanceiroHandler(ILancamentoFinanceiroRepository repo) => _repo = repo;

    public async Task<RelatorioFinanceiroDto> Handle(GetRelatorioFinanceiroQuery r, CancellationToken ct)
    {
        var lancamentos = r.ObraId.HasValue
            ? await _repo.GetByObraAsync(r.EmpresaId, r.ObraId.Value, ct)
            : await _repo.GetAllByEmpresaAsync(r.EmpresaId, ct);

        var lista = lancamentos.Where(l => !l.IsDeleted).ToList();

        var receitas = lista.Where(l => l.Tipo == TipoLancamentoEnum.Receita);
        var despesas = lista.Where(l => l.Tipo == TipoLancamentoEnum.Despesa);

        var totalReceitas = receitas.Sum(l => l.Valor);
        var totalDespesas = despesas.Sum(l => l.Valor);

        var fluxoMensal = lista
            .GroupBy(l => new { l.DataVencimento.Year, l.DataVencimento.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new FluxoMensalItemDto(
                g.Key.Month, g.Key.Year,
                g.Where(l => l.Tipo == TipoLancamentoEnum.Receita).Sum(l => l.Valor),
                g.Where(l => l.Tipo == TipoLancamentoEnum.Despesa).Sum(l => l.Valor),
                g.Where(l => l.Tipo == TipoLancamentoEnum.Receita).Sum(l => l.Valor) -
                g.Where(l => l.Tipo == TipoLancamentoEnum.Despesa).Sum(l => l.Valor)));

        return new RelatorioFinanceiroDto(
            totalReceitas, totalDespesas, totalReceitas - totalDespesas,
            receitas.Where(l => l.Status == StatusLancamentoEnum.Realizado).Sum(l => l.ValorRealizado ?? l.Valor),
            despesas.Where(l => l.Status == StatusLancamentoEnum.Realizado).Sum(l => l.ValorRealizado ?? l.Valor),
            receitas.Where(l => l.Status == StatusLancamentoEnum.Previsto).Sum(l => l.Valor),
            despesas.Where(l => l.Status == StatusLancamentoEnum.Previsto).Sum(l => l.Valor),
            despesas.Where(l => l.Status != StatusLancamentoEnum.Realizado && l.DataVencimento < DateTime.Today).Sum(l => l.Valor),
            fluxoMensal);
    }
}
