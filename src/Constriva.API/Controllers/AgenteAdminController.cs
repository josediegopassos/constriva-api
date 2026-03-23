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
[Route("api/v1/agente/admin")]
[Produces("application/json")]
public class AgenteAdminController : BaseController
{
    public AgenteAdminController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("relatorio-mensal")]
    public async Task<ActionResult<IEnumerable<AdminRelatorioItemDto>>> GetRelatorioMensal(
        [FromQuery] int ano, [FromQuery] int mes, CancellationToken ct)
        => Ok(await Mediator.Send(new GetAdminRelatorioMensalQuery(ano, mes), ct));

    [HttpPost("cota-avulsa")]
    public async Task<IActionResult> CriarCotaAvulsa([FromBody] CriarCotaAvulsaDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new CriarCotaAvulsaCommand(dto.EmpresaId, CurrentUser.UserId, dto), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("empresas")]
    public async Task<ActionResult<IEnumerable<AdminEmpresaAgenteDto>>> GetEmpresas(CancellationToken ct)
        => Ok(await Mediator.Send(new GetAdminEmpresasAgenteQuery(), ct));

    [HttpPut("empresas/{empresaId:guid}/tier")]
    public async Task<IActionResult> AlterarTier(Guid empresaId, [FromBody] AlterarTierDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarTierEmpresaCommand(empresaId, dto.TierId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
