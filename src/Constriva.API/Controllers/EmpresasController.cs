using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Empresas;
using Constriva.Domain.Enums;
using Constriva.Application.Features.Empresas.DTOs;
using Constriva.Application.Features.Empresas.Commands;

namespace Constriva.API.Controllers;

[Authorize]
public sealed class EmpresasController : BaseController
{
    public EmpresasController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<EmpresaDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null, [FromQuery] StatusEmpresaEnum? status = null,
        CancellationToken ct = default)
    {
        if (!CurrentUser.IsSuperAdmin) return Forbid();
        return Ok(await Mediator.Send(new GetEmpresasQuery(page, pageSize, search, status), ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmpresaDto>> GetById(Guid id, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin && CurrentUser.EmpresaId != id) return Forbid();
        return OkOrNotFound(await Mediator.Send(new GetEmpresaByIdQuery(id), ct));
    }

    [HttpPost]
    public async Task<ActionResult<EmpresaDto>> Create([FromBody] CreateEmpresaDto dto, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin) return Forbid();
        try
        {
            var result = await Mediator.Send(new CreateEmpresaCommand(dto), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmpresaDto dto, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin && CurrentUser.EmpresaId != id) return Forbid();
        try { await Mediator.Send(new UpdateEmpresaCommand(id, dto), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/modulos")]
    public async Task<IActionResult> UpdateModulos(Guid id, [FromBody] UpdateModulosDto dto, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin) return Forbid();
        await Mediator.Send(new UpdateModulosCommand(id, dto), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/ativo")]
    public async Task<IActionResult> AtivarDesativar(Guid id, [FromBody] bool ativo, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin) return Forbid();
        await Mediator.Send(new AtivarDesativarEmpresaCommand(id, ativo), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/plano")]
    public async Task<IActionResult> AlterarPlano(Guid id, [FromBody] AlterarPlanoCommand cmd, CancellationToken ct)
    {
        if (!CurrentUser.IsSuperAdmin) return Forbid();
        await Mediator.Send(cmd, ct);
        return NoContent();
    }
}
