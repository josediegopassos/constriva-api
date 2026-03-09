using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Obras.Commands;
using Constriva.Application.Features.Obras.Queries;
using Constriva.Domain.Enums;
using Constriva.Application.Features.Obras.DTOs;
using Constriva.Application.Features.Cronograma;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/obras")]
[Produces("application/json")]
public class ObrasController : BaseController
{
    public ObrasController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ObraResumoDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null, [FromQuery] StatusObraEnum? status = null,
        [FromQuery] TipoObraEnum? tipo = null, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetObrasQuery(RequireEmpresaId(), page, pageSize, search, status, tipo), ct));

    [HttpGet("dashboard")]
    public async Task<ActionResult<ObrasDashboardDto>> Dashboard(CancellationToken ct)
        => Ok(await Mediator.Send(new GetObrasDashboardQuery(RequireEmpresaId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObraDto>> GetById(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetObraByIdQuery(id, RequireEmpresaId()), ct));

    [HttpPost]
    public async Task<ActionResult<ObraResumoDto>> Create([FromBody] CreateObraDto dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new CreateObraCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateObraDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new UpdateObraCommand(id, RequireEmpresaId(), CurrentUser.UserId, dto), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusObraRequest req, CancellationToken ct)
    {
        try { await Mediator.Send(new UpdateStatusObraCommand(id, RequireEmpresaId(), CurrentUser.UserId, req.Status, req.Observacao), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/percentual")]
    public async Task<IActionResult> UpdatePercentual(Guid id, [FromBody] decimal percentual, CancellationToken ct)
    {
        try { await Mediator.Send(new UpdatePercentualObraCommand(id, RequireEmpresaId(), percentual), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteObraCommand(id, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("{id:guid}/curva-s")]
    public async Task<ActionResult<IEnumerable<CurvaSPontoDto>>> GetCurvaS(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetCurvaSQuery(id, RequireEmpresaId()), ct));

    [HttpGet("{id:guid}/evm")]
    public async Task<ActionResult<EVMDto>> GetEVM(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetEVMQuery(id, RequireEmpresaId()), ct));

    [HttpGet("{id:guid}/cronograma")]
    public async Task<ActionResult<CronogramaObraDto?>> GetCronograma(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetCronogramaQuery(id, RequireEmpresaId()), ct));

    [HttpPost("{id:guid}/fases")]
    public async Task<IActionResult> CreateFase(Guid id, [FromBody] CreateFaseObraRequest req, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateFaseCommand(id, RequireEmpresaId(), req.Nome, req.Ordem, req.Inicio, req.Fim, req.ValorPrevisto, req.FasePaiId), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    public record UpdateStatusObraRequest(Domain.Enums.StatusObraEnum Status, string? Observacao);
    public record CreateFaseObraRequest(string Nome, int Ordem, DateTime Inicio, DateTime Fim,
        decimal ValorPrevisto, Guid? FasePaiId = null);
}
