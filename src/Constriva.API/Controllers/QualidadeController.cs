using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Qualidade;
using Constriva.Application.Features.Qualidade.Commands;
using Constriva.Application.Features.Qualidade.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/qualidade")]
public sealed class QualidadeController : BaseController
{
    public QualidadeController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("inspecoes")]
    public async Task<ActionResult<PaginatedResult<InspecaoDto>>> GetInspecoes(
        [FromQuery] Guid? obraId, [FromQuery] StatusInspecaoEnum? status,
        [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetInspecoesQuery(RequireEmpresaId(), obraId, status, page), ct));

    [HttpPost("inspecoes")]
    public async Task<IActionResult> CreateInspecao([FromBody] CreateInspecaoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateInspecaoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("inspecoes/{id:guid}")]
    public async Task<IActionResult> UpdateInspecao(Guid id, [FromBody] UpdateInspecaoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateInspecaoCommand(id, RequireEmpresaId(), dto.NumeroInspecao, dto.DataProgramada, dto.DataRealizada, dto.Responsavel, dto.Observacoes, dto.Status.ToString()), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("inspecoes/{id:guid}")]
    public async Task<IActionResult> DeleteInspecao(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteInspecaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("ncs")]
    public async Task<ActionResult<PaginatedResult<NaoConformidadeDto>>> GetNCs(
        [FromQuery] Guid? obraId, [FromQuery] StatusNaoConformidadeEnum? status,
        [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetNCsQuery(RequireEmpresaId(), obraId, status, page), ct));

    [HttpPost("ncs")]
    public async Task<IActionResult> CreateNC([FromBody] CreateNCDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateNCCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("ncs/{id:guid}")]
    public async Task<IActionResult> UpdateNC(Guid id, [FromBody] UpdateNCDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateNCCommand(id, RequireEmpresaId(), dto.Descricao, dto.Causa, dto.AcaoCorretiva, dto.DataPrazo, dto.Status.ToString()), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("ncs/{id:guid}")]
    public async Task<IActionResult> DeleteNC(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteNCCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("fvs")]
    public async Task<ActionResult<IEnumerable<FVSDto>>> GetFVS([FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetFVSQuery(RequireEmpresaId(), obraId), ct));

    [HttpPut("fvs/{id:guid}")]
    public async Task<IActionResult> UpdateFVS(Guid id, [FromBody] UpdateFVSDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateFVSCommand(id, RequireEmpresaId(), dto.Servico, dto.Responsavel?.ToString(), dto.DataVerificacao, dto.Aprovado, dto.Observacoes), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("fvs/{id:guid}")]
    public async Task<IActionResult> DeleteFVS(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteFVSCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
