using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Estoque;
using Constriva.Application.Features.Estoque.Commands;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Application.Features.Estoque.Queries;
using Constriva.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers
{   
    [Authorize]
    [Route("api/v1/estoque")]
    public sealed class EstoqueController : BaseController
    {
        public EstoqueController(IMediator mediator, ICurrentUser currentUser)
            : base(mediator, currentUser) { }

        [HttpGet("materiais")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetMateriais(
            [FromQuery] string? search, CancellationToken ct)
            => Ok(await Mediator.Send(new GetMateriaisQuery(RequireEmpresaId(), search), ct));

        [HttpGet("materiais/{id:guid}")]
        public async Task<ActionResult<MaterialDetalheDto>> GetMaterialById(Guid id, CancellationToken ct)
            => OkOrNotFound(await Mediator.Send(new GetMaterialByIdQuery(id, RequireEmpresaId()), ct));

        [HttpGet("saldos")]
        public async Task<ActionResult<IEnumerable<SaldoEstoqueDto>>> GetSaldos(
            [FromQuery] Guid? almoxarifadoId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetSaldosEstoqueQuery(RequireEmpresaId(), almoxarifadoId), ct));

        [HttpGet("movimentacoes")]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueDto>>> GetMovimentacoes(
            [FromQuery] Guid? almoxarifadoId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim,
            CancellationToken ct)
            => Ok(await Mediator.Send(new GetMovimentacoesEstoqueQuery(RequireEmpresaId(), almoxarifadoId, inicio, fim), ct));

        [HttpGet("grupos")]
        public async Task<ActionResult<IEnumerable<GrupoMaterialDto>>> GetGrupos(CancellationToken ct)
            => Ok(await Mediator.Send(new GetGruposQuery(RequireEmpresaId()), ct));

        [HttpGet("requisicoes/{id:guid}")]
        public async Task<ActionResult<RequisicaoDetalheDto>> GetRequisicaoById(Guid id, CancellationToken ct)
            => OkOrNotFound(await Mediator.Send(new GetRequisicaoByIdQuery(id, RequireEmpresaId()), ct));

        [HttpGet("requisicoes/export")]
        public async Task<IActionResult> ExportRequisicoes(
            [FromQuery] Guid? obraId, [FromQuery] StatusRequisicaoEnum? status, CancellationToken ct)
        {
            var bytes = await Mediator.Send(new ExportRequisicoesQuery(RequireEmpresaId(), obraId, status), ct);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"requisicoes_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }

        [HttpGet("requisicoes")]
        public async Task<ActionResult<PaginatedResult<RequisicaoDto>>> GetRequisicoes(
            [FromQuery] Guid? obraId, [FromQuery] StatusRequisicaoEnum? status,
            [FromQuery] int page = 1, CancellationToken ct = default)
            => Ok(await Mediator.Send(new GetRequisicoesQuery(RequireEmpresaId(), obraId, status, page), ct));

        [HttpPost("materiais")]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateMaterialCommand(RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("materiais/{id:guid}")]
        public async Task<IActionResult> UpdateMaterial(Guid id, [FromBody] UpdateMaterialDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateMaterialCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("materiais/{id:guid}")]
        public async Task<IActionResult> DeleteMaterial(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteMaterialCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("almoxarifados")]
        public async Task<ActionResult<IEnumerable<AlmoxarifadoDto>>> GetAlmoxarifados(CancellationToken ct)
         => Ok(await Mediator.Send(new GetAlmoxarifadosQuery(RequireEmpresaId()), ct));

        [HttpPost("almoxarifados")]
        public async Task<IActionResult> CreateAlmoxarifado([FromBody] CreateAlmoxarifadoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateAlmoxarifadoCommand(RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("almoxarifados/{id:guid}")]
        public async Task<IActionResult> UpdateAlmoxarifado(Guid id, [FromBody] UpdateAlmoxarifadoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateAlmoxarifadoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("almoxarifados/{id:guid}")]
        public async Task<IActionResult> DeleteAlmoxarifado(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteAlmoxarifadoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("movimentacoes/{id:guid}")]
        public async Task<IActionResult> DeleteMovimentacao(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteMovimentacaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("movimentacoes")]
        public async Task<IActionResult> CreateMovimentacao([FromBody] CreateMovimentacaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateMovimentacaoCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("requisicoes")]
        public async Task<IActionResult> CreateRequisicao([FromBody] CreateRequisicaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateRequisicaoCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("requisicoes/{id:guid}")]
        public async Task<IActionResult> UpdateRequisicao(Guid id, [FromBody] UpdateRequisicaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateRequisicaoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("requisicoes/{id:guid}")]
        public async Task<IActionResult> DeleteRequisicao(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteRequisicaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("requisicoes/{id:guid}/itens")]
        public async Task<IActionResult> AddItemRequisicao(Guid id, [FromBody] AddItemRequisicaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new AddItemRequisicaoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("requisicoes/{id:guid}/aprovar")]
        public async Task<IActionResult> AprovarRequisicao(Guid id, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new AprovarRequisicaoCommand(id, RequireEmpresaId(), CurrentUser.UserId), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("requisicoes/{id:guid}/atender")]
        public async Task<IActionResult> AtenderRequisicao(Guid id, [FromBody] IEnumerable<AtenderItemDto> itens, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new AtenderRequisicaoCommand(id, RequireEmpresaId(), CurrentUser.UserId, itens), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("requisicoes/{id:guid}/reprovar")]
        public async Task<IActionResult> ReprovarRequisicao(Guid id, [FromBody] ReprovarRequisicaoRequest body, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new ReprovarRequisicaoCommand(id, RequireEmpresaId(), CurrentUser.UserId, body.Motivo), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("requisicoes/{id:guid}/cancelar")]
        public async Task<IActionResult> CancelarRequisicao(Guid id, [FromBody] CancelarRequisicaoRequest body, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CancelarRequisicaoCommand(id, RequireEmpresaId(), CurrentUser.UserId, body.Motivo), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }
    }

    public record ReprovarRequisicaoRequest(string Motivo);

    public record CancelarRequisicaoRequest(string Motivo);
}
