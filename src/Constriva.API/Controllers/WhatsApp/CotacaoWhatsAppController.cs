using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Compras.WhatsApp.Commands;
using Constriva.Application.Features.Compras.WhatsApp.Queries;

namespace Constriva.API.Controllers.WhatsApp;

[Authorize]
[Route("api/v1/cotacoes/{cotacaoId:guid}/whatsapp")]
[ApiController]
public sealed class CotacaoWhatsAppController : BaseController
{
    public CotacaoWhatsAppController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpPost("enviar")]
    [ProducesResponseType(typeof(EnviarCotacoesWhatsAppResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EnviarCotacoesWhatsAppResult>> EnviarCotacao(
        [FromRoute] Guid cotacaoId,
        [FromBody] EnviarCotacaoWhatsAppRequest request,
        CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(
                new EnviarCotacoesWhatsAppCommand(
                    RequireEmpresaId(), CurrentUser.UserId, cotacaoId,
                    request.FornecedoresIds,
                    request.MensagemPersonalizada,
                    request.DataLimiteResposta), ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("status")]
    [ProducesResponseType(typeof(CotacaoWhatsAppStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CotacaoWhatsAppStatusDto>> ObterStatus(
        [FromRoute] Guid cotacaoId,
        CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(
                new GetCotacaoWhatsAppStatusQuery(RequireEmpresaId(), cotacaoId), ct);
            return result == null
                ? NotFound("Nenhuma sessão WhatsApp encontrada para esta cotação")
                : Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("comparativo")]
    [ProducesResponseType(typeof(PropostasComparativoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PropostasComparativoDto>> ObterComparativo(
        [FromRoute] Guid cotacaoId,
        CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(
                new GetPropostasComparativoQuery(RequireEmpresaId(), cotacaoId), ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpPost("aprovar")]
    [ProducesResponseType(typeof(AprovarPropostaWhatsAppResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AprovarPropostaWhatsAppResult>> AprovarProposta(
        [FromRoute] Guid cotacaoId,
        [FromBody] AprovarPropostaWhatsAppRequest request,
        CancellationToken ct)
    {
        try
        {
            var result = await Mediator.Send(
                new AprovarPropostaWhatsAppCommand(
                    RequireEmpresaId(), CurrentUser.UserId,
                    cotacaoId, request.PropostaCotacaoId), ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

public record EnviarCotacaoWhatsAppRequest
{
    public IReadOnlyList<Guid>? FornecedoresIds { get; init; }
    public string? MensagemPersonalizada { get; init; }
    public DateTime? DataLimiteResposta { get; init; }
}

public record AprovarPropostaWhatsAppRequest
{
    public required Guid PropostaCotacaoId { get; init; }
}
