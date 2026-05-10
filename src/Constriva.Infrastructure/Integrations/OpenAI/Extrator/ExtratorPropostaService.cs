using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Constriva.Application.Features.Agente.Settings;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Domain.ValueObjects.WhatsApp;
using Constriva.Infrastructure.Integrations.OpenAI.Extrator.Dtos;
using Constriva.Infrastructure.Integrations.OpenAI.Extrator.Prompts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Constriva.Infrastructure.Integrations.OpenAI.Extrator;

public class ExtratorPropostaService : IExtratorPropostaService
{
    private const string Modelo = "gpt-4o-mini";
    private const int MaxTokens = 4000;
    private const double Temperature = 0.1;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAISettings _settings;
    private readonly ILogger<ExtratorPropostaService> _logger;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ExtratorPropostaService(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAISettings> settings,
        ILogger<ExtratorPropostaService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<PropostaExtraidaValueObject> ExtrairAsync(
        EntradaExtratorValueObject entrada,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[EXTRATOR] Iniciando extração. Cotação: {NumeroCotacao} | Fornecedor: {NomeFornecedor} | " +
            "Tipo: {TipoConteudo} | RespostaId: {RespostaId}",
            entrada.NumeroCotacao, entrada.NomeFornecedor,
            entrada.TipoConteudo, entrada.RespostaFornecedorWhatsAppId);

        if (entrada.TipoConteudo == TipoConteudoMensagemEnum.Audio)
        {
            entrada = await ConverterAudioParaTextoAsync(entrada, cancellationToken);
        }

        var messages = MontarMensagens(entrada);

        var payload = new JsonObject
        {
            ["model"] = Modelo,
            ["max_tokens"] = MaxTokens,
            ["temperature"] = Temperature,
            ["messages"] = messages,
            ["response_format"] = new JsonObject { ["type"] = "json_object" }
        };

        var client = _httpClientFactory.CreateClient("OpenAI");
        var url = $"{_settings.BaseUrl.TrimEnd('/')}/v1/chat/completions";
        var httpContent = new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponse;
        try
        {
            httpResponse = await client.PostAsync(url, httpContent, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                "Timeout na chamada ao modelo de IA");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[EXTRATOR] Erro HTTP na chamada OpenAI. RespostaId: {RespostaId}",
                entrada.RespostaFornecedorWhatsAppId);
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                $"Erro na comunicação com OpenAI: {ex.Message}", ex);
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError(
                "[EXTRATOR] OpenAI retornou {StatusCode}. RespostaId: {RespostaId}",
                (int)httpResponse.StatusCode, entrada.RespostaFornecedorWhatsAppId);
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                $"OpenAI retornou HTTP {(int)httpResponse.StatusCode}");
        }

        var responseJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        using var responseDoc = JsonDocument.Parse(responseJson);
        var root = responseDoc.RootElement;

        var tokensConsumidos = 0;
        if (root.TryGetProperty("usage", out var usage) &&
            usage.TryGetProperty("total_tokens", out var tt))
            tokensConsumidos = tt.GetInt32();

        var firstChoice = root.GetProperty("choices")[0];
        var message = firstChoice.GetProperty("message");
        var content = message.TryGetProperty("content", out var cp) && cp.ValueKind != JsonValueKind.Null
            ? cp.GetString()
            : null;

        if (string.IsNullOrEmpty(content))
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                "OpenAI retornou conteúdo vazio na resposta");

        RespostaOpenAiExtracaoDto respostaDto;
        try
        {
            respostaDto = JsonSerializer.Deserialize<RespostaOpenAiExtracaoDto>(content, JsonOpts)
                ?? throw new ExtratorPropostaException(
                    MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                    "Desserialização retornou null");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex,
                "[EXTRATOR] JSON inválido do modelo. RespostaId: {RespostaId}",
                entrada.RespostaFornecedorWhatsAppId);
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                "Modelo retornou JSON inválido", ex);
        }

        if (respostaDto.NaoEProposta)
        {
            _logger.LogWarning(
                "[EXTRATOR] Não é proposta. Fornecedor: {Fornecedor} | Motivo: {Motivo} | RespostaId: {RespostaId}",
                entrada.NomeFornecedor, respostaDto.MotivoNaoEProposta,
                entrada.RespostaFornecedorWhatsAppId);

            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.RespostaNaoEProposta,
                respostaDto.MotivoNaoEProposta ?? "Conteúdo não identificado como proposta comercial",
                nivelConfiancaObtido: 0);
        }

        var itensExtraidos = MapearItens(respostaDto.Itens, entrada.ItensCotacao);

        var nivelConfianca = CalcularNivelConfianca(itensExtraidos, entrada.ItensCotacao, respostaDto);

        if (nivelConfianca < 30)
        {
            _logger.LogWarning(
                "[EXTRATOR] Confiança insuficiente: {NivelConfianca} | Cotação: {NumeroCotacao} | RespostaId: {RespostaId}",
                nivelConfianca, entrada.NumeroCotacao, entrada.RespostaFornecedorWhatsAppId);

            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ConfiancaInsuficiente,
                $"Nível de confiança {nivelConfianca} abaixo do mínimo de 30",
                nivelConfiancaObtido: nivelConfianca);
        }

        DateTime? validadeProposta = null;
        if (!string.IsNullOrEmpty(respostaDto.ValidadeProposta))
        {
            if (DateTime.TryParse(respostaDto.ValidadeProposta,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dv))
                validadeProposta = dv;
            else
                _logger.LogWarning("[EXTRATOR] Não foi possível parsear validade: {Validade}",
                    respostaDto.ValidadeProposta);
        }

        var resultado = new PropostaExtraidaValueObject
        {
            NivelConfianca = nivelConfianca,
            CondicoesPagamento = respostaDto.CondicoesPagamento,
            PrazoEntregaDias = respostaDto.PrazoEntregaDias,
            ValidadeProposta = validadeProposta,
            Observacoes = respostaDto.Observacoes,
            NaoEProposta = false,
            MotivoNaoEProposta = null,
            ItensExtraidos = itensExtraidos,
            TokensConsumidos = tokensConsumidos,
            ModeloUtilizado = Modelo
        };

        _logger.LogInformation(
            "[EXTRATOR] Extração concluída. Confiança: {Confianca} | Itens: {Total}/{Esperado} | " +
            "Tokens: {Tokens} | RespostaId: {RespostaId}",
            nivelConfianca, itensExtraidos.Count, entrada.ItensCotacao.Count,
            tokensConsumidos, entrada.RespostaFornecedorWhatsAppId);

        return resultado;
    }

    private JsonArray MontarMensagens(EntradaExtratorValueObject entrada)
    {
        var contexto = ExtratorPropostaPrompts.ConstruirContextoCotacao(
            entrada.NumeroCotacao, entrada.NomeFornecedor, entrada.ItensCotacao);

        var messages = new JsonArray
        {
            new JsonObject { ["role"] = "system", ["content"] = ExtratorPropostaPrompts.SystemPrompt }
        };

        switch (entrada.TipoConteudo)
        {
            case TipoConteudoMensagemEnum.Texto:
                messages.Add(new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = contexto + "\n\n" + entrada.TextoMensagem
                });
                break;

            case TipoConteudoMensagemEnum.Imagem:
                ValidarMidia(entrada);
                var base64Img = Convert.ToBase64String(entrada.ConteudoMidia!);
                var instrucaoImg = entrada.TextoMensagem != null
                    ? ExtratorPropostaPrompts.InstrucaoAnaliseTextoEImagem
                    : ExtratorPropostaPrompts.InstrucaoAnaliseImagem;
                var textoImg = contexto + "\n\n" + instrucaoImg +
                    (entrada.TextoMensagem != null ? "\n\nTEXTO DA MENSAGEM:\n" + entrada.TextoMensagem : "");

                messages.Add(new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = new JsonArray
                    {
                        new JsonObject { ["type"] = "text", ["text"] = textoImg },
                        new JsonObject
                        {
                            ["type"] = "image_url",
                            ["image_url"] = new JsonObject
                            {
                                ["url"] = $"data:{entrada.MimeTypeMidia};base64,{base64Img}",
                                ["detail"] = "high"
                            }
                        }
                    }
                });
                break;

            case TipoConteudoMensagemEnum.Documento:
                ValidarMidia(entrada);
                var base64Doc = Convert.ToBase64String(entrada.ConteudoMidia!);
                var textoPdf = contexto + "\n\n" + ExtratorPropostaPrompts.InstrucaoAnalisePdf +
                    (entrada.TextoMensagem != null ? "\n\nTEXTO ADICIONAL:\n" + entrada.TextoMensagem : "");

                messages.Add(new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = new JsonArray
                    {
                        new JsonObject { ["type"] = "text", ["text"] = textoPdf },
                        new JsonObject
                        {
                            ["type"] = "image_url",
                            ["image_url"] = new JsonObject
                            {
                                ["url"] = $"data:{entrada.MimeTypeMidia};base64,{base64Doc}",
                                ["detail"] = "high"
                            }
                        }
                    }
                });
                break;

            case TipoConteudoMensagemEnum.Audio:
                messages.Add(new JsonObject
                {
                    ["role"] = "user",
                    ["content"] = contexto + "\n\n" + entrada.TextoMensagem
                });
                break;

            default:
                throw new ExtratorPropostaException(
                    MotivoFalhaProcessamentoEnum.FormatoNaoSuportado,
                    $"Tipo de conteúdo '{entrada.TipoConteudo}' não é suportado para extração");
        }

        return messages;
    }

    private async Task<EntradaExtratorValueObject> ConverterAudioParaTextoAsync(
        EntradaExtratorValueObject entrada, CancellationToken ct)
    {
        ValidarMidia(entrada);

        var extensao = entrada.MimeTypeMidia?.ToLower() switch
        {
            "audio/ogg" => "ogg",
            "audio/ogg; codecs=opus" => "ogg",
            "audio/mpeg" => "mp3",
            "audio/mp4" => "m4a",
            "audio/wav" => "wav",
            "audio/webm" => "webm",
            _ => "ogg"
        };

        _logger.LogInformation(
            "[EXTRATOR] Transcrevendo áudio via Whisper. Tamanho: {Bytes} bytes | MimeType: {Mime} | RespostaId: {RespostaId}",
            entrada.ConteudoMidia!.Length, entrada.MimeTypeMidia, entrada.RespostaFornecedorWhatsAppId);

        var client = _httpClientFactory.CreateClient("OpenAI");
        var url = $"{_settings.BaseUrl.TrimEnd('/')}/v1/audio/transcriptions";

        using var form = new MultipartFormDataContent();
        var audioContent = new ByteArrayContent(entrada.ConteudoMidia);
        audioContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
            entrada.MimeTypeMidia ?? "audio/ogg");
        form.Add(audioContent, "file", $"audio.{extensao}");
        form.Add(new StringContent("whisper-1"), "model");
        form.Add(new StringContent("pt"), "language");

        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync(url, form, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[EXTRATOR] Erro HTTP na transcrição Whisper. RespostaId: {RespostaId}",
                entrada.RespostaFornecedorWhatsAppId);
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                $"Erro na transcrição do áudio: {ex.Message}", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError(
                "[EXTRATOR] Whisper retornou {StatusCode}. Erro: {Erro} | RespostaId: {RespostaId}",
                (int)response.StatusCode, errorBody, entrada.RespostaFornecedorWhatsAppId);
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.ErroApiOpenAI,
                $"Whisper retornou HTTP {(int)response.StatusCode}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);
        using var doc = JsonDocument.Parse(json);
        var transcricao = doc.RootElement.TryGetProperty("text", out var textProp)
            ? textProp.GetString()
            : null;

        if (string.IsNullOrWhiteSpace(transcricao))
        {
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.FormatoNaoSuportado,
                "Transcrição do áudio retornou vazia");
        }

        _logger.LogInformation(
            "[EXTRATOR] Transcrição concluída. Caracteres: {Len} | RespostaId: {RespostaId}",
            transcricao.Length, entrada.RespostaFornecedorWhatsAppId);

        return new EntradaExtratorValueObject
        {
            RespostaFornecedorWhatsAppId = entrada.RespostaFornecedorWhatsAppId,
            CotacaoId = entrada.CotacaoId,
            NumeroCotacao = entrada.NumeroCotacao,
            NomeFornecedor = entrada.NomeFornecedor,
            TipoConteudo = TipoConteudoMensagemEnum.Audio,
            TextoMensagem = $"[TRANSCRIÇÃO DE ÁUDIO]\n{transcricao}",
            ConteudoMidia = null,
            MimeTypeMidia = null,
            ItensCotacao = entrada.ItensCotacao
        };
    }

    private static void ValidarMidia(EntradaExtratorValueObject entrada)
    {
        if (entrada.ConteudoMidia is null || entrada.ConteudoMidia.Length == 0)
            throw new ExtratorPropostaException(
                MotivoFalhaProcessamentoEnum.MidiaInacessivel,
                "Conteúdo da mídia está vazio ou não foi baixado");
    }

    private static IReadOnlyList<ItemExtraidoValueObject> MapearItens(
        List<ItemExtracaoDto>? itensExtraidos,
        IReadOnlyList<ItemCotacaoReferenciaValueObject> itensCotacao)
    {
        if (itensExtraidos is null || itensExtraidos.Count == 0)
            return [];

        var idsValidos = itensCotacao.Select(i => i.ItemCotacaoId).ToHashSet();

        return itensExtraidos.Select(item =>
        {
            Guid? itemCotacaoId = null;
            var confianca = item.ConfiancaItem ?? 0;

            if (!string.IsNullOrEmpty(item.ItemCotacaoId) &&
                Guid.TryParse(item.ItemCotacaoId, out var parsedId) &&
                idsValidos.Contains(parsedId))
            {
                itemCotacaoId = parsedId;
            }
            else if (!string.IsNullOrEmpty(item.ItemCotacaoId))
            {
                confianca = Math.Min(confianca, 40);
            }

            return new ItemExtraidoValueObject
            {
                ItemCotacaoId = itemCotacaoId,
                DescricaoOriginal = item.DescricaoOriginal ?? "Sem descrição",
                PrecoUnitario = item.PrecoUnitario ?? 0,
                Quantidade = item.Quantidade ?? 0,
                UnidadeMedida = item.UnidadeMedida,
                Marca = item.Marca,
                Disponivel = item.Disponivel,
                ConfiancaItem = confianca,
                Observacao = item.Observacao
            };
        }).ToList();
    }

    private static int CalcularNivelConfianca(
        IReadOnlyList<ItemExtraidoValueObject> itensExtraidos,
        IReadOnlyList<ItemCotacaoReferenciaValueObject> itensCotacao,
        RespostaOpenAiExtracaoDto respostaDto)
    {
        if (itensCotacao.Count == 0)
            return 0;

        var itensComMapeamento = itensExtraidos.Count(i => i.ItemCotacaoId.HasValue);
        var percentualCobertura = (double)itensComMapeamento / itensCotacao.Count;
        var baseScore = (int)(percentualCobertura * 60);

        var bonus = 0;
        if (!string.IsNullOrEmpty(respostaDto.CondicoesPagamento))
            bonus += 15;
        if (respostaDto.PrazoEntregaDias.HasValue)
            bonus += 15;
        if (!string.IsNullOrEmpty(respostaDto.ValidadeProposta))
            bonus += 10;

        if (itensExtraidos.Count > 0)
        {
            var mediaConfianca = itensExtraidos.Average(x => x.ConfiancaItem);
            baseScore = (int)(baseScore * (mediaConfianca / 100.0));
        }

        return Math.Min(100, Math.Max(0, baseScore + bonus));
    }
}
