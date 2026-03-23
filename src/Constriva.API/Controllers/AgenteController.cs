using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Agente.Commands;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Queries;

namespace Constriva.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/agente")]
[Produces("application/json")]
public class AgenteController : BaseController
{
    public AgenteController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequestDto dto, CancellationToken ct)
    {
        try 
        { 
            return Ok(await Mediator.Send(new ChatCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); 
        }
        catch (Exception ex) 
        { 
            return HandleException(ex); 
        }
    }

    [HttpGet("sessoes")]
    public async Task<ActionResult<IEnumerable<SessaoResumoDto>>> GetSessoes(CancellationToken ct)
        => Ok(await Mediator.Send(new GetSessoesQuery(RequireEmpresaId(), CurrentUser.UserId), ct));

    [HttpGet("sessao/{id:guid}")]
    public async Task<ActionResult<SessaoDetalheDto>> GetSessao(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetSessaoByIdQuery(id, RequireEmpresaId()), ct));

    [HttpDelete("sessao/{id:guid}")]
    public async Task<IActionResult> DeleteSessao(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteSessaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("consumo")]
    public async Task<ActionResult<DashboardConsumoDto>> GetConsumo(CancellationToken ct)
        => Ok(await Mediator.Send(new GetDashboardConsumoQuery(RequireEmpresaId()), ct));

    [HttpGet("consumo/historico-diario")]
    public async Task<ActionResult<IEnumerable<ConsumoDiarioDto>>> GetConsumoDiario(CancellationToken ct)
        => Ok(await Mediator.Send(new GetConsumoDiarioQuery(RequireEmpresaId()), ct));

    [HttpGet("consumo/usuarios")]
    public async Task<ActionResult<IEnumerable<ConsumoUsuarioDto>>> GetConsumoUsuarios(CancellationToken ct)
        => Ok(await Mediator.Send(new GetConsumoUsuariosQuery(RequireEmpresaId()), ct));

    [HttpGet("notificacoes")]
    public async Task<ActionResult<IEnumerable<NotificacaoDto>>> GetNotificacoes(
        [FromQuery] bool? lida, CancellationToken ct)
        => Ok(await Mediator.Send(new GetNotificacoesQuery(RequireEmpresaId(), lida), ct));

    [HttpPatch("notificacoes/{id:guid}/lida")]
    public async Task<IActionResult> MarcarNotificacaoLida(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new MarcarNotificacaoLidaCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
