using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Relatorios.DTOs;

namespace Constriva.Application.Features.Relatorios;

public record GetDashboardEmpresaQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<DashboardEmpresaDto>, ITenantRequest;

public class GetDashboardEmpresaHandler : IRequestHandler<GetDashboardEmpresaQuery, DashboardEmpresaDto>
{
    private readonly IObraRepository _obraRepo;
    private readonly ILancamentoFinanceiroRepository _finRepo;
    private readonly IRHRepository _rhRepo;
    private readonly ISSTRepository _sstRepo;

    public GetDashboardEmpresaHandler(
        IObraRepository obraRepo, ILancamentoFinanceiroRepository finRepo,
        IRHRepository rhRepo, ISSTRepository sstRepo)
    {
        _obraRepo = obraRepo; _finRepo = finRepo;
        _rhRepo = rhRepo; _sstRepo = sstRepo;
    }

    public async Task<DashboardEmpresaDto> Handle(GetDashboardEmpresaQuery r, CancellationToken ct)
    {
        var obras = await _obraRepo.GetAllByEmpresaAsync(r.EmpresaId, ct);
        var obrasList = obras.Where(o => !o.IsDeleted).ToList();

        var hoje = DateTime.Today;
        var lancamentos = await _finRepo.GetByPeriodoAsync(
            r.EmpresaId, new DateTime(hoje.Year, hoje.Month, 1), hoje, ct);
        var lancList = lancamentos.ToList();

        var (funcionarios, _) = await _rhRepo.GetFuncionariosPagedAsync(r.EmpresaId, null, null, null, 1, 1, ct);
        var acidentesMes = await _sstRepo.GetAcidentesAsync(r.EmpresaId, null, ct);

        return new DashboardEmpresaDto(
            obrasList.Count,
            obrasList.Count(o => o.Status == StatusObraEnum.EmAndamento),
            obrasList.Count(o => o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje),
            obrasList.Count(o => o.Status == StatusObraEnum.Concluida),
            obrasList.Sum(o => o.ValorContrato),
            lancList.Where(l => l.Tipo == TipoLancamentoEnum.Receita && l.Status == StatusLancamentoEnum.Realizado)
                .Sum(l => l.ValorRealizado ?? l.Valor),
            lancList.Where(l => l.Tipo == TipoLancamentoEnum.Despesa && l.Status == StatusLancamentoEnum.Realizado)
                .Sum(l => l.ValorRealizado ?? l.Valor),
            lancList.Where(l => l.Status == StatusLancamentoEnum.Realizado)
                .Sum(l => l.Tipo == TipoLancamentoEnum.Receita ? (l.ValorRealizado ?? l.Valor) : -(l.ValorRealizado ?? l.Valor)),
            funcionarios.Count(),
            acidentesMes.Count(a => a.DataHoraAcidente >= new DateTime(hoje.Year, hoje.Month, 1)));
    }
}
