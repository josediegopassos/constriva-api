using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly IUnitOfWork _uow;
    private readonly Microsoft.Extensions.Logging.ILogger<AgentService> _logger;

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
        IUnitOfWork uow,
        Microsoft.Extensions.Logging.ILogger<AgentService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _repo = repo;
        _tokenService = tokenService;
        _toolsService = toolsService;
        _uow = uow;
        _logger = logger;
    }

    public async Task<ChatResponseDto> ChatAsync(Guid empresaId, Guid usuarioId, ChatRequestDto request, CancellationToken ct)
    {
        // 1. Validate quota - throws CotaExcedidaException if over quota
        await _tokenService.ValidarCotaAsync(empresaId, ct);

        // 2. Get or create session
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

        // 3. Load last 10 messages from session
        var historicoMensagens = (await _repo.GetUltimasMensagensAsync(sessao.Id, 10, ct)).ToList();

        // 4. Build messages array (all Dictionary for consistent serialization)
        var messages = new List<Dictionary<string, object>>();

        // System prompt
        var systemPrompt = $"""
            Você é o assistente IA do Constriva, um sistema de gestão de obras e construção civil.

            Módulos disponíveis: clientes, compras, contratos, cronograma, empresas, estoque, financeiro, fornecedores, ged, obras, orcamento, qualidade, relatorios, rh, sst, usuarios.

            Responda sempre em português brasileiro. Seja objetivo e use apenas os dados necessários.

            Data atual: {DateTime.UtcNow:yyyy-MM-dd}
            """;

        messages.Add(new Dictionary<string, object> { ["role"] = "system", ["content"] = systemPrompt });

        // Loaded history — resiliente: se falhar, continua sem histórico
        try
        {
            foreach (var msg in historicoMensagens)
            {
                if (msg.Role == RoleAgenteEnum.User && !string.IsNullOrEmpty(msg.Conteudo))
                    messages.Add(new Dictionary<string, object> { ["role"] = "user", ["content"] = msg.Conteudo });
                else if (msg.Role == RoleAgenteEnum.Assistant && string.IsNullOrEmpty(msg.ToolCallsJson) && !string.IsNullOrEmpty(msg.Conteudo))
                    messages.Add(new Dictionary<string, object> { ["role"] = "assistant", ["content"] = msg.Conteudo });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[AGENTE] Falha ao carregar histórico da sessão {SessaoId}. Continuando sem histórico.", sessao.Id);
            messages.Clear();
            messages.Add(new Dictionary<string, object> { ["role"] = "system", ["content"] = systemPrompt });
        }

        // New user message
        messages.Add(new Dictionary<string, object> { ["role"] = "user", ["content"] = request.Mensagem });

        // 5. Save user message to AgenteHistorico
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

        // 6. Call OpenAI API in a loop (max 5 iterations)
        var client = _httpClientFactory.CreateClient("OpenAI");
        var tools = _toolsService.GetToolDefinitions();
        var totalTokensInput = 0;
        var totalTokensOutput = 0;
        string? assistantResponse = null;

        try
        {
        for (var iteration = 0; iteration < 5; iteration++)
        {
            // Build JSON manually to avoid naming policy mangling dictionary keys
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

            var requestObj = new System.Text.Json.Nodes.JsonObject
            {
                ["model"] = "gpt-4o-mini",
                ["max_tokens"] = 800,
                ["messages"] = messagesArray,
                ["tools"] = System.Text.Json.Nodes.JsonNode.Parse(JsonSerializer.Serialize(tools)),
                ["parallel_tool_calls"] = false
            };

            var requestJson = requestObj.ToJsonString();
            _logger.LogInformation("[AGENTE] Iteration {Iter}, Messages count: {Count}", iteration, messages.Count);
            _logger.LogDebug("[AGENTE] Request JSON: {Json}", requestJson);
            var httpContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

            var url = $"{_settings.BaseUrl.TrimEnd('/')}/v1/chat/completions";
            var httpResponse = await client.PostAsync(url, httpContent, ct);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorBody = await httpResponse.Content.ReadAsStringAsync(ct);
                var statusCode = (int)httpResponse.StatusCode;
                var errorDetail = "";
                try
                {
                    using var errDoc = JsonDocument.Parse(errorBody);
                    if (errDoc.RootElement.TryGetProperty("error", out var errObj) &&
                        errObj.TryGetProperty("message", out var errMsg))
                        errorDetail = errMsg.GetString() ?? "";
                }
                catch { /* ignore parse errors */ }

                var msg = statusCode switch
                {
                    400 => $"Requisição inválida para a OpenAI: {errorDetail}",
                    401 => "Chave da API OpenAI inválida ou não configurada.",
                    429 => "Limite de requisições da OpenAI atingido. Tente novamente em alguns segundos.",
                    500 or 502 or 503 => "Serviço da OpenAI temporariamente indisponível. Tente novamente.",
                    _ => $"Erro na comunicação com a OpenAI (HTTP {statusCode}): {errorDetail}"
                };
                throw new InvalidOperationException(msg);
            }

            var responseJson = await httpResponse.Content.ReadAsStringAsync(ct);
            using var responseDoc = JsonDocument.Parse(responseJson);
            var root = responseDoc.RootElement;

            // Accumulate usage tokens
            if (root.TryGetProperty("usage", out var usage))
            {
                totalTokensInput += usage.TryGetProperty("prompt_tokens", out var pt) ? pt.GetInt32() : 0;
                totalTokensOutput += usage.TryGetProperty("completion_tokens", out var cpt) ? cpt.GetInt32() : 0;
            }

            var choices = root.GetProperty("choices");
            var firstChoice = choices[0];
            var message = firstChoice.GetProperty("message");
            var finishReason = firstChoice.GetProperty("finish_reason").GetString();

            if (finishReason == "tool_calls")
            {
                // Process tool calls
                var toolCallsElement = message.GetProperty("tool_calls");
                var assistantContent = message.TryGetProperty("content", out var ac) && ac.ValueKind != JsonValueKind.Null
                    ? ac.GetString() : null;

                // Add the assistant message with tool_calls to the messages
                var assistantMsg = new Dictionary<string, object>
                {
                    ["role"] = "assistant",
                    ["tool_calls"] = JsonSerializer.Deserialize<JsonElement>(toolCallsElement.GetRawText())
                };
                if (assistantContent != null) assistantMsg["content"] = assistantContent;
                messages.Add(assistantMsg);

                // Save assistant tool_calls message to historico
                var toolCallsHistorico = new AgenteHistorico
                {
                    EmpresaId = empresaId,
                    SessaoId = sessao.Id,
                    Role = RoleAgenteEnum.Assistant,
                    Conteudo = assistantContent ?? "",
                    ToolCallsJson = toolCallsElement.GetRawText(),
                    TokensInput = 0,
                    TokensOutput = 0
                };
                await _repo.AddHistoricoAsync(toolCallsHistorico, ct);

                // Execute each tool call
                foreach (var toolCall in toolCallsElement.EnumerateArray())
                {
                    var toolCallId = toolCall.GetProperty("id").GetString()!;
                    var function = toolCall.GetProperty("function");
                    var toolName = function.GetProperty("name").GetString()!;
                    var toolArgs = function.GetProperty("arguments").GetString()!;

                    string toolResult;
                    try
                    {
                        toolResult = await _toolsService.ExecuteToolAsync(toolName, toolArgs, empresaId, usuarioId, ct);
                    }
                    catch (Exception ex)
                    {
                        toolResult = $"Erro ao executar ferramenta '{toolName}': {ex.Message}";
                    }

                    // Save tool result to historico
                    var toolResultHistorico = new AgenteHistorico
                    {
                        EmpresaId = empresaId,
                        SessaoId = sessao.Id,
                        Role = RoleAgenteEnum.Tool,
                        Conteudo = toolResult,
                        ToolCallsJson = JsonSerializer.Serialize(new ToolResultData { ToolCallId = toolCallId, ToolName = toolName }, _jsonOpts),
                        TokensInput = 0,
                        TokensOutput = 0
                    };
                    await _repo.AddHistoricoAsync(toolResultHistorico, ct);

                    // Add tool result to messages
                    messages.Add(new Dictionary<string, object>
                    {
                        ["role"] = "tool",
                        ["content"] = toolResult,
                        ["tool_call_id"] = toolCallId
                    });
                }

                await _uow.SaveChangesAsync(ct);
                continue; // Next iteration of the loop
            }

            // finish_reason == "stop" or other - break
            assistantResponse = message.TryGetProperty("content", out var contentProp) && contentProp.ValueKind != JsonValueKind.Null
                ? contentProp.GetString()
                : "Desculpe, não consegui gerar uma resposta.";
            break;
        }

        } // end try
        catch (InvalidOperationException)
        {
            throw; // erros de cota e OpenAI já tratados pelo HandleException
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AGENTE] Erro no loop de chat para empresa {EmpresaId}", empresaId);
            assistantResponse = "Desculpe, ocorreu um erro ao processar sua solicitação. Tente novamente.";
        }

        assistantResponse ??= "Desculpe, o número máximo de iterações foi atingido.";

        // 7. Save assistant response to AgenteHistorico
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

        // 8. Register consumption
        await _tokenService.RegistrarConsumoAsync(empresaId, usuarioId, totalTokensInput, totalTokensOutput, ct);

        // 9. Get updated quota info
        var dashboard = await _tokenService.ObterDashboardConsumoAsync(empresaId, ct);
        var consumo = dashboard.ConsumoMesAtual;

        // 10. Return ChatResponseDto
        return new ChatResponseDto(
            sessao.Id,
            assistantResponse,
            totalTokensInput + totalTokensOutput,
            consumo.TokensRestantes,
            consumo.PercentualUso);
    }

    private class ToolResultData
    {
        [JsonPropertyName("tool_call_id")]
        public string ToolCallId { get; set; } = "";
        [JsonPropertyName("tool_name")]
        public string ToolName { get; set; } = "";
    }
}
