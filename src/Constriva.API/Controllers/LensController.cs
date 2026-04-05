using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Lens.Commands;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Queries;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/lens")]
public sealed class LensController : BaseController
{
    public LensController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    /// <summary>Upload e processamento de documento via OCR.</summary>
    [HttpPost("upload")]
    [RequestSizeLimit(52_428_800)] // 50 MB
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile arquivo, [FromForm] InitProcessamentoLensDto dto, CancellationToken ct)
    {
        try
        {
            var empresaId = RequireEmpresaId();
            var processamentoId = await Mediator.Send(
                new CreateProcessamentoLensCommand(empresaId, CurrentUser.UserId, arquivo, dto), ct);
            return Accepted(new { sucesso = true, dados = new { processamentoId }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Lista processamentos com filtros e paginacao.</summary>
    [HttpGet("processamentos")]
    public async Task<IActionResult> GetProcessamentos(
        [FromQuery] Guid? obraId, [FromQuery] StatusProcessamentoLensEnum? status,
        [FromQuery] TipoDocumentoLensEnum? tipoDocumento,
        [FromQuery] DateTime? de, [FromQuery] DateTime? ate,
        [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20,
        CancellationToken ct = default)
    {
        try
        {
            var result = await Mediator.Send(new GetProcessamentosLensQuery(
                RequireEmpresaId(), obraId, status, tipoDocumento, de, ate, pagina, tamanhoPagina), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Obtem detalhes de um processamento.</summary>
    [HttpGet("processamentos/{id:guid}")]
    public async Task<IActionResult> GetProcessamento(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetProcessamentoByIdQuery(id, RequireEmpresaId()), ct);
            if (result is null) return NotFound(new { sucesso = false, dados = (object?)null, erros = new[] { "Processamento nao encontrado." } });
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Obtem resultado completo com sugestoes de matching.</summary>
    [HttpGet("processamentos/{id:guid}/resultado")]
    public async Task<IActionResult> GetResultado(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetResultadoProcessamentoQuery(id, RequireEmpresaId()), ct);
            if (result is null) return NotFound(new { sucesso = false, dados = (object?)null, erros = new[] { "Processamento nao encontrado." } });
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Edita um item extraido.</summary>
    [HttpPatch("processamentos/{id:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> EditItem(Guid id, Guid itemId, [FromBody] EditItemLensDto dto, CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new EditItemLensCommand(id, itemId, RequireEmpresaId(), CurrentUser.UserId, dto), ct);
            return Ok(new { sucesso = true, dados = new { mensagem = "Item atualizado com sucesso." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Aprova um item extraido.</summary>
    [HttpPost("processamentos/{id:guid}/itens/{itemId:guid}/aprovar")]
    public async Task<IActionResult> ApproveItem(Guid id, Guid itemId, CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new ApproveItemLensCommand(id, itemId, RequireEmpresaId(), CurrentUser.UserId), ct);
            return Ok(new { sucesso = true, dados = new { mensagem = "Item aprovado com sucesso." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Rejeita um item extraido.</summary>
    [HttpPost("processamentos/{id:guid}/itens/{itemId:guid}/rejeitar")]
    public async Task<IActionResult> RejectItem(Guid id, Guid itemId, [FromBody] RejectItemLensDto dto, CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new RejectItemLensCommand(id, itemId, RequireEmpresaId(), CurrentUser.UserId, dto.Motivo), ct);
            return Ok(new { sucesso = true, dados = new { mensagem = "Item rejeitado com sucesso." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Consolida dados extraidos em pedido de compra.</summary>
    [HttpPost("processamentos/{id:guid}/consolidar")]
    public async Task<IActionResult> Consolidar(Guid id, [FromBody] ConsolidarDocumentoLensDto dto, CancellationToken ct)
    {
        try
        {
            var compraId = await Mediator.Send(
                new ConsolidarDocumentoLensCommand(id, RequireEmpresaId(), CurrentUser.UserId, dto), ct);
            return Ok(new { sucesso = true, dados = new { compraId, mensagem = "Consolidacao realizada com sucesso." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Rejeita um processamento completo.</summary>
    [HttpPost("processamentos/{id:guid}/rejeitar")]
    public async Task<IActionResult> RejectProcessamento(Guid id, [FromBody] RejectItemLensDto dto, CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new UpdateStatusProcessamentoCommand(
                id, StatusProcessamentoLensEnum.Rejeitado,
                null, null, null, null, null, null, null, null, null, null, null, null, null, null), ct);
            return Ok(new { sucesso = true, dados = new { mensagem = "Processamento rejeitado." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Exclui um processamento (apenas em Pendente ou Erro).</summary>
    [HttpDelete("processamentos/{id:guid}")]
    public async Task<IActionResult> DeleteProcessamento(Guid id, CancellationToken ct)
    {
        try
        {
            // Use UpdateStatus to set Cancelado - soft delete via base entity
            await Mediator.Send(new UpdateStatusProcessamentoCommand(
                id, StatusProcessamentoLensEnum.Cancelado,
                null, null, null, null, null, null, null, null, null, null, null, null, null, null), ct);
            return NoContent();
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Reprocessa um documento.</summary>
    [HttpPost("processamentos/{id:guid}/reprocessar")]
    public async Task<IActionResult> Reprocessar(Guid id, CancellationToken ct)
    {
        try
        {
            await Mediator.Send(new ReprocessDocumentoLensCommand(id, RequireEmpresaId(), CurrentUser.UserId, null), ct);
            return Accepted(new { sucesso = true, dados = new { mensagem = "Reprocessamento iniciado." }, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    // -- Analytics --

    /// <summary>Resumo analytics dos processamentos.</summary>
    [HttpGet("analytics/resumo")]
    public async Task<IActionResult> GetAnalyticsResumo([FromQuery] DateTime? de, [FromQuery] DateTime? ate, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAnalyticsResumoQuery(RequireEmpresaId(), de, ate), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Analytics por tipo de documento.</summary>
    [HttpGet("analytics/por-tipo")]
    public async Task<IActionResult> GetAnalyticsPorTipo([FromQuery] DateTime? de, [FromQuery] DateTime? ate, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetAnalyticsPorTipoQuery(RequireEmpresaId(), de, ate), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Tendencia de confidence score.</summary>
    [HttpGet("analytics/confidence-trend")]
    public async Task<IActionResult> GetTendenciaConfidence([FromQuery] DateTime? de, [FromQuery] DateTime? ate, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetTendenciaConfidenceQuery(RequireEmpresaId(), de, ate), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Warnings mais frequentes.</summary>
    [HttpGet("analytics/warnings-frequentes")]
    public async Task<IActionResult> GetWarningsFrequentes([FromQuery] int limite = 10, CancellationToken ct = default)
    {
        try
        {
            var result = await Mediator.Send(new GetWarningsFrequentesQuery(RequireEmpresaId(), limite), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Log de processamento OCR (MongoDB).</summary>
    [HttpGet("processamentos/{id:guid}/log")]
    public async Task<IActionResult> GetLog(Guid id, CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(new GetLogProcessamentoQuery(id, RequireEmpresaId()), ct);
            return Ok(new { sucesso = true, dados = result, erros = Array.Empty<string>() });
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>Lista tipos de documento suportados.</summary>
    [HttpGet("config/tipos-documento")]
    public IActionResult GetTiposDocumento()
    {
        var tipos = Enum.GetValues<TipoDocumentoLensEnum>()
            .Select(t => new { valor = (int)t, nome = t.ToString(), descricao = t.ToDescricao() })
            .ToList();
        return Ok(new { sucesso = true, dados = tipos, erros = Array.Empty<string>() });
    }

    /// <summary>Verifica status do servico Constriva.Lens.</summary>
    [HttpGet("config/status")]
    public IActionResult GetStatus()
    {
        return Ok(new { sucesso = true, dados = new { lensDisponivel = true, timestamp = DateTime.UtcNow }, erros = Array.Empty<string>() });
    }
}
