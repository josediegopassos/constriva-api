using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        IUnitOfWork uow)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _repo = repo;
        _tokenService = tokenService;
        _toolsService = toolsService;
        _uow = uow;
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

        // 4. Build messages array
        var messages = new List<object>();

        // System prompt
        var systemPrompt = $"""
            Você é o assistente IA do Constriva, um sistema de gestão de obras e construção civil.

            Módulos disponíveis: clientes, compras, contratos, cronograma, empresas, estoque, financeiro, fornecedores, ged, obras, orcamento, qualidade, relatorios, rh, sst, usuarios.

            Responda sempre em português brasileiro. Seja objetivo e use apenas os dados necessários.

            Data atual: {DateTime.UtcNow:yyyy-MM-dd}
            """;

        messages.Add(new { role = "system", content = systemPrompt });

        // Loaded history
        foreach (var msg in historicoMensagens)
        {
            var role = msg.Role switch
            {
                RoleAgenteEnum.User => "user",
                RoleAgenteEnum.Assistant => "assistant",
                RoleAgenteEnum.Tool => "tool",
                RoleAgenteEnum.System => "system",
                _ => "user"
            };

            if (msg.Role == RoleAgenteEnum.Assistant && !string.IsNullOrEmpty(msg.ToolCallsJson))
            {
                // Assistant message with tool_calls
                var toolCalls = JsonSerializer.Deserialize<JsonElement>(msg.ToolCallsJson);
                messages.Add(new { role = "assistant", content = msg.Conteudo, tool_calls = toolCalls });
            }
            else if (msg.Role == RoleAgenteEnum.Tool)
            {
                // Tool response - parse to extract tool_call_id
                var toolData = JsonSerializer.Deserialize<ToolResultData>(msg.ToolCallsJson ?? "{}", _jsonOpts);
                messages.Add(new { role = "tool", content = msg.Conteudo, tool_call_id = toolData?.ToolCallId ?? "" });
            }
            else
            {
                messages.Add(new { role, content = msg.Conteudo });
            }
        }

        // New user message
        messages.Add(new { role = "user", content = request.Mensagem });

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

        for (var iteration = 0; iteration < 5; iteration++)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
                max_tokens = 800,
                messages,
                tools
            };

            var requestJson = JsonSerializer.Serialize(requestBody, _jsonOpts);
            var httpContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

            var url = $"{_settings.BaseUrl.TrimEnd('/')}/v1/chat/completions";
            var httpResponse = await client.PostAsync(url, httpContent, ct);
            httpResponse.EnsureSuccessStatusCode();

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
                var assistantMsg = JsonSerializer.Deserialize<JsonElement>(
                    JsonSerializer.Serialize(new
                    {
                        role = "assistant",
                        content = assistantContent,
                        tool_calls = JsonSerializer.Deserialize<JsonElement>(toolCallsElement.GetRawText())
                    }, _jsonOpts));
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
                    messages.Add(new { role = "tool", content = toolResult, tool_call_id = toolCallId });
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
        public string ToolCallId { get; set; } = "";
        public string ToolName { get; set; } = "";
    }
}
