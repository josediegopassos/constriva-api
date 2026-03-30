using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.RH;
using Constriva.Application.Features.RH.Commands;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/rh")]
public sealed class RHController : BaseController
{
    public RHController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("funcionarios")]
    public async Task<ActionResult<PaginatedResult<FuncionarioDto>>> GetFuncionarios(
        [FromQuery] string? search, [FromQuery] Guid? obraId,
        [FromQuery] StatusFuncionarioEnum? status, [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetFuncionariosQuery(RequireEmpresaId(), search, obraId, status, page), ct));

    [HttpGet("funcionarios/{id:guid}")]
    public async Task<ActionResult<FuncionarioDetalhadoDto?>> GetFuncionarioById(Guid id, CancellationToken ct)
        => OkOrNotFound(await Mediator.Send(new GetFuncionarioByIdQuery(id, RequireEmpresaId()), ct));

    [HttpPost("funcionarios")]
    public async Task<IActionResult> CreateFuncionario([FromBody] CreateFuncionarioDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateFuncionarioCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("funcionarios/{id:guid}")]
    public async Task<IActionResult> UpdateFuncionario(Guid id, [FromBody] UpdateFuncionarioDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateFuncionarioCommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("funcionarios/{id:guid}/status")]
    public async Task<IActionResult> AlterarStatusFuncionario(Guid id, [FromBody] AlterarStatusFuncionarioDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusFuncionarioCommand(id, RequireEmpresaId(), dto.Status, dto.Motivo), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("funcionarios/{id:guid}")]
    public async Task<IActionResult> DeleteFuncionario(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteFuncionarioCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("ponto")]
    public async Task<ActionResult<IEnumerable<RegistroPontoDto>>> GetPonto(
        [FromQuery] Guid? funcionarioId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim,
        CancellationToken ct)
        => Ok(await Mediator.Send(new GetPontosQuery(RequireEmpresaId(), funcionarioId, inicio, fim), ct));

    [HttpPost("ponto")]
    public async Task<IActionResult> RegistrarPonto([FromBody] RegistrarPontoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new RegistrarPontoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("pontos/{id:guid}")]
    public async Task<IActionResult> UpdatePonto(Guid id, [FromBody] UpdatePontoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdatePontoCommand(id, RequireEmpresaId(), dto.Entrada, dto.Saida, dto.Observacoes), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("pontos/{id:guid}")]
    public async Task<IActionResult> DeletePonto(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeletePontoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("folhas")]
    public async Task<ActionResult<IEnumerable<FolhaPagamentoDto>>> GetFolha(
        [FromQuery] string? competencia, CancellationToken ct)
        => Ok(await Mediator.Send(new GetFolhasQuery(RequireEmpresaId(), competencia), ct));

    [HttpPost("folhas/gerar")]
    public async Task<IActionResult> GerarFolha([FromBody] GerarFolhaDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new GerarFolhaPagamentoCommand(RequireEmpresaId(), dto.Competencia, dto.FuncionarioId), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("funcionarios/{id:guid}/folhas")]
    public async Task<ActionResult<IEnumerable<FolhaFuncionarioDto>>> GetFolhasFuncionario(Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetFolhasFuncionarioQuery(id, RequireEmpresaId()), ct));

    [HttpGet("funcionarios/ativos")]
    public async Task<ActionResult<IEnumerable<FuncionarioResumoDto>>> GetFuncionariosAtivos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetFuncionariosAtivosQuery(RequireEmpresaId()), ct));

    [HttpGet("cargos")]
    public async Task<ActionResult<IEnumerable<CargoDto>>> GetCargos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetCargosQuery(RequireEmpresaId()), ct));

    [HttpPost("cargos")]
    public async Task<IActionResult> CreateCargo([FromBody] CreateCargoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateCargoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("cargos/{id:guid}")]
    public async Task<IActionResult> UpdateCargo(Guid id, [FromBody] UpdateCargoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateCargoCommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("cargos/{id:guid}/ativo")]
    public async Task<IActionResult> ToggleAtivoCargo(Guid id, [FromBody] ToggleAtivoDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new ToggleAtivoCargoCommand(id, RequireEmpresaId(), dto.Ativo), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("cargos/{id:guid}")]
    public async Task<IActionResult> DeleteCargo(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteCargoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("departamentos")]
    public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetDepartamentos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetDepartamentosQuery(RequireEmpresaId()), ct));

    [HttpPost("departamentos")]
    public async Task<IActionResult> CreateDepartamento([FromBody] CreateDepartamentoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateDepartamentoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("departamentos/{id:guid}")]
    public async Task<IActionResult> UpdateDepartamento(Guid id, [FromBody] UpdateDepartamentoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateDepartamentoCommand(id, RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("departamentos/{id:guid}")]
    public async Task<IActionResult> DeleteDepartamento(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteDepartamentoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("departamentos/{id:guid}/ativo")]
    public async Task<IActionResult> ToggleAtivoDepartamento(Guid id, [FromBody] ToggleAtivoDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new ToggleAtivoDepartamentoCommand(id, RequireEmpresaId(), dto.Ativo), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
