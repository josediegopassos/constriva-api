using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.SST;
using Constriva.Application.Features.SST.Commands;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/sst")]
public sealed class SSTController : BaseController
{
    public SSTController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("dds")]
    public async Task<ActionResult<PaginatedResult<DDSDto>>> GetDDS(
        [FromQuery] Guid? obraId, [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetDDSQuery(RequireEmpresaId(), obraId, page), ct));

    [HttpPost("dds")]
    public async Task<IActionResult> CreateDDS([FromBody] CreateDDSDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateDDSCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("dds/{id:guid}")]
    public async Task<IActionResult> UpdateDDS(Guid id, [FromBody] UpdateDDSDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateDDSCommand(id, RequireEmpresaId(), dto.Tema, dto.Local, dto.DataRealizacao, dto.QuantidadeParticipantes, dto.Observacoes), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("dds/{id:guid}")]
    public async Task<IActionResult> DeleteDDS(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteDDSCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("acidentes")]
    public async Task<ActionResult<IEnumerable<AcidenteDto>>> GetAcidentes(
        [FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetAcidentesQuery(RequireEmpresaId(), obraId), ct));

    [HttpPost("acidentes")]
    public async Task<IActionResult> RegistrarAcidente([FromBody] CreateAcidenteDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateAcidenteCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("acidentes/{id:guid}")]
    public async Task<IActionResult> UpdateAcidente(Guid id, [FromBody] UpdateAcidenteDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateAcidenteCommand(id, RequireEmpresaId(), dto.Descricao, dto.DataAcidente, dto.AfastamentoMedico, dto.DiasAfastamento, dto.CausaRaiz, dto.AcoesCorretivas), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("acidentes/{id:guid}")]
    public async Task<IActionResult> DeleteAcidente(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteAcidenteCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("indicadores")]
    public async Task<ActionResult<IndicadoresDashboardDto>> GetIndicadores(
        [FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetIndicadoresSSTQuery(RequireEmpresaId(), obraId), ct));

    [HttpGet("epis")]
    public async Task<ActionResult<IEnumerable<EPIDto>>> GetEPIs(CancellationToken ct)
        => Ok(await Mediator.Send(new GetEPIsQuery(RequireEmpresaId()), ct));

    [HttpPost("epis")]
    public async Task<IActionResult> CreateEPI([FromBody] CreateEPIDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateEPICommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("epis/{id:guid}")]
    public async Task<IActionResult> UpdateEPI(Guid id, [FromBody] UpdateEPIDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateEPICommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("epis/{id:guid}")]
    public async Task<IActionResult> DeleteEPI(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteEPICommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
