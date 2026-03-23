using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Contratos;
using Constriva.Application.Features.Contratos.Commands;
using Constriva.Application.Features.Contratos.DTOs;
using Constriva.Application.Features.Contratos.Queries;
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

    [HttpGet("medicoes")]
    public async Task<ActionResult<IEnumerable<MedicaoGeralDto>>> GetTodasMedicoes(CancellationToken ct)
        => Ok(await Mediator.Send(new GetTodasMedicoesQuery(RequireEmpresaId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContratoDetalheDto>> GetById(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetContratoByIdQuery(id, RequireEmpresaId()), ct));

    [HttpPost]
    public async Task<IActionResult> CreateContrato([FromBody] CreateContratoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateContratoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateContrato(Guid id, [FromBody] UpdateContratoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateContratoCommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/ativar")]
    public async Task<IActionResult> Ativar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusContratoCommand(id, RequireEmpresaId(), StatusContratoEnum.Ativo), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/suspender")]
    public async Task<IActionResult> Suspender(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusContratoCommand(id, RequireEmpresaId(), StatusContratoEnum.Suspenso), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/encerrar")]
    public async Task<IActionResult> Encerrar(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusContratoCommand(id, RequireEmpresaId(), StatusContratoEnum.Encerrado), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/rescindir")]
    public async Task<IActionResult> Rescindir(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusContratoCommand(id, RequireEmpresaId(), StatusContratoEnum.Rescindido), ct); return NoContent(); }
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

    [HttpPatch("{id:guid}/medicoes/{medicaoId:guid}/submeter")]
    public async Task<IActionResult> SubmeterMedicao(Guid id, Guid medicaoId, CancellationToken ct)
    {
        try { await Mediator.Send(new SubmeterMedicaoCommand(medicaoId, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/medicoes/{medicaoId:guid}/analisar")]
    public async Task<IActionResult> AnalisarMedicao(Guid id, Guid medicaoId, CancellationToken ct)
    {
        try { await Mediator.Send(new AnalisarMedicaoCommand(medicaoId, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/medicoes/{medicaoId:guid}/aprovar")]
    public async Task<IActionResult> AprovarMedicao(Guid id, Guid medicaoId, CancellationToken ct)
    {
        try { await Mediator.Send(new AprovarMedicaoCommand(medicaoId, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/medicoes/{medicaoId:guid}/pagar")]
    public async Task<IActionResult> PagarMedicao(Guid id, Guid medicaoId, CancellationToken ct)
    {
        try { await Mediator.Send(new PagarMedicaoCommand(medicaoId, RequireEmpresaId(), CurrentUser.UserId), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:guid}/medicoes/{medicaoId:guid}/rejeitar")]
    public async Task<IActionResult> RejeitarMedicao(Guid id, Guid medicaoId, [FromBody] RejeitarMedicaoRequest body, CancellationToken ct)
    {
        try { await Mediator.Send(new RejeitarMedicaoCommand(medicaoId, RequireEmpresaId(), CurrentUser.UserId, body.Motivo), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    public record RejeitarMedicaoRequest(string Motivo);
}
