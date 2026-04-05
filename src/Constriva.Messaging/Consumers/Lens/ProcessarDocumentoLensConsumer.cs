using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.Lens.Commands;
using Constriva.Messaging.Contracts.Lens.Events;
using Constriva.Messaging.Models.Lens;
using Constriva.Messaging.Repositories.Lens;
using Constriva.Messaging.Services.Lens;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.Lens;

/// <summary>
/// Consumer responsável por processar documentos via Constriva.Lens (OCR).
/// </summary>
public class ProcessarDocumentoLensConsumer : IConsumer<ProcessDocumentoLensCommand>
{
    private readonly IConstrivaLensService _lensServico;
    private readonly ILogProcessamentoLensRepository _logRepositorio;
    private readonly ISignalRNotificationService _notificacao;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProcessarDocumentoLensConsumer> _logger;

    /// <summary>
    /// Inicializa o consumer de processamento de documentos Lens.
    /// </summary>
    public ProcessarDocumentoLensConsumer(
        IConstrivaLensService lensServico,
        ILogProcessamentoLensRepository logRepositorio,
        ISignalRNotificationService notificacao,
        IPublishEndpoint publishEndpoint,
        ILogger<ProcessarDocumentoLensConsumer> logger)
    {
        _lensServico = lensServico;
        _logRepositorio = logRepositorio;
        _notificacao = notificacao;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    /// <summary>
    /// Consome e processa o comando de processamento de documento OCR.
    /// </summary>
    public async Task Consume(ConsumeContext<ProcessDocumentoLensCommand> context)
    {
        var comando = context.Message;
        var ct = context.CancellationToken;

        _logger.LogInformation(
            "Iniciando processamento OCR. ProcessamentoId: {ProcessamentoId}, Arquivo: {Arquivo}, Tipo: {Tipo}.",
            comando.ProcessamentoId, comando.NomeArquivo, comando.TipoDocumento);

        // Notificar que o processamento está em andamento
        await _publishEndpoint.Publish(new DocumentoLensProcessedEvent
        {
            CorrelacaoId = comando.CorrelacaoId,
            ProcessamentoId = comando.ProcessamentoId,
            UsuarioId = comando.UsuarioId,
            ObraId = comando.ObraId,
            EmpresaId = comando.EmpresaId,
            Status = "Processando",
            Mensagem = "Documento enviado para processamento OCR."
        }, ct);

        await _notificacao.NotificarProcessamentoAtualizadoAsync(
            comando.UsuarioId, comando.ObraId, comando.EmpresaId,
            new { comando.ProcessamentoId, Status = "Processando", Mensagem = "Processamento OCR iniciado." }, ct);

        try
        {
            // Verificar se o arquivo existe
            if (!File.Exists(comando.CaminhoArquivo))
            {
                throw new FileNotFoundException(
                    $"Arquivo não encontrado no caminho: {comando.CaminhoArquivo}", comando.CaminhoArquivo);
            }

            // Processar documento via Constriva.Lens
            var resultado = await _lensServico.ProcessarDocumentoAsync(
                comando.CaminhoArquivo, comando.TipoDocumento, ct);

            // Salvar log no MongoDB
            var log = new LogProcessamentoLens
            {
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                EmpresaId = comando.EmpresaId,
                ObraId = comando.ObraId,
                TipoDocumento = resultado.TipoDocumento,
                TipoDocumentoDeclarado = resultado.TipoDocumentoDeclarado,
                TiposConferem = resultado.TiposConferem,
                Status = "Concluido",
                ConfidenceScore = resultado.ConfidenceScore,
                TextoBruto = resultado.TextoBruto,
                TotalItens = resultado.DadosExtraidos.HasValue ? 1 : 0,
                Warnings = resultado.Warnings,
                TempoProcessamentoMs = resultado.TempoProcessamentoMs,
                PaginasProcessadas = resultado.PaginasProcessadas,
                TamanhoArquivoBytes = comando.TamanhoBytes,
                ExtensaoArquivo = comando.ExtensaoArquivo,
                UrlLensUtilizada = "ConstrivaLens",
                TentativaNumero = 1
            };

            await _logRepositorio.InserirAsync(log, ct);

            // Publicar evento de conclusão
            var eventoConcluido = new DocumentoLensCompletedEvent
            {
                CorrelacaoId = comando.CorrelacaoId,
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                ObraId = comando.ObraId,
                EmpresaId = comando.EmpresaId,
                TipoDocumento = resultado.TipoDocumento,
                TipoDocumentoDeclarado = resultado.TipoDocumentoDeclarado,
                TiposConferem = resultado.TiposConferem,
                ConfidenceScore = resultado.ConfidenceScore,
                Warnings = resultado.Warnings,
                TempoProcessamentoMs = resultado.TempoProcessamentoMs,
                DadosExtraidosJson = resultado.DadosExtraidos.HasValue
                    ? resultado.DadosExtraidos.Value.GetRawText()
                    : null,
                MetodoExtracao = resultado.MetodoExtracao
            };

            await _publishEndpoint.Publish(eventoConcluido, ct);

            // Notificar via SignalR
            await _notificacao.NotificarProcessamentoConcluidoAsync(
                comando.UsuarioId, comando.ObraId, comando.EmpresaId,
                new
                {
                    comando.ProcessamentoId,
                    Status = "Concluido",
                    resultado.TipoDocumento,
                    resultado.TiposConferem,
                    resultado.ConfidenceScore,
                    resultado.Warnings,
                    resultado.TempoProcessamentoMs,
                    Mensagem = "Processamento OCR concluído com sucesso."
                }, ct);

            _logger.LogInformation(
                "Processamento OCR concluído. ProcessamentoId: {ProcessamentoId}, Confidence: {Confidence}.",
                comando.ProcessamentoId, resultado.ConfidenceScore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro no processamento OCR. ProcessamentoId: {ProcessamentoId}, Arquivo: {Arquivo}.",
                comando.ProcessamentoId, comando.NomeArquivo);

            // Salvar log de erro no MongoDB
            var logErro = new LogProcessamentoLens
            {
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                EmpresaId = comando.EmpresaId,
                ObraId = comando.ObraId,
                TipoDocumento = comando.TipoDocumento,
                TipoDocumentoDeclarado = comando.TipoDocumento,
                Status = "Erro",
                Erros = new List<string> { ex.Message },
                TamanhoArquivoBytes = comando.TamanhoBytes,
                ExtensaoArquivo = comando.ExtensaoArquivo,
                UrlLensUtilizada = "ConstrivaLens",
                TentativaNumero = 1
            };

            try
            {
                await _logRepositorio.InserirAsync(logErro, ct);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Falha ao salvar log de erro no MongoDB para ProcessamentoId: {ProcessamentoId}.", comando.ProcessamentoId);
            }

            // Publicar evento de erro
            var podeReprocessar = ex is not FileNotFoundException;
            await _publishEndpoint.Publish(new DocumentoLensErrorEvent
            {
                CorrelacaoId = comando.CorrelacaoId,
                ProcessamentoId = comando.ProcessamentoId,
                UsuarioId = comando.UsuarioId,
                ObraId = comando.ObraId,
                EmpresaId = comando.EmpresaId,
                CodigoErro = ex.GetType().Name,
                MensagemErro = ex.Message,
                PodeReprocessar = podeReprocessar
            }, ct);

            // Notificar erro via SignalR
            await _notificacao.NotificarProcessamentoErroAsync(
                comando.UsuarioId, comando.ObraId, comando.EmpresaId,
                new
                {
                    comando.ProcessamentoId,
                    Status = "Erro",
                    CodigoErro = ex.GetType().Name,
                    MensagemErro = "Ocorreu um erro durante o processamento OCR.",
                    PodeReprocessar = podeReprocessar,
                    Mensagem = "Erro no processamento OCR."
                }, ct);

            throw;
        }
    }
}
