using Microsoft.AspNetCore.SignalR;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Extensions;

namespace Constriva.API.Hubs;

public class LensNotificationService : ILensNotificationService
{
    private readonly IHubContext<LensHub> _hub;

    public LensNotificationService(IHubContext<LensHub> hub) => _hub = hub;

    public async Task NotificarProcessamentoAtualizado(DocumentoLens doc, string mensagem)
    {
        var dto = new ProcessamentoUpdatedNotificationDto(
            doc.Id, doc.Status, doc.Status.ToString(),
            doc.TipoDocumentoDeclarado.ToDescricao(),
            DateTime.UtcNow, mensagem);

        await EnviarParaGrupos(doc, "LensProcessamentoAtualizado", dto);
    }

    public async Task NotificarProcessamentoConcluido(DocumentoLens doc, float confidenceScore, int totalItens, List<string> warnings, string? fornecedorSugerido, string? cnpjFornecedor, decimal? valorTotal, string? dataEmissao, int tempoProcessamentoMs)
    {
        var dto = new ProcessamentoCompletedNotificationDto(
            doc.Id, doc.Status.ToString(), doc.TipoDocumento.ToDescricao(),
            doc.TiposConferem, confidenceScore, totalItens, warnings,
            fornecedorSugerido, cnpjFornecedor, valorTotal, dataEmissao,
            tempoProcessamentoMs, "Processamento OCR concluído com sucesso.");

        await EnviarParaGrupos(doc, "LensProcessamentoConcluido", dto);
    }

    public async Task NotificarProcessamentoErro(DocumentoLens doc, string codigoErro, string mensagemErro, bool podeReprocessar)
    {
        var dto = new ProcessamentoErrorNotificationDto(
            doc.Id, doc.Status.ToString(), doc.TipoDocumentoDeclarado.ToDescricao(),
            codigoErro, mensagemErro, podeReprocessar,
            "Ocorreu um erro no processamento OCR.");

        await EnviarParaGrupos(doc, "LensProcessamentoErro", dto);
    }

    public async Task NotificarItemAtualizado(Guid processamentoId, Guid itemId, string acao, Guid usuarioId, string nomeUsuario, Guid? obraId, Guid empresaId)
    {
        var dto = new ItemUpdatedNotificationDto(
            processamentoId, itemId, acao, usuarioId, nomeUsuario, DateTime.UtcNow);

        var tasks = new List<Task>
        {
            _hub.Clients.Group($"user-{usuarioId}").SendAsync("LensItemAtualizado", dto),
            _hub.Clients.Group($"empresa-{empresaId}").SendAsync("LensItemAtualizado", dto)
        };
        if (obraId.HasValue)
            tasks.Add(_hub.Clients.Group($"obra-{obraId}").SendAsync("LensItemAtualizado", dto));

        await Task.WhenAll(tasks);
    }

    public async Task NotificarConsolidacaoConcluida(DocumentoLens doc, Guid compraId, int totalItensConsolidados, int totalItensRejeitados, decimal valorTotal)
    {
        var dto = new ConsolidationCompletedNotificationDto(
            doc.Id, compraId, totalItensConsolidados, totalItensRejeitados,
            valorTotal, "Consolidação concluída com sucesso.");

        await EnviarParaGrupos(doc, "LensConsolidacaoConcluida", dto);
    }

    private async Task EnviarParaGrupos(DocumentoLens doc, string evento, object dados)
    {
        var tasks = new List<Task>
        {
            _hub.Clients.Group($"user-{doc.UsuarioId}").SendAsync(evento, dados),
            _hub.Clients.Group($"empresa-{doc.EmpresaId}").SendAsync(evento, dados)
        };
        if (doc.ObraId.HasValue)
            tasks.Add(_hub.Clients.Group($"obra-{doc.ObraId}").SendAsync(evento, dados));

        await Task.WhenAll(tasks);
    }
}
