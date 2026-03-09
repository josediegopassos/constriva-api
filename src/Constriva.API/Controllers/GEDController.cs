using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.GED;
using Constriva.Application.Features.GED.Commands;
using Constriva.Application.Features.GED.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/ged")]
public sealed class GEDController : BaseController
{
    public GEDController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet("pastas")]
    public async Task<ActionResult<IEnumerable<PastaDto>>> GetPastas(
        [FromQuery] Guid? obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetPastasQuery(RequireEmpresaId(), obraId), ct));

    [HttpPost("pastas")]
    public async Task<IActionResult> CreatePasta([FromBody] CreatePastaDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new CreatePastaCommand(RequireEmpresaId(), dto.Nome, dto.ObraId, dto.PastaPaiId), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("pastas/{id:guid}")]
    public async Task<IActionResult> UpdatePasta(Guid id, [FromBody] UpdatePastaDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdatePastaCommand(id, RequireEmpresaId(), dto.Nome), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("pastas/{id:guid}")]
    public async Task<IActionResult> DeletePasta(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeletePastaCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpGet("documentos")]
    public async Task<ActionResult<PaginatedResult<DocumentoGEDDto>>> GetDocumentos(
        [FromQuery] Guid? pastaId, [FromQuery] string? search,
        [FromQuery] TipoDocumentoGEDEnum? tipo, [FromQuery] int page = 1, CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetDocumentosQuery(RequireEmpresaId(), pastaId, search, tipo, page), ct));

    [HttpPost("documentos/upload")]
    public async Task<IActionResult> Upload([FromForm] UploadDocumentoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UploadDocumentoCommand(RequireEmpresaId(), dto), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPut("documentos/{id:guid}")]
    public async Task<IActionResult> UpdateDocumento(Guid id, [FromBody] UpdateDocumentoDto dto, CancellationToken ct)
    {
        try { return Ok(await Mediator.Send(new UpdateDocumentoCommand(id, RequireEmpresaId(), dto.Nome, dto.Descricao, dto.DataVencimento, dto.Tags), ct)); }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpDelete("documentos/{id:guid}")]
    public async Task<IActionResult> DeleteDocumento(Guid id, CancellationToken ct)
    {
        try { await Mediator.Send(new DeleteDocumentoCommand(id, RequireEmpresaId()), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
