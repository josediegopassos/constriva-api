using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Estoque;
using Constriva.Application.Features.Estoque.Commands;
using Constriva.Application.Features.Estoque.DTOs;
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

        [HttpGet("saldos")]
        public async Task<ActionResult<IEnumerable<SaldoEstoqueDto>>> GetSaldos(
            [FromQuery] Guid? almoxarifadoId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetSaldosEstoqueQuery(RequireEmpresaId(), almoxarifadoId), ct));

        [HttpGet("movimentacoes")]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoqueDto>>> GetMovimentacoes(
            [FromQuery] Guid? almoxarifadoId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim,
            CancellationToken ct)
            => Ok(await Mediator.Send(new GetMovimentacoesEstoqueQuery(RequireEmpresaId(), almoxarifadoId, inicio, fim), ct));

        [HttpGet("almoxarifados")]
        public async Task<ActionResult<IEnumerable<AlmoxarifadoDto>>> GetAlmoxarifados(CancellationToken ct)
            => Ok(await Mediator.Send(new GetAlmoxarifadosQuery(RequireEmpresaId()), ct));

        [HttpGet("requisicoes")]
        public async Task<ActionResult<PaginatedResult<RequisicaoDto>>> GetRequisicoes(
            [FromQuery] Guid? obraId, [FromQuery] StatusRequisicaoEnum? status,
            [FromQuery] int page = 1, CancellationToken ct = default)
            => Ok(await Mediator.Send(new GetRequisicoesQuery(RequireEmpresaId(), obraId, status, page), ct));

        [HttpPost("requisicoes")]
        public async Task<IActionResult> CreateRequisicao([FromBody] CreateRequisicaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateRequisicaoCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("materiais")]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateMaterialCommand(RequireEmpresaId(), dto.Nome, dto.Unidade, dto.Categoria, dto.CodigoInterno, dto.PrecoUnitario), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("materiais/{id:guid}")]
        public async Task<IActionResult> UpdateMaterial(Guid id, [FromBody] UpdateMaterialDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateMaterialCommand(id, RequireEmpresaId(), dto.Nome, dto.Unidade, dto.Categoria, dto.CodigoInterno, dto.PrecoUnitario), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("materiais/{id:guid}")]
        public async Task<IActionResult> DeleteMaterial(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteMaterialCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("almoxarifados")]
        public async Task<IActionResult> CreateAlmoxarifado([FromBody] CreateAlmoxarifadoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateAlmoxarifadoCommand(RequireEmpresaId(), dto.Nome, dto.ObraId, dto.Responsavel), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("almoxarifados/{id:guid}")]
        public async Task<IActionResult> UpdateAlmoxarifado(Guid id, [FromBody] UpdateAlmoxarifadoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateAlmoxarifadoCommand(id, RequireEmpresaId(), dto.Nome, dto.Responsavel), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("almoxarifados/{id:guid}")]
        public async Task<IActionResult> DeleteAlmoxarifado(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteAlmoxarifadoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("requisicoes/{id:guid}")]
        public async Task<IActionResult> UpdateRequisicao(Guid id, [FromBody] UpdateRequisicaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateRequisicaoCommand(id, RequireEmpresaId(), dto.Descricao, dto.DataNecessidade, dto.Observacoes), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("requisicoes/{id:guid}")]
        public async Task<IActionResult> DeleteRequisicao(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteRequisicaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }
    }
}
