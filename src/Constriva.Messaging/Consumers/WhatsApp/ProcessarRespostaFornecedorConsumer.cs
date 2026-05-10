using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Domain.ValueObjects.WhatsApp;
using Constriva.Infrastructure.Integrations.OpenAI.Extrator;
using Constriva.Infrastructure.Integrations.WhatsApp;
using Constriva.Infrastructure.Persistence;
using Constriva.Messaging.Contracts.WhatsApp.Commands;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class ProcessarRespostaFornecedorConsumer : IConsumer<ProcessarRespostaFornecedorCommand>
{
    private readonly IWhatsAppGateway _gateway;
    private readonly IExtratorPropostaService _extrator;
    private readonly AppDbContext _db;
    private readonly IFileStorageService _storage;
    private readonly IPublishEndpoint _publish;
    private readonly ILogger<ProcessarRespostaFornecedorConsumer> _logger;

    public ProcessarRespostaFornecedorConsumer(
        IWhatsAppGateway gateway, IExtratorPropostaService extrator,
        AppDbContext db, IFileStorageService storage,
        IPublishEndpoint publish, ILogger<ProcessarRespostaFornecedorConsumer> logger)
    {
        _gateway = gateway;
        _extrator = extrator;
        _db = db;
        _storage = storage;
        _publish = publish;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessarRespostaFornecedorCommand> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var resposta = await _db.RespostasFornecedorWhatsApp
            .FirstOrDefaultAsync(r => r.WaMessageId == msg.WaMessageId &&
                r.EmpresaId == msg.EmpresaId && !r.IsDeleted, ct)
            ?? throw new InvalidOperationException(
                $"RespostaFornecedorWhatsApp não encontrada para WaMessageId {msg.WaMessageId}");

        var cotacaoWa = await _db.CotacoesWhatsApp
            .FirstAsync(c => c.Id == resposta.CotacaoWhatsAppId, ct);

        var itensCotacao = await _db.ItensCotacao
            .Where(i => i.CotacaoId == cotacaoWa.CotacaoId && !i.IsDeleted)
            .AsNoTracking().ToListAsync(ct);

        var cotacao = await _db.Cotacoes
            .FirstAsync(c => c.Id == cotacaoWa.CotacaoId, ct);

        var mensagemOriginal = await _db.MensagensWhatsApp
            .FirstOrDefaultAsync(m => m.FornecedorCotacaoId == resposta.FornecedorCotacaoId &&
                m.CotacaoWhatsAppId == cotacaoWa.Id && !m.IsDeleted, ct);

        var nomeFornecedor = mensagemOriginal?.NomeFornecedor ?? "Fornecedor";

        await _publish.Publish(new RespostaFornecedorRecebidaEvent
        {
            CotacaoId = cotacaoWa.CotacaoId,
            FornecedorCotacaoId = resposta.FornecedorCotacaoId,
            FornecedorId = resposta.FornecedorId,
            EmpresaId = msg.EmpresaId,
            NomeFornecedor = nomeFornecedor,
            RecebidaEm = resposta.RecebidaEm,
            TipoConteudo = msg.TipoConteudo,
            WaMessageId = msg.WaMessageId
        }, ct);

        byte[]? conteudoMidia = null;

        if (resposta.TipoConteudo != TipoConteudoMensagemEnum.Texto &&
            !string.IsNullOrEmpty(resposta.WaMediaId))
        {
            try
            {
                _logger.LogInformation("Baixando mídia {MediaId}. Tipo: {Tipo}",
                    resposta.WaMediaId, resposta.MediaMimeType);

                var mediaInfo = await _gateway.ObterInfoMediaAsync(resposta.WaMediaId, ct);
                conteudoMidia = await _gateway.DownloadMediaAsync(mediaInfo.Url, ct);

                var nomeArquivo = resposta.MediaNomeArquivo ?? $"{resposta.WaMediaId}.bin";
                var path = $"whatsapp/{msg.EmpresaId}/respostas/{resposta.Id}/{nomeArquivo}";

                using var stream = new MemoryStream(conteudoMidia);
                await _storage.UploadAsync(stream, path,
                    resposta.MediaMimeType ?? "application/octet-stream", "whatsapp", ct);

                resposta.RegistrarMediaArmazenada(path);
                await _db.SaveChangesAsync(ct);

                _logger.LogInformation("Mídia salva: {Bytes} bytes em {Path}", conteudoMidia.Length, path);
            }
            catch (WhatsAppGatewayException ex)
            {
                _logger.LogError(ex, "Falha ao baixar mídia {MediaId}", resposta.WaMediaId);
                await PublicarFalhaAsync(resposta, cotacaoWa,
                    MotivoFalhaProcessamentoEnum.MidiaInacessivel,
                    $"Não foi possível baixar a mídia: {ex.Message}", null, ct);
                return;
            }
        }

        try
        {
            _logger.LogInformation("Iniciando extração IA. CotacaoId: {CotacaoId} | FornecedorId: {FornecedorId}",
                cotacaoWa.CotacaoId, resposta.FornecedorId);

            var entrada = new EntradaExtratorValueObject
            {
                RespostaFornecedorWhatsAppId = resposta.Id,
                CotacaoId = cotacaoWa.CotacaoId,
                NumeroCotacao = cotacao.Numero,
                NomeFornecedor = nomeFornecedor,
                TipoConteudo = resposta.TipoConteudo,
                TextoMensagem = resposta.TextoMensagem,
                ConteudoMidia = conteudoMidia,
                MimeTypeMidia = resposta.MediaMimeType,
                ItensCotacao = itensCotacao.Select(i => new ItemCotacaoReferenciaValueObject
                {
                    ItemCotacaoId = i.Id, Descricao = i.Descricao,
                    UnidadeMedida = i.UnidadeMedida, Quantidade = i.Quantidade,
                    Especificacao = i.Especificacao, PrecoReferencia = i.PrecoReferencia
                }).ToList()
            };

            var resultado = await _extrator.ExtrairAsync(entrada, ct);

            _logger.LogInformation("Extração concluída. Confiança: {C} | Itens: {I} | Tokens: {T}",
                resultado.NivelConfianca, resultado.ItensExtraidos.Count, resultado.TokensConsumidos);

            var proposta = new Domain.Entities.Compras.PropostaCotacao
            {
                EmpresaId = msg.EmpresaId, CotacaoId = cotacaoWa.CotacaoId,
                FornecedorId = resposta.FornecedorId, DataRecebimento = resposta.RecebidaEm,
                DataValidade = resultado.ValidadeProposta, CondicoesPagamento = resultado.CondicoesPagamento,
                PrazoEntrega = resultado.PrazoEntregaDias, Observacoes = resultado.Observacoes,
                ValorTotal = resultado.ItensExtraidos.Sum(i => i.PrecoUnitario * i.Quantidade),
                Vencedora = false
            };

            proposta.Itens = resultado.ItensExtraidos
                .Where(i => i.ItemCotacaoId.HasValue)
                .Select(i => new Domain.Entities.Compras.ItemPropostaCotacao
                {
                    EmpresaId = msg.EmpresaId, PropostaId = proposta.Id,
                    ItemCotacaoId = i.ItemCotacaoId!.Value, PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade, Marca = i.Marca,
                    Disponivel = i.Disponivel ?? true, MenorPreco = false
                }).ToList();

            _db.PropostasCotacao.Add(proposta);
            resposta.MarcarComoExtraidaComSucesso(proposta.Id, resultado.NivelConfianca);
            cotacaoWa.IncrementarPropostasExtraidas();
            await _db.SaveChangesAsync(ct);

            await _publish.Publish(new PropostaExtraidaComSucessoEvent
            {
                CotacaoId = cotacaoWa.CotacaoId, FornecedorCotacaoId = resposta.FornecedorCotacaoId,
                FornecedorId = resposta.FornecedorId, EmpresaId = msg.EmpresaId,
                WaMessageId = msg.WaMessageId, ExtraidaEm = DateTime.UtcNow,
                NivelConfianca = resultado.NivelConfianca,
                CondicoesPagamento = resultado.CondicoesPagamento,
                PrazoEntregaDias = resultado.PrazoEntregaDias,
                ValidadeProposta = resultado.ValidadeProposta,
                Observacoes = resultado.Observacoes,
                ItensExtraidos = resultado.ItensExtraidos.Select(i =>
                    new Contracts.WhatsApp.Events.ItemPropostaExtraidoDto
                    {
                        ItemCotacaoId = i.ItemCotacaoId ?? Guid.Empty,
                        DescricaoOriginal = i.DescricaoOriginal, PrecoUnitario = i.PrecoUnitario,
                        Quantidade = i.Quantidade, Marca = i.Marca,
                        Disponivel = i.Disponivel ?? true, Observacao = i.Observacao
                    }).ToList()
            }, ct);
        }
        catch (ExtratorPropostaException ex)
        {
            _logger.LogWarning(ex, "Extração falhou. Motivo: {Motivo} | Confiança: {C}",
                ex.Motivo, ex.NivelConfiancaObtido);

            resposta.MarcarComoFalhaNaExtracao(ex.Motivo, ex.Message, ex.NivelConfiancaObtido);
            await _db.SaveChangesAsync(ct);

            await PublicarFalhaAsync(resposta, cotacaoWa, ex.Motivo, ex.Message,
                ex.NivelConfiancaObtido, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Erro inesperado. WaMessageId: {W}", msg.WaMessageId);
            throw;
        }
    }

    private async Task PublicarFalhaAsync(
        Domain.Entities.WhatsApp.RespostaFornecedorWhatsApp resposta,
        Domain.Entities.WhatsApp.CotacaoWhatsApp cotacaoWa,
        MotivoFalhaProcessamentoEnum motivo, string descricao,
        int? nivelConfianca, CancellationToken ct)
    {
        await _publish.Publish(new PropostaExtraidaComFalhaEvent
        {
            CotacaoId = cotacaoWa.CotacaoId,
            FornecedorCotacaoId = resposta.FornecedorCotacaoId,
            FornecedorId = resposta.FornecedorId,
            EmpresaId = resposta.EmpresaId,
            WaMessageId = resposta.WaMessageId,
            FalhouEm = DateTime.UtcNow,
            Motivo = (MotivoFalhaExtracao)(int)motivo,
            DescricaoFalha = descricao,
            MensagemParaGestor = "A resposta do fornecedor não pôde ser processada automaticamente. Revise manualmente.",
            NivelConfiancaObtido = nivelConfianca,
            RequerIntervencaoManual = true
        }, ct);
    }
}
