using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Compras;
using Constriva.Application.Features.Compras.Commands;
using Constriva.Application.Features.Compras.DTOs;
using Constriva.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers
{
    [Authorize]
    [Route("api/v1/compras")]
    public sealed class ComprasController : BaseController
    {
        public ComprasController(IMediator mediator, ICurrentUser currentUser)
            : base(mediator, currentUser) { }

        [HttpGet("pedidos")]
        public async Task<ActionResult<PaginatedResult<PedidoCompraDto>>> GetPedidos(
            [FromQuery] int page = 1, [FromQuery] StatusPedidoCompraEnum? status = null,
            [FromQuery] Guid? obraId = null, CancellationToken ct = default)
            => Ok(await Mediator.Send(new GetPedidosCompraQuery(RequireEmpresaId(), obraId, status, page), ct));

        [HttpPost("pedidos")]
        public async Task<IActionResult> CreatePedido([FromBody] CreatePedidoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreatePedidoCompraCommand(RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("pedidos/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatusPedido(
            Guid id, [FromBody] UpdateStatusPedidoRequest req, CancellationToken ct)
        {
            try { await Mediator.Send(new UpdateStatusPedidoCommand(id, RequireEmpresaId(), req.Status), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("cotacoes")]
        public async Task<ActionResult<IEnumerable<CotacaoDto>>> GetCotacoes(
            [FromQuery] Guid? obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetCotacoesQuery(RequireEmpresaId(), obraId), ct));

        [HttpGet("fornecedores")]
        public async Task<ActionResult<PaginatedResult<FornecedorDto>>> GetFornecedores(
            [FromQuery] string? search, [FromQuery] TipoFornecedorEnum? tipo,
            [FromQuery] int page = 1, CancellationToken ct = default)
            => Ok(await Mediator.Send(new GetFornecedoresQuery(RequireEmpresaId(), search, tipo, page), ct));

        [HttpPost("fornecedores")]
        public async Task<IActionResult> CreateFornecedor([FromBody] CreateFornecedorDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateFornecedorCommand(RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("fornecedores/{id:guid}")]
        public async Task<IActionResult> UpdateFornecedor(Guid id, [FromBody] UpdateFornecedorDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateFornecedorCommand(id, RequireEmpresaId(), dto.RazaoSocial, dto.NomeFantasia, dto.CNPJ, dto.Email, dto.Telefone, dto.Endereco, dto.Tipo), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("fornecedores/{id:guid}")]
        public async Task<IActionResult> DeleteFornecedor(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteFornecedorCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("pedidos/{id:guid}")]
        public async Task<IActionResult> UpdatePedido(Guid id, [FromBody] UpdatePedidoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdatePedidoCompraCommand(id, RequireEmpresaId(), dto.FornecedorId, dto.DataEntregaPrevista, dto.Observacoes), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("pedidos/{id:guid}")]
        public async Task<IActionResult> DeletePedido(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeletePedidoCompraCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }
    }

    public record UpdateStatusPedidoRequest(StatusPedidoCompraEnum Status);
}
