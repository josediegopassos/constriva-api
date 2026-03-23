using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Fornecedores.Commands;
using Constriva.Application.Features.Fornecedores.DTOs;
using Constriva.Application.Features.Fornecedores.Queries;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/fornecedores")]
[Produces("application/json")]
public class FornecedoresController : BaseController
{
    public FornecedoresController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<FornecedorResumoDto>>> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] TipoFornecedorEnum? tipo = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetFornecedoresQuery(RequireEmpresaId(), search, tipo, page, pageSize), ct));

    [HttpGet("ativos")]
    public async Task<ActionResult<IEnumerable<FornecedorAtivoDto>>> GetAtivos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetFornecedoresAtivosQuery(RequireEmpresaId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorDto>> GetById(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetFornecedorByIdQuery(id, RequireEmpresaId()), ct));

    [HttpPost]
    public async Task<ActionResult<FornecedorResumoDto>> Create([FromBody] CreateFornecedorDto dto, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new CreateFornecedorCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFornecedorDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new UpdateFornecedorCommand(id, RequireEmpresaId(), CurrentUser.UserId, dto), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/ativar")]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new AtivarFornecedorCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/desativar")]
    public async Task<IActionResult> Desativar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DesativarFornecedorCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/homologar")]
    public async Task<IActionResult> Homologar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new HomologarFornecedorCommand(id, RequireEmpresaId(), true), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/deshomologar")]
    public async Task<IActionResult> Deshomologar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new HomologarFornecedorCommand(id, RequireEmpresaId(), false), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteFornecedorCommand(id, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
