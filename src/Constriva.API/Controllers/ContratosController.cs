using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Contratos;
using Constriva.Application.Features.Contratos.Commands;
using Constriva.Application.Features.Contratos.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/contratos")]
public sealed class ContratosController : BaseController
{
    public ContratosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ContratoDto>>> GetContratos(
        [FromQuery] Guid? obraId, [FromQuery] StatusContratoEnum? status,
        [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetContratosQuery(RequireEmpresaId(), obraId, status, page), ct));

    [HttpPost]
    public async Task<IActionResult> CreateContrato([FromBody] CreateContratoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateContratoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateContrato(Guid id, [FromBody] UpdateContratoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateContratoCommand(id, RequireEmpresaId(), dto.Numero, dto.Objeto, dto.ValorTotal, dto.DataInicio, dto.DataFim, dto.Status.ToString(), dto.Observacoes), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteContrato(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteContratoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("{id:guid}/medicoes")]
    public async Task<ActionResult<IEnumerable<MedicaoContratoDto>>> GetMedicoes(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetMedicoesContratoQuery(id, RequireEmpresaId()), ct));

    [HttpPost("{id:guid}/medicoes")]
    public async Task<IActionResult> CreateMedicao(Guid id, [FromBody] CreateMedicaoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateMedicaoCommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}/medicoes/{medicaoId:guid}")]
    public async Task<IActionResult> UpdateMedicao(Guid id, Guid medicaoId, [FromBody] UpdateMedicaoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateMedicaoCommand(medicaoId, id, RequireEmpresaId(), dto.Numero, dto.DataMedicao, dto.ValorMedido, dto.Observacoes), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("{id:guid}/medicoes/{medicaoId:guid}")]
    public async Task<IActionResult> DeleteMedicao(Guid id, Guid medicaoId, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteMedicaoCommand(medicaoId, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
