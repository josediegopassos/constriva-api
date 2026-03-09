using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Cronograma;
using Constriva.Application.Features.Cronograma.Commands;
using Constriva.Application.Features.Cronograma.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers
{
    [Authorize]
    [Route("api/v1/cronograma")]
    public sealed class CronogramaController : BaseController
    {
        public CronogramaController(IMediator mediator, ICurrentUser currentUser)
            : base(mediator, currentUser) { }

        [HttpGet("{obraId:guid}")]
        public async Task<ActionResult<CronogramaObraDto?>> GetCronograma(Guid obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetCronogramaQuery(obraId, RequireEmpresaId()), ct));

        [HttpPost("{obraId:guid}")]
        public async Task<IActionResult> CreateCronograma(Guid obraId, [FromBody] CreateCronogramaDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateCronogramaCommand(obraId, RequireEmpresaId(), dto.Nome, dto.DataInicio, dto.DataFim, dto.Descricao, dto.Observacoes), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("{obraId:guid}/atividades")]
        public async Task<ActionResult<IEnumerable<AtividadeDto>>> GetAtividades(Guid obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetAtividadesQuery(obraId, RequireEmpresaId()), ct));

        [HttpPost("{obraId:guid}/atividades")]
        public async Task<IActionResult> CreateAtividade(Guid obraId, [FromBody] CreateAtividadeDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new CreateAtividadeCommand(obraId, RequireEmpresaId(), dto), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpPatch("atividades/{id:guid}/progresso")]
        public async Task<IActionResult> UpdateProgresso(Guid id, [FromBody] UpdateProgressoDto dto, CancellationToken ct)
        {
            try { await Mediator.Send(new UpdateProgressoAtividadeCommand(id, RequireEmpresaId(), dto.Percentual), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpGet("{obraId:guid}/curva-s")]
        public async Task<ActionResult<IEnumerable<CurvaSPontoDto>>> GetCurvaS(Guid obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetCurvaSQuery(obraId, RequireEmpresaId()), ct));

        [HttpGet("{obraId:guid}/evm")]
        public async Task<ActionResult<EVMDto>> GetEVM(Guid obraId, CancellationToken ct)
            => Ok(await Mediator.Send(new GetEVMQuery(obraId, RequireEmpresaId()), ct));

        [HttpPut("atividades/{id:guid}")]
        public async Task<IActionResult> UpdateAtividade(Guid id, [FromBody] UpdateAtividadeDto dto, CancellationToken ct)
        {
            try { return Ok(await Mediator.Send(new UpdateAtividadeCommand(id, RequireEmpresaId(), dto.Nome, dto.Descricao, dto.DataInicio, dto.DataFim, dto.PercentualConcluido, dto.Responsavel), ct)); }
            catch (Exception ex) { return HandleException(ex); }
        }

        [HttpDelete("atividades/{id:guid}")]
        public async Task<IActionResult> DeleteAtividade(Guid id, CancellationToken ct)
        {
            try { await Mediator.Send(new DeleteAtividadeCommand(id, RequireEmpresaId()), ct); return NoContent(); }
            catch (Exception ex) { return HandleException(ex); }
        }
    }
}
