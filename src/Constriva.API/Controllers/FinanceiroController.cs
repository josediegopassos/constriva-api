using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Financeiro.Commands;
using Constriva.Application.Features.Financeiro.DTOs;
using Constriva.Application.Features.Financeiro.Queries;
using Constriva.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers
{
    [Authorize]
    [Route("api/v1/financeiro")]
    public sealed class FinanceiroController : BaseController
    {
        public FinanceiroController(IMediator mediator, ICurrentUser currentUser)
            : base(mediator, currentUser) { }

        [HttpGet("lancamentos")]
        public async Task<ActionResult<PaginatedResult<LancamentoFinanceiroDto>>> GetLancamentos(
            [FromQuery] Guid? obraId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim,
            [FromQuery] TipoLancamentoEnum? tipo, [FromQuery] StatusLancamentoEnum? status,
            [FromQuery] int page = 1, CancellationToken ct = default)
            => Ok(await Mediator.Send(new GetLancamentosQuery(RequireEmpresaId(), obraId, inicio, fim, tipo, status, page), ct));

        [HttpPost("lancamentos")]
        public async Task<IActionResult> CreateLancamento([FromBody] CreateLancamentoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateLancamentoCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("lancamentos/{id:guid}/baixar")]
        public async Task<IActionResult> BaixarLancamento(
            Guid id, [FromBody] BaixarLancamentoRequest req, CancellationToken ct)
        {
            try
            {
                await Mediator.Send(new BaixarLancamentoCommand(id, RequireEmpresaId(), req.ValorRealizado, req.DataPagamento), ct);
                return NoContent();
            }
            catch (Exception ex) { return HandleException(ex); }
        }


        [HttpPut("lancamentos/{id:guid}")]
        public async Task<IActionResult> UpdateLancamento(Guid id, [FromBody] UpdateLancamentoDto dto, CancellationToken ct)
        {
            try
            {
                await Mediator.Send(new UpdateLancamentoCommand(id, RequireEmpresaId(), dto), ct);
                return NoContent();
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("lancamentos/{id:guid}")]
        public async Task<IActionResult> DeleteLancamento(Guid id, CancellationToken ct)
        {
            await Mediator.Send(new DeleteLancamentoCommand(id, RequireEmpresaId()), ct);
            return NoContent();
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardFinanceiroDto>> Dashboard(
            [FromQuery] Guid? obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetDashboardFinanceiroQuery(RequireEmpresaId(), obraId), ct));

        [HttpGet("fluxo-caixa")]
        public async Task<ActionResult<IEnumerable<FluxoCaixaItemDto>>> GetFluxoCaixa(
            [FromQuery] Guid? obraId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim,
            CancellationToken ct)
            => Ok(await Mediator.Send(new GetFluxoCaixaQuery(RequireEmpresaId(), obraId, inicio, fim), ct));

        [HttpGet("dre")]
        public async Task<ActionResult<DREDto>> GetDRE(
            [FromQuery] Guid? obraId, [FromQuery] string? competencia, CancellationToken ct)
            => Ok(await Mediator.Send(new GetDREQuery(RequireEmpresaId(), obraId, competencia), ct));
    }

    public record BaixarLancamentoRequest(decimal ValorRealizado, DateTime DataPagamento);
}