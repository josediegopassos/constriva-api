using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Application.Features.Orcamento.Queries;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.API.Controllers.Orcamento;

/// <summary>
/// Gerenciamento de Orçamentos por Obra
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/obras/{obraId}/orcamentos")]
[Produces("application/json")]
public class ObraOrcamentosController : OrcamentoBaseController
{
    public ObraOrcamentosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    /// <summary>Lista todos os orçamentos de uma obra</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrcamentoResumoDto>), 200)]
    public async Task<ActionResult<IEnumerable<OrcamentoResumoDto>>> GetOrcamentos(
        Guid obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetOrcamentosByObraQuery(obraId, EmpresaId), ct));

    /// <summary>Cria um novo orçamento para a obra</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrcamentoResumoDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<OrcamentoResumoDto>> Create(
        Guid obraId,
        [FromBody] CreateOrcamentoDto dto,
        CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateOrcamentoCommand(obraId, EmpresaId, CurrentUser.UserId, dto), ct);

        return CreatedAtRoute("GetOrcamento", new { id = result.Id }, result);
    }

    /// <summary>Dashboard de orçamentos da obra</summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(OrcamentoDashboardDto), 200)]
    public async Task<ActionResult<OrcamentoDashboardDto>> Dashboard(
        Guid obraId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetOrcamentoDashboardQuery(obraId, EmpresaId), ct));
}

/// <summary>
/// Gerenciamento de Orçamentos (detalhes, grupos, itens, composições)
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/orcamentos")]
[Produces("application/json")]
public class OrcamentosController : OrcamentoBaseController
{
    public OrcamentosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    // ─── Listagem ──────────────────────────────────────────────────────────────

    /// <summary>Lista orçamentos da empresa com paginação</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<OrcamentoResumoDto>), 200)]
    public async Task<ActionResult<PaginatedResult<OrcamentoResumoDto>>> GetOrcamentos(
        [FromQuery] Guid? obraId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await Mediator.Send(new GetOrcamentosQuery(EmpresaId, obraId, page, pageSize), ct));

    // ─── CRUD Principal ────────────────────────────────────────────────────────

    /// <summary>Obtém o detalhe completo de um orçamento</summary>
    [HttpGet("{id}", Name = "GetOrcamento")]
    [ProducesResponseType(typeof(OrcamentoDetalheDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OrcamentoDetalheDto>> GetOrcamento(Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new GetOrcamentoDetalheQuery(id, EmpresaId), ct);
        return result == null ? NotFound() : Ok(result);
    }

    /// <summary>Atualiza um orçamento existente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OrcamentoResumoDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OrcamentoResumoDto>> UpdateOrcamento(
        Guid id, [FromBody] UpdateOrcamentoDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(new UpdateOrcamentoCommand(id, EmpresaId, dto), ct);
        return Ok(result);
    }

    /// <summary>Exclui um orçamento (soft delete)</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOrcamento(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteOrcamentoCommand(id, EmpresaId), ct);
        return NoContent();
    }

    // ─── Status ─────────────────────────────────────────────────────────────────

    /// <summary>Altera o status de um orçamento</summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AlterarStatus(Guid id, [FromBody] AlterarStatusOrcamentoDto dto, CancellationToken ct)
    {
        try { await Mediator.Send(new AlterarStatusOrcamentoCommand(id, EmpresaId, CurrentUser.UserId, dto.Status, dto.Observacao), ct); return NoContent(); }
        catch (Exception ex) { return HandleException(ex); }
    }

    // ─── Workflow de Aprovação ─────────────────────────────────────────────────

