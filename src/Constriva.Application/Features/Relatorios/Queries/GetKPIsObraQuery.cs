using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Relatorios.DTOs;

namespace Constriva.Application.Features.Relatorios;

public record GetKPIsObraQuery(Guid ObraId, Guid EmpresaId)
    : IRequest<KPIsObraDto?>, ITenantRequest;

public class GetKPIsObraHandler : IRequestHandler<GetKPIsObraQuery, KPIsObraDto?>
{
    private readonly IObraRepository _obraRepo;
    private readonly IRelatoriosRepository _relRepo;
    private readonly ILancamentoFinanceiroRepository _finRepo;

    public GetKPIsObraHandler(IObraRepository obraRepo, IRelatoriosRepository relRepo,
        ILancamentoFinanceiroRepository finRepo)
    {
        _obraRepo = obraRepo; _relRepo = relRepo; _finRepo = finRepo;
    }

    public async Task<KPIsObraDto?> Handle(GetKPIsObraQuery r, CancellationToken ct)
    {
        var obra = await _obraRepo.GetByIdAndEmpresaAsync(r.ObraId, r.EmpresaId, ct);
        if (obra == null) return null;

        var atividades = await _relRepo.GetAtividadesParaKPIAsync(r.EmpresaId, r.ObraId, ct);
        var atividadesList = atividades.ToList();

        var lancamentos = await _finRepo.GetByObraAsync(r.EmpresaId, r.ObraId, ct);
        var despesas = lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Despesa && l.Status == StatusLancamentoEnum.Realizado);
        var custoReal = despesas.Sum(l => l.ValorRealizado ?? l.Valor);

        var hoje = DateTime.Today;
        var duracaoTotal = Math.Max((obra.DataFimPrevista - obra.DataInicioPrevista).TotalDays, 1);
        var decorrido = Math.Min((hoje - obra.DataInicioPrevista).TotalDays, duracaoTotal);
        var percPlanejado = (decimal)(decorrido / duracaoTotal) * 100m;
        var percRealizado = obra.PercentualConcluido;

        var vp = obra.ValorContrato * percPlanejado / 100m;
        var va = obra.ValorContrato * percRealizado / 100m;
        var idp = vp > 0 ? va / vp : 1m;
        var idc = custoReal > 0 ? va / custoReal : 1m;
        var variacao = va - custoReal;

        return new KPIsObraDto(
            obra.Id, obra.Nome,
            Math.Round(percPlanejado, 1), Math.Round(percRealizado, 1),
            atividadesList.Count,
            atividadesList.Count(a => a.PercentualConcluido >= 100),
            atividadesList.Count(a => a.DataFimPlanejada < hoje && a.PercentualConcluido < 100),
            obra.ValorContrato, obra.ValorContrato * percRealizado / 100m,
            obra.ValorContrato - (obra.ValorContrato * percRealizado / 100m),
            custoReal, Math.Round(variacao, 2),
            Math.Round(vp, 2), Math.Round(va, 2), Math.Round(custoReal, 2),
            Math.Round(variacao, 2), Math.Round(idp, 2), Math.Round(idc, 2));
    }
}
