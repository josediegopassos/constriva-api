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

        [HttpGet("pedidos/{id:guid}")]
        public async Task<IActionResult> GetPedidoById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await Mediator.Send(new GetPedidoCompraByIdQuery(id, RequireEmpresaId()), ct);
                return result is null ? NotFound(new { message = "Pedido não encontrado." }) : Ok(result);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("pedidos")]
        public async Task<IActionResult> CreatePedido([FromBody] CreatePedidoDto dto, CancellationToken ct)
        {
            try { 
                return Ok(await Mediator.Send(new CreatePedidoCompraCommand(RequireEmpresaId(), dto), ct)); 
            }
            catch (Exception ex) 
            { 
                return HandleException(ex); 
            }
        }

        [HttpPatch("pedidos/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatusPedido(
            Guid id, [FromBody] UpdateStatusPedidoRequest req, CancellationToken ct)
        {
            try { await Mediator.Send(new UpdateStatusPedidoCommand(id, RequireEmpresaId(), req.Status), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("pedidos/{id:guid}")]
        public async Task<IActionResult> UpdatePedido(Guid id, [FromBody] UpdatePedidoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdatePedidoCompraCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("pedidos/{id:guid}")]
        public async Task<IActionResult> DeletePedido(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeletePedidoCompraCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("cotacoes")]
        public async Task<ActionResult<IEnumerable<CotacaoDto>>> GetCotacoes(
            [FromQuery] Guid? obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetCotacoesQuery(RequireEmpresaId(), obraId), ct));

        [HttpGet("cotacoes/{id:guid}")]
        public async Task<IActionResult> GetCotacaoById(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await Mediator.Send(new GetCotacaoByIdQuery(id, RequireEmpresaId()), ct);
                return result is null ? NotFound(new { message = "Cotação não encontrada." }) : Ok(result);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("cotacoes")]
        public async Task<IActionResult> CreateCotacao([FromBody] CreateCotacaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateCotacaoCommand(RequireEmpresaId(), CurrentUser.UserId, dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("cotacoes/{id:guid}")]
        public async Task<IActionResult> UpdateCotacao(Guid id, [FromBody] UpdateCotacaoDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateCotacaoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("cotacoes/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatusCotacao(Guid id, [FromBody] UpdateStatusCotacaoRequest req, CancellationToken ct)
        {
            try
            {
                await Mediator.Send(new UpdateStatusCotacaoCommand(id, RequireEmpresaId(), req.Status, req.FornecedorVencedorId, req.Observacoes), ct);
                return NoContent();
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("cotacoes/{id:guid}")]
        public async Task<IActionResult> DeleteCotacao(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteCotacaoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        // ─── Fornecedores Convidados ─────────────────────────────────────────────

        [HttpGet("cotacoes/{id:guid}/fornecedores")]
        public async Task<IActionResult> GetFornecedoresCotacao(Guid id, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new GetFornecedoresCotacaoQuery(id, RequireEmpresaId()), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("cotacoes/{id:guid}/fornecedores")]
        public async Task<IActionResult> ConvidarFornecedores(Guid id, [FromBody] ConvidarFornecedoresDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new ConvidarFornecedoresCotacaoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("cotacoes/{id:guid}/fornecedores/{fornecedorId:guid}")]
        public async Task<IActionResult> RemoverFornecedorCotacao(Guid id, Guid fornecedorId, CancellationToken ct)
        {
            try { await Mediator.Send(new RemoverFornecedorCotacaoCommand(id, fornecedorId, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        // ─── Propostas ───────────────────────────────────────────────────────────

        [HttpGet("cotacoes/{id:guid}/propostas")]
        public async Task<IActionResult> GetPropostasCotacao(Guid id, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new GetPropostasCotacaoQuery(id, RequireEmpresaId()), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("cotacoes/{id:guid}/propostas/{propostaId:guid}")]
        public async Task<IActionResult> GetPropostaById(Guid id, Guid propostaId, CancellationToken ct)
        {
            try
            {
                var result = await Mediator.Send(new GetPropostaByIdQuery(propostaId, RequireEmpresaId()), ct);
                return result is null ? NotFound(new { message = "Proposta não encontrada." }) : Ok(result);
            }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPost("cotacoes/{id:guid}/propostas")]
        public async Task<IActionResult> CreateProposta(Guid id, [FromBody] CreatePropostaDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreatePropostaCotacaoCommand(id, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPut("cotacoes/{id:guid}/propostas/{propostaId:guid}")]
        public async Task<IActionResult> UpdateProposta(Guid id, Guid propostaId, [FromBody] UpdatePropostaDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdatePropostaCotacaoCommand(propostaId, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("cotacoes/{id:guid}/propostas/{propostaId:guid}")]
        public async Task<IActionResult> DeleteProposta(Guid id, Guid propostaId, CancellationToken ct)
        {
            try { await Mediator.Send(new DeletePropostaCotacaoCommand(propostaId, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

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

        [HttpGet("formas-pagamento")]
        public IActionResult GetFormasPagamento()
        {
            var formas = Enum.GetValues<FormaPagamentoEnum>()
                .Select(f => new
                {
                    valor = (int)f,
                    nome = f.ToString(),
                    descricao = f switch
                    {
                        FormaPagamentoEnum.Dinheiro => "Dinheiro",
                        FormaPagamentoEnum.Cheque => "Cheque",
                        FormaPagamentoEnum.Transferencia => "Transferência Bancária",
                        FormaPagamentoEnum.Boleto => "Boleto Bancário",
                        FormaPagamentoEnum.CartaoCredito => "Cartão de Crédito",
                        FormaPagamentoEnum.Pix => "PIX",
                        _ => f.ToString()
                    }
                })
                .ToList();

            return Ok(new { sucesso = true, dados = formas, erros = Array.Empty<string>() });
        }
    }

    public record UpdateStatusPedidoRequest(StatusPedidoCompraEnum Status);
    public record UpdateStatusCotacaoRequest(StatusCotacaoEnum Status, Guid? FornecedorVencedorId = null, string? Observacoes = null);
}