    /// <summary>Envia um orçamento para revisão</summary>
    [HttpPatch("{id}/revisar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> EnviarRevisao(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new EnviarParaRevisaoCommand(id, EmpresaId, CurrentUser.UserId), ct);
        return NoContent();
    }

    /// <summary>Aprova um orçamento</summary>
    [HttpPatch("{id}/aprovar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AprovarOrcamento(
        Guid id, [FromBody] AprovarOrcamentoDto dto, CancellationToken ct)
    {
        await Mediator.Send(
            new AprovarOrcamentoCommand(id, EmpresaId, CurrentUser.UserId, dto.Observacao), ct);
        return NoContent();
    }

    /// <summary>Reprova um orçamento (retorna para rascunho)</summary>
    [HttpPatch("{id}/reprovar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ReprovarOrcamento(
        Guid id, [FromBody] ReprovarOrcamentoDto dto, CancellationToken ct)
    {
        await Mediator.Send(
            new ReprovarOrcamentoCommand(id, EmpresaId, CurrentUser.UserId, dto.Motivo), ct);
        return NoContent();
    }

    /// <summary>Define um orçamento aprovado como linha de base</summary>
    [HttpPatch("{id}/linha-base")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DefinirLinhaDBase(Guid id, CancellationToken ct)
    {
        await Mediator.Send(new DefinirLinhaDBaseCommand(id, EmpresaId), ct);
        return NoContent();
    }

    /// <summary>Duplica um orçamento como nova versão (rascunho)</summary>
    [HttpPost("{id}/duplicar")]
    [ProducesResponseType(typeof(OrcamentoResumoDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OrcamentoResumoDto>> Duplicar(
        Guid id, [FromBody] DuplicarRequest req, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new DuplicarOrcamentoCommand(id, EmpresaId, CurrentUser.UserId, req.NovoNome), ct);
        return CreatedAtRoute("GetOrcamento", new { id = result.Id }, result);
    }

    // ─── Grupos ────────────────────────────────────────────────────────────────

    /// <summary>Lista grupos de um orçamento</summary>
    [HttpGet("{orcamentoId}/grupos")]
    [ProducesResponseType(typeof(IEnumerable<GrupoOrcamentoDto>), 200)]
    public async Task<ActionResult<IEnumerable<GrupoOrcamentoDto>>> GetGrupos(
        Guid orcamentoId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetGruposOrcamentoQuery(orcamentoId, EmpresaId), ct));

    /// <summary>Cria um novo grupo no orçamento</summary>
    [HttpPost("{orcamentoId}/grupos")]
    [ProducesResponseType(typeof(GrupoOrcamentoDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GrupoOrcamentoDto>> CreateGrupo(
        Guid orcamentoId, [FromBody] CreateGrupoDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateGrupoOrcamentoCommand(orcamentoId, EmpresaId, dto), ct);
        return StatusCode(201, result);
    }

    /// <summary>Atualiza um grupo existente</summary>
    [HttpPut("grupos/{grupoId}")]
    [ProducesResponseType(typeof(GrupoOrcamentoDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GrupoOrcamentoDto>> UpdateGrupo(
        Guid grupoId, [FromBody] UpdateGrupoDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new UpdateGrupoOrcamentoCommand(grupoId, EmpresaId, dto), ct);
        return Ok(result);
    }

    /// <summary>Exclui um grupo (soft delete)</summary>
    [HttpDelete("grupos/{grupoId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteGrupo(Guid grupoId, CancellationToken ct)
    {
        await Mediator.Send(new DeleteGrupoOrcamentoCommand(grupoId, EmpresaId), ct);
        return NoContent();
    }

    // ─── Itens ─────────────────────────────────────────────────────────────────

    /// <summary>Lista itens de um grupo</summary>
    [HttpGet("grupos/{grupoId}/itens")]
    [ProducesResponseType(typeof(IEnumerable<ItemOrcamentoDto>), 200)]
    public async Task<ActionResult<IEnumerable<ItemOrcamentoDto>>> GetItens(
        Guid grupoId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetItensGrupoQuery(grupoId, EmpresaId), ct));

    /// <summary>Cria um novo item no grupo</summary>
    [HttpPost("grupos/{grupoId}/itens")]
    [ProducesResponseType(typeof(ItemOrcamentoDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ItemOrcamentoDto>> CreateItem(
        Guid grupoId, [FromBody] CreateItemOrcDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateItemOrcamentoCommand(grupoId, EmpresaId, dto), ct);
        return StatusCode(201, result);
    }

    /// <summary>Atualiza um item existente</summary>
    [HttpPut("itens/{itemId}")]
    [ProducesResponseType(typeof(ItemOrcamentoDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ItemOrcamentoDto>> UpdateItem(
        Guid itemId, [FromBody] UpdateItemOrcDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new UpdateItemOrcamentoCommand(itemId, EmpresaId, dto), ct);
        return Ok(result);
    }

    /// <summary>Exclui um item (soft delete)</summary>
    [HttpDelete("itens/{itemId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteItem(Guid itemId, CancellationToken ct)
    {
        await Mediator.Send(new DeleteItemOrcamentoCommand(itemId, EmpresaId), ct);
        return NoContent();
    }

    // ─── Composições ───────────────────────────────────────────────────────────

    /// <summary>Lista composições de um orçamento</summary>
    [HttpGet("{orcamentoId}/composicoes")]
    [ProducesResponseType(typeof(IEnumerable<ComposicaoOrcamentoDto>), 200)]
    public async Task<ActionResult<IEnumerable<ComposicaoOrcamentoDto>>> GetComposicoes(
        Guid orcamentoId, CancellationToken ct)
        => Ok(await Mediator.Send(new GetComposicoesOrcamentoQuery(orcamentoId, EmpresaId), ct));

    /// <summary>Cria uma nova composição</summary>
    [HttpPost("{orcamentoId}/composicoes")]
    [ProducesResponseType(typeof(ComposicaoOrcamentoDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ComposicaoOrcamentoDto>> CreateComposicao(
        Guid orcamentoId, [FromBody] CreateComposicaoDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateComposicaoCommand(orcamentoId, EmpresaId, dto), ct);
        return StatusCode(201, result);
    }

    /// <summary>Adiciona insumo a uma composição</summary>
    [HttpPost("composicoes/{composicaoId}/insumos")]
    [ProducesResponseType(typeof(InsumoComposicaoDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<InsumoComposicaoDto>> CreateInsumo(
        Guid composicaoId, [FromBody] CreateInsumoDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateInsumoCommand(composicaoId, EmpresaId, dto), ct);
        return StatusCode(201, result);
    }

    // ─── SINAPI ────────────────────────────────────────────────────────────────

    /// <summary>Importa itens da tabela SINAPI</summary>
    [HttpPost("{orcamentoId}/importar-sinapi")]
    [ProducesResponseType(typeof(ImportarSinapiResultDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ImportarSinapiResultDto>> ImportarSinapi(
        Guid orcamentoId, [FromBody] ImportarSinapiDto dto, CancellationToken ct)
    {
        var importados = await Mediator.Send(
            new ImportarSinapiCommand(orcamentoId, EmpresaId, dto), ct);
        return Ok(new ImportarSinapiResultDto(importados, $"{importados} item(ns) SINAPI importado(s) com sucesso."));
    }

    // ─── Histórico ─────────────────────────────────────────────────────────────

    /// <summary>Histórico de alterações do orçamento</summary>
    [HttpGet("{id}/historico")]
    [ProducesResponseType(typeof(IEnumerable<HistoricoOrcamentoDto>), 200)]
    public async Task<ActionResult<IEnumerable<HistoricoOrcamentoDto>>> GetHistorico(
        Guid id, CancellationToken ct)
        => Ok(await Mediator.Send(new GetHistoricoOrcamentoQuery(id, EmpresaId), ct));
}

// ─── Base Controller ──────────────────────────────────────────────────────────

public abstract class OrcamentoBaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly ICurrentUser CurrentUser;
    protected Guid EmpresaId => CurrentUser.EmpresaId ?? throw new UnauthorizedAccessException();

    protected OrcamentoBaseController(IMediator mediator, ICurrentUser currentUser)
    {
        Mediator = mediator;
        CurrentUser = currentUser;
    }
}

// ─── Request/Response Models ──────────────────────────────────────────────────

public record DuplicarRequest(string NovoNome);
public record ImportarSinapiResultDto(int Importados, string Mensagem);
