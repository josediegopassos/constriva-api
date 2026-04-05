using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Services;
using Constriva.Application.Features.Agente.Settings;
using Constriva.Domain.Entities.Agente;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Services;

public class AgentService : IAgentService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAISettings _settings;
    private readonly IAgenteRepository _repo;
    private readonly IAgenteTokenService _tokenService;
    private readonly IConstrivaToolsService _toolsService;
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<AgentService> _logger;

    // System prompt estático — só muda a data
    private const string SystemPromptTemplate = """
        Você é o assistente IA do Constriva, sistema de gestão de obras.
        Módulos: clientes, compras, contratos, cronograma, estoque, financeiro, fornecedores, ged, obras, orcamento, qualidade, rh, sst.
        Regras: responda em PT-BR, seja objetivo, use apenas dados necessários, não invente dados.
        Data: {0}
        """;

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    public AgentService(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAISettings> settings,
        IAgenteRepository repo,
        IAgenteTokenService tokenService,
        IConstrivaToolsService toolsService,
        ICacheService cache,
        IUnitOfWork uow,
        ILogger<AgentService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _repo = repo;
        _tokenService = tokenService;
        _toolsService = toolsService;
        _cache = cache;
        _uow = uow;
        _logger = logger;
    }

    public async Task<ChatResponseDto> ChatAsync(Guid empresaId, Guid usuarioId, ChatRequestDto request, CancellationToken ct)
    {
        // 1. Validar cota — lança CotaExcedidaException se excedida
        await _tokenService.ValidarCotaAsync(empresaId, ct);

        // 2. Obter ou criar sessão
        AgenteSessao sessao;
        if (request.SessaoId.HasValue)
        {
            sessao = await _repo.GetSessaoByIdAsync(request.SessaoId.Value, empresaId, ct)
                ?? throw new KeyNotFoundException($"Sessão {request.SessaoId.Value} não encontrada.");
            sessao.AtualizadaEm = DateTime.UtcNow;
        }
        else
        {
            sessao = new AgenteSessao
            {
                EmpresaId = empresaId,
                UsuarioId = usuarioId,
                AtualizadaEm = DateTime.UtcNow,
                Ativa = true
            };
            await _repo.AddSessaoAsync(sessao, ct);
            await _uow.SaveChangesAsync(ct);
        }

        // 3. Montar mensagens com context windowing
        var messages = await BuildMessagesAsync(sessao.Id, request.Mensagem, ct);

        // 4. Salvar mensagem do usuário no histórico
        var userHistorico = new AgenteHistorico
        {
            EmpresaId = empresaId,
            SessaoId = sessao.Id,
            Role = RoleAgenteEnum.User,
            Conteudo = request.Mensagem,
            TokensInput = 0,
            TokensOutput = 0
        };
        await _repo.AddHistoricoAsync(userHistorico, ct);
        await _uow.SaveChangesAsync(ct);

        // 5. Loop de chamadas à OpenAI
        var client = _httpClientFactory.CreateClient("OpenAI");
        var tools = _toolsService.GetToolDefinitions();
        var totalTokensInput = 0;
        var totalTokensOutput = 0;
        string? assistantResponse = null;

        try
        {
            for (var iteration = 0; iteration < _settings.MaxIteracoesToolCall; iteration++)
            {
                var requestObj = BuildRequestPayload(messages, tools);
                var requestJson = requestObj.ToJsonString();

                _logger.LogInformation("[AGENTE] Iteração {Iter}, Mensagens: {Count}", iteration, messages.Count);
                _logger.LogDebug("[AGENTE] Request payload: {Json}", requestJson);

                var httpContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
                var url = $"{_settings.BaseUrl.TrimEnd('/')}/v1/chat/completions";
                var httpResponse = await client.PostAsync(url, httpContent, ct);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorBody = await httpResponse.Content.ReadAsStringAsync(ct);
                    throw new InvalidOperationException(ParseOpenAIError((int)httpResponse.StatusCode, errorBody));
                }

                var responseJson = await httpResponse.Content.ReadAsStringAsync(ct);
                using var responseDoc = JsonDocument.Parse(responseJson);
                var root = responseDoc.RootElement;

                // Acumular tokens de TODAS as iterações
                if (root.TryGetProperty("usage", out var usage))
                {
                    totalTokensInput += usage.TryGetProperty("prompt_tokens", out var pt) ? pt.GetInt32() : 0;
                    totalTokensOutput += usage.TryGetProperty("completion_tokens", out var cpt) ? cpt.GetInt32() : 0;
                }

                var firstChoice = root.GetProperty("choices")[0];
                var message = firstChoice.GetProperty("message");
                var finishReason = firstChoice.GetProperty("finish_reason").GetString();

                if (finishReason == "tool_calls")
                {
                    await ProcessToolCallsAsync(message, messages, empresaId, usuarioId, sessao.Id, ct);
                    continue;
                }

                // Resposta final
                assistantResponse = message.TryGetProperty("content", out var contentProp) && contentProp.ValueKind != JsonValueKind.Null
                    ? contentProp.GetString()
                    : "Desculpe, não consegui gerar uma resposta.";
                break;
            }
        }
        catch (InvalidOperationException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AGENTE] Erro no loop de chat para empresa {EmpresaId}", empresaId);
            assistantResponse = "Desculpe, ocorreu um erro ao processar sua solicitação. Tente novamente.";
        }

        assistantResponse ??= "Desculpe, o número máximo de iterações foi atingido.";

        // 6. Salvar resposta do assistente com tokens totais acumulados
        var assistantHistorico = new AgenteHistorico
        {
            EmpresaId = empresaId,
            SessaoId = sessao.Id,
            Role = RoleAgenteEnum.Assistant,
            Conteudo = assistantResponse,
            TokensInput = totalTokensInput,
            TokensOutput = totalTokensOutput
        };
        await _repo.AddHistoricoAsync(assistantHistorico, ct);
        await _uow.SaveChangesAsync(ct);

        // 7. Registrar consumo (tokens de TODAS as iterações)
        await _tokenService.RegistrarConsumoAsync(empresaId, usuarioId, totalTokensInput, totalTokensOutput, ct);

        // 8. Retornar resposta
        var dashboard = await _tokenService.ObterDashboardConsumoAsync(empresaId, ct);
        var consumo = dashboard.ConsumoMesAtual;

        return new ChatResponseDto(
            sessao.Id,
            assistantResponse,
            totalTokensInput + totalTokensOutput,
            consumo.TokensRestantes,
            consumo.PercentualUso);
    }

    /// <summary>
    /// Monta o array de mensagens com system prompt cacheado e context windowing.
    /// Limita o histórico para não estourar o contexto.
    /// </summary>
    private async Task<List<Dictionary<string, object>>> BuildMessagesAsync(Guid sessaoId, string novaMensagem, CancellationToken ct)
    {
        var messages = new List<Dictionary<string, object>>();

        // System prompt cacheado por dia (só muda a data)
        var hoje = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var systemPrompt = string.Format(SystemPromptTemplate, hoje);
        messages.Add(new Dictionary<string, object> { ["role"] = "system", ["content"] = systemPrompt });

        // Carregar histórico com limite
        var historicoMensagens = (await _repo.GetUltimasMensagensAsync(sessaoId, _settings.MaxMensagensHistorico, ct)).ToList();

        // Context windowing: estimar tokens e truncar se necessário
        var tokensEstimados = EstimarTokens(systemPrompt) + EstimarTokens(novaMensagem);
        var mensagensIncluidas = new List<AgenteHistorico>();

        // Iterar do mais recente ao mais antigo, incluir enquanto cabe no contexto
        for (var i = historicoMensagens.Count - 1; i >= 0; i--)
        {
            var msg = historicoMensagens[i];
            var tokensMsg = EstimarTokens(msg.Conteudo ?? "");
            if (tokensEstimados + tokensMsg > _settings.MaxTokensContexto)
                break;

            tokensEstimados += tokensMsg;
            mensagensIncluidas.Insert(0, msg);
        }

        // Adicionar histórico filtrado
        try
        {
            foreach (var msg in mensagensIncluidas)
            {
                if (msg.Role == RoleAgenteEnum.User && !string.IsNullOrEmpty(msg.Conteudo))
                    messages.Add(new Dictionary<string, object> { ["role"] = "user", ["content"] = msg.Conteudo });
                else if (msg.Role == RoleAgenteEnum.Assistant && string.IsNullOrEmpty(msg.ToolCallsJson) && !string.IsNullOrEmpty(msg.Conteudo))
                    messages.Add(new Dictionary<string, object> { ["role"] = "assistant", ["content"] = msg.Conteudo });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[AGENTE] Falha ao carregar histórico da sessão {SessaoId}.", sessaoId);
            messages.Clear();
            messages.Add(new Dictionary<string, object> { ["role"] = "system", ["content"] = systemPrompt });
        }

        // Nova mensagem do usuário
        messages.Add(new Dictionary<string, object> { ["role"] = "user", ["content"] = novaMensagem });

        return messages;
    }

    /// <summary>
    /// Monta o payload JSON da requisição para a OpenAI.
    /// </summary>
    private System.Text.Json.Nodes.JsonObject BuildRequestPayload(
        List<Dictionary<string, object>> messages,
        IReadOnlyList<object> tools)
    {
        var messagesArray = new System.Text.Json.Nodes.JsonArray();
        foreach (var msg in messages)
        {
            var node = new System.Text.Json.Nodes.JsonObject();
            foreach (var kv in msg)
            {
                if (kv.Value is JsonElement je)
                    node[kv.Key] = System.Text.Json.Nodes.JsonNode.Parse(je.GetRawText());
                else if (kv.Value is string s)
                    node[kv.Key] = s;
                else if (kv.Value != null)
                    node[kv.Key] = System.Text.Json.Nodes.JsonNode.Parse(JsonSerializer.Serialize(kv.Value));
            }
            messagesArray.Add(node);
        }

        return new System.Text.Json.Nodes.JsonObject
        {
            ["model"] = _settings.Model,
            ["max_tokens"] = _settings.MaxTokensResposta,
            ["temperature"] = _settings.Temperature,
            ["messages"] = messagesArray,
            ["tools"] = System.Text.Json.Nodes.JsonNode.Parse(JsonSerializer.Serialize(tools)),
            ["parallel_tool_calls"] = false
        };
    }

    /// <summary>
    /// Processa tool calls com cache de resultados para evitar chamadas repetidas.
    /// </summary>
    private async Task ProcessToolCallsAsync(
        JsonElement message,
        List<Dictionary<string, object>> messages,
        Guid empresaId, Guid usuarioId, Guid sessaoId,
        CancellationToken ct)
    {
        var toolCallsElement = message.GetProperty("tool_calls");
        var assistantContent = message.TryGetProperty("content", out var ac) && ac.ValueKind != JsonValueKind.Null
            ? ac.GetString() : null;

        // Adicionar mensagem do assistente com tool_calls ao contexto
        var assistantMsg = new Dictionary<string, object>
        {
            ["role"] = "assistant",
            ["tool_calls"] = JsonSerializer.Deserialize<JsonElement>(toolCallsElement.GetRawText())
        };
        if (assistantContent != null) assistantMsg["content"] = assistantContent;
        messages.Add(assistantMsg);

        // Salvar no histórico
        var toolCallsHistorico = new AgenteHistorico
        {
            EmpresaId = empresaId,
            SessaoId = sessaoId,
            Role = RoleAgenteEnum.Assistant,
            Conteudo = assistantContent ?? "",
            ToolCallsJson = toolCallsElement.GetRawText(),
            TokensInput = 0,
            TokensOutput = 0
        };
        await _repo.AddHistoricoAsync(toolCallsHistorico, ct);

        // Executar cada tool call
        foreach (var toolCall in toolCallsElement.EnumerateArray())
        {
            var toolCallId = toolCall.GetProperty("id").GetString()!;
            var function = toolCall.GetProperty("function");
            var toolName = function.GetProperty("name").GetString()!;
            var toolArgs = function.GetProperty("arguments").GetString()!;

            // Cache de resultados de tools — mesma chamada em 60s retorna cache
            var cacheKey = $"agent:tool:{empresaId}:{toolName}:{toolArgs.GetHashCode()}";
            var toolResult = await _cache.GetAsync<string>(cacheKey, ct);

            if (toolResult is null)
            {
                try
                {
                    toolResult = await _toolsService.ExecuteToolAsync(toolName, toolArgs, empresaId, usuarioId, ct);
                }
                catch (Exception ex)
                {
                    toolResult = $"Erro ao executar ferramenta '{toolName}': {ex.Message}";
                }

                // Cachear resultado por tempo configurável
                await _cache.SetAsync(cacheKey, toolResult, TimeSpan.FromSeconds(_settings.CacheToolResultSegundos), ct);
            }
            else
            {
                _logger.LogDebug("[AGENTE] Cache hit para tool {Tool}, args hash {Hash}", toolName, toolArgs.GetHashCode());
            }

            // Truncar resultado muito grande para economizar tokens
            if (toolResult.Length > 2000)
            {
                toolResult = toolResult[..2000] + "\n... (resultado truncado para economizar tokens)";
                _logger.LogInformation("[AGENTE] Resultado da tool {Tool} truncado de {Original} para 2000 chars.", toolName, toolResult.Length);
            }

            // Salvar no histórico
            var toolResultHistorico = new AgenteHistorico
            {
                EmpresaId = empresaId,
                SessaoId = sessaoId,
                Role = RoleAgenteEnum.Tool,
                Conteudo = toolResult,
                ToolCallsJson = JsonSerializer.Serialize(new ToolResultData { ToolCallId = toolCallId, ToolName = toolName }, _jsonOpts),
                TokensInput = 0,
                TokensOutput = 0
            };
            await _repo.AddHistoricoAsync(toolResultHistorico, ct);

            // Adicionar resultado ao contexto
            messages.Add(new Dictionary<string, object>
            {
                ["role"] = "tool",
                ["content"] = toolResult,
                ["tool_call_id"] = toolCallId
            });
        }

        await _uow.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Estimativa rápida de tokens (~4 chars por token para português).
    /// </summary>
    private static int EstimarTokens(string texto) => (texto.Length + 3) / 4;

    /// <summary>
    /// Extrai mensagem de erro amigável da resposta da OpenAI.
    /// </summary>
    private static string ParseOpenAIError(int statusCode, string errorBody)
    {
        var errorDetail = "";
        try
        {
            using var errDoc = JsonDocument.Parse(errorBody);
            if (errDoc.RootElement.TryGetProperty("error", out var errObj) &&
                errObj.TryGetProperty("message", out var errMsg))
                errorDetail = errMsg.GetString() ?? "";
        }
        catch { /* ignore */ }

        return statusCode switch
        {
            400 => $"Requisição inválida para a OpenAI: {errorDetail}",
            401 => "Chave da API OpenAI inválida ou não configurada.",
            429 => "Limite de requisições da OpenAI atingido. Tente novamente em alguns segundos.",
            500 or 502 or 503 => "Serviço da OpenAI temporariamente indisponível. Tente novamente.",
            _ => $"Erro na comunicação com a OpenAI (HTTP {statusCode}): {errorDetail}"
        };
    }

    private class ToolResultData
    {
        [JsonPropertyName("tool_call_id")]
        public string ToolCallId { get; set; } = "";
        [JsonPropertyName("tool_name")]
        public string ToolName { get; set; } = "";
    }
}
