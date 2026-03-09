using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Usuarios;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Application.Features.Usuarios.Commands;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/usuarios")]
public sealed class UsuariosController : BaseController
{
    public UsuariosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    private bool PodeGerenciarUsuarios =>
        CurrentUser.IsSuperAdmin || CurrentUser.IsAdminEmpresa;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<UsuarioDto>>> GetAll(
        [FromQuery] Guid? empresaId, [FromQuery] string? search,
        [FromQuery] bool? ativo, [FromQuery] int page = 1,
        CancellationToken ct = default)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        return Ok(await Mediator.Send(
            new GetUsuariosQuery(CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin, empresaId, search, ativo, page),
            ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioDetalheDto?>> GetById(Guid id, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        return OkOrNotFound(await Mediator.Send(
            new GetUsuarioByIdQuery(id, CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin),
            ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        try
        {
            var result = await Mediator.Send(
                new CreateUsuarioCommand(CurrentUser.IsSuperAdmin, CurrentUser.EmpresaId, dto),
                ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUsuarioDto dto, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        try
        {
            await Mediator.Send(
                new UpdateUsuarioCommand(id, CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin, dto),
                ct);
            return NoContent();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/ativo")]
    public async Task<IActionResult> AtivarDesativar(Guid id, [FromBody] bool ativo, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        await Mediator.Send(
            new AtivarDesativarUsuarioCommand(id, CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin, ativo),
            ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/senha")]
    public async Task<IActionResult> ResetSenha(Guid id, [FromBody] ResetSenhaDto dto, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        try
        {
            await Mediator.Send(
                new ResetSenhaCommand(id, CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin, dto.NovaSenha),
                ct);
            return NoContent();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}/permissoes")]
    public async Task<IActionResult> UpdatePermissoes(
        Guid id, [FromBody] List<UpdatePermissaoDto> dtos, CancellationToken ct)
    {
        if (!PodeGerenciarUsuarios) return Forbid();
        try
        {
            await Mediator.Send(
                new UpdatePermissoesCommand(id, CurrentUser.EmpresaId, CurrentUser.IsSuperAdmin, dtos),
                ct);
            return NoContent();
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
