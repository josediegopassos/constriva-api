using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Clientes.Commands;
using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Application.Features.Clientes.Queries;
using Constriva.Application.Features.Obras.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/clientes")]
[Produces("application/json")]
public class ClientesController : BaseController
{
    public ClientesController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ClienteResumoDto>>> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] StatusClienteEnum? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetClientesQuery(RequireEmpresaId(), search, status, page, pageSize), ct));

    [HttpGet("ativos")]
    public async Task<ActionResult<IEnumerable<ClienteAtivoDto>>> GetAtivos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetClientesAtivosQuery(RequireEmpresaId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClienteDto>> GetById(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetClienteByIdQuery(id, RequireEmpresaId()), ct));

    [HttpPost]
    public async Task<ActionResult<ClienteResumoDto>> Create([FromBody] CreateClienteDto dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new CreateClienteCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClienteDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new UpdateClienteCommand(id, RequireEmpresaId(), CurrentUser.UserId, dto), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteClienteCommand(id, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusClienteDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusClienteCommand(id, RequireEmpresaId(), CurrentUser.UserId, dto.Status), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("{id:guid}/obras")]
    public async Task<ActionResult<IEnumerable<ObraResumoDto>>> GetObras(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetObrasDoClienteQuery(id, RequireEmpresaId()), ct));
}
