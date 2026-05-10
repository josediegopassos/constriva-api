using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.Lens.Commands;
using Constriva.Messaging.Contracts.Lens.Events;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.Lens;

public class ReprocessarDocumentoLensConsumer : IConsumer<ReprocessDocumentoLensCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISignalRNotificationService _notificacao;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ReprocessarDocumentoLensConsumer> _logger;

    public ReprocessarDocumentoLensConsumer(
        IPublishEndpoint publishEndpoint,
        ISignalRNotificationService notificacao,
        IHttpClientFactory httpClientFactory,
        ILogger<ReprocessarDocumentoLensConsumer> logger)
    {
        _publishEndpoint = publishEndpoint;
        _notificacao = notificacao;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReprocessDocumentoLensCommand> context)
    {
        var comando = context.Message;
        var ct = context.CancellationToken;

        _logger.LogInformation(
            "Iniciando reprocessamento. ProcessamentoId: {ProcessamentoId}, Motivo: {Motivo}.",
            comando.ProcessamentoId, comando.MotivoReprocessamento);

        try
        {
            var client = _httpClientFactory.CreateClient("ConstrivaApi");
            var resposta = await client.GetAsync($"/api/interno/lens/processamentos/{comando.ProcessamentoId}", ct);

            if (!resposta.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Falha ao buscar dados do processamento {ProcessamentoId} na API. Status: {Status}.",
                    comando.ProcessamentoId, resposta.StatusCode);
                throw new InvalidOperationException(
                    $"Processamento {comando.ProcessamentoId} não encontrado na API.");
            }

            var json = await resposta.Content.ReadAsStringAsync(ct);
            var dados = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

            if (!dados.TryGetProperty("dados", out var dadosProcessamento))
            {
                throw new InvalidOperationException(
                    $"Resposta da API não contém dados do processamento {comando.ProcessamentoId}.");
            }

            await _publishEndpoint.Publish(new DocumentoLensProcessedEvent
            {
                CorrelacaoId = comando.CorrelacaoId,
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                EmpresaId = dadosProcessamento.GetProperty("empresaId").GetGuid(),
                Status = "Pendente",
                Mensagem = $"Reprocessamento iniciado. Motivo: {comando.MotivoReprocessamento ?? "Não informado"}."
            }, ct);

            var novoComando = new ProcessDocumentoLensCommand
            {
                CorrelacaoId = comando.CorrelacaoId,
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                EmpresaId = dadosProcessamento.GetProperty("empresaId").GetGuid(),
                ObraId = dadosProcessamento.TryGetProperty("obraId", out var obraId) && obraId.ValueKind != System.Text.Json.JsonValueKind.Null
                    ? obraId.GetGuid() : null,
                TipoDocumento = dadosProcessamento.GetProperty("tipoDocumento").GetString() ?? "",
                NomeArquivo = dadosProcessamento.GetProperty("nomeArquivo").GetString() ?? "",
                CaminhoArquivo = dadosProcessamento.GetProperty("caminhoArquivo").GetString() ?? "",
                ExtensaoArquivo = dadosProcessamento.GetProperty("extensaoArquivo").GetString() ?? "",
                TamanhoBytes = dadosProcessamento.GetProperty("tamanhoBytes").GetInt64()
            };

            await _publishEndpoint.Publish(novoComando, ct);

            await _notificacao.NotificarProcessamentoAtualizadoAsync(
                comando.UsuarioId,
                novoComando.ObraId,
                novoComando.EmpresaId,
                new
                {
                    comando.ProcessamentoId,
                    Status = "Pendente",
                    Mensagem = "Reprocessamento agendado com sucesso."
                }, ct);

            _logger.LogInformation(
                "Reprocessamento agendado. ProcessamentoId: {ProcessamentoId}.",
                comando.ProcessamentoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro no reprocessamento. ProcessamentoId: {ProcessamentoId}.",
                comando.ProcessamentoId);
            throw;
        }
    }
}
