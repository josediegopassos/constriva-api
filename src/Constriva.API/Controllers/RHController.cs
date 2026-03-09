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

    [HttpPost("funcionarios")]
    public async Task<IActionResult> CreateFuncionario([FromBody] CreateFuncionarioDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreateFuncionarioCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("funcionarios/{id:guid}")]
    public async Task<IActionResult> UpdateFuncionario(Guid id, [FromBody] UpdateFuncionarioDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateFuncionarioCommand(id, RequireEmpresaId(), dto.Nome, dto.Cargo, dto.Departamento, dto.SalarioBase, dto.Telefone, dto.Email, dto.Ativo, dto.ObraId), ct)); }
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

    [HttpGet("cargos")]
    public async Task<ActionResult<IEnumerable<CargoDto>>> GetCargos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetCargosQuery(RequireEmpresaId()), ct));

    [HttpGet("departamentos")]
    public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetDepartamentos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetDepartamentosQuery(RequireEmpresaId()), ct));
}
