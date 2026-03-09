using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Relatorios;
using Constriva.Application.Features.Relatorios.DTOs;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/relatorios")]
public sealed class RelatoriosController : BaseController
{
    public RelatoriosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardEmpresaDto>> Dashboard(
        [FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetDashboardEmpresaQuery(RequireEmpresaId(), obraId), ct));

    [HttpGet("dashboard-executivo")]
    public async Task<ActionResult<DashboardEmpresaDto>> DashboardExecutivo(CancellationToken ct)
        => Ok(await Mediator.Send(new GetDashboardEmpresaQuery(RequireEmpresaId()), ct));

    [HttpGet("performance-obras")]
    public async Task<ActionResult<IEnumerable<PerformanceObraDto>>> PerformanceObras(CancellationToken ct)
        => Ok(await Mediator.Send(new GetPerformanceObrasQuery(RequireEmpresaId()), ct));

    [HttpGet("obras/{obraId:guid}/kpis")]
    public async Task<ActionResult<KPIsObraDto?>> KPIsObra(Guid obraId, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetKPIsObraQuery(obraId, RequireEmpresaId()), ct));

    [HttpGet("financeiro")]
    public async Task<ActionResult<RelatorioFinanceiroDto>> RelatorioFinanceiro(
        [FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetRelatorioFinanceiroQuery(RequireEmpresaId(), obraId), ct));
}
