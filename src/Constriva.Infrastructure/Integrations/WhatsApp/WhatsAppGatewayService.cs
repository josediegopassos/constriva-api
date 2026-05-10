using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Domain.ValueObjects.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;
using Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Outbound;
using Constriva.Infrastructure.Integrations.WhatsApp.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Constriva.Infrastructure.Integrations.WhatsApp;

public class WhatsAppGatewayService : IWhatsAppGateway
{
    private readonly HttpClient _httpClient;
    private readonly WhatsAppOptions _options;
    private readonly ILogger<WhatsAppGatewayService> _logger;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public WhatsAppGatewayService(
        IHttpClientFactory httpClientFactory,
        IOptions<WhatsAppOptions> options,
        ILogger<WhatsAppGatewayService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("WhatsApp");
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> EnviarConviteCotacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        string tituloCotacao,
        DateTime dataLimiteResposta,
        string urlFormulario,
        CancellationToken cancellationToken = default)
    {
        var request = CriarTemplateRequest(
            telefoneDestino,
            _options.TemplateConviteCotacao,
            nomeFornecedor,
            numeroCotacao,
            tituloCotacao,
            dataLimiteResposta.ToString("dd/MM/yyyy HH:mm"),
            urlFormulario);

        _logger.LogInformation(
            "[WHATSAPP] Enviando convite cotação {Template} para {Telefone} [Cotação: {Numero}]",
            _options.TemplateConviteCotacao, RedactarTelefone(telefoneDestino), numeroCotacao);

        return await EnviarMensagemAsync(request, cancellationToken);
    }

    public async Task<string> EnviarLembreteCotacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        DateTime dataLimiteResposta,
        int numeroTentativa,
        CancellationToken cancellationToken = default)
    {
        var request = CriarTemplateRequest(
            telefoneDestino,
            _options.TemplateLembreteCotacao,
            nomeFornecedor,
            numeroCotacao,
            dataLimiteResposta.ToString("dd/MM/yyyy HH:mm"),
            numeroTentativa.ToString());

        _logger.LogInformation(
            "[WHATSAPP] Enviando lembrete #{Tentativa} para {Telefone} [Cotação: {Numero}]",
            numeroTentativa, RedactarTelefone(telefoneDestino), numeroCotacao);

        return await EnviarMensagemAsync(request, cancellationToken);
    }

    public async Task<string> EnviarConfirmacaoAprovacaoAsync(
        string telefoneDestino,
        string nomeFornecedor,
        string numeroCotacao,
        decimal valorTotal,
        int? prazoEntregaDias,
        CancellationToken cancellationToken = default)
    {
        var valorFormatado = valorTotal.ToString("C2", new CultureInfo("pt-BR"));
        var prazoFormatado = prazoEntregaDias.HasValue ? $"{prazoEntregaDias} dias úteis" : "a combinar";

        var request = CriarTemplateRequest(
            telefoneDestino,
            _options.TemplateConfirmacaoAprovacao,
            nomeFornecedor,
            numeroCotacao,
            valorFormatado,
            prazoFormatado);

        _logger.LogInformation(
            "[WHATSAPP] Enviando confirmação aprovação para {Telefone} [Cotação: {Numero}]",
            RedactarTelefone(telefoneDestino), numeroCotacao);

        return await EnviarMensagemAsync(request, cancellationToken);
    }

    public async Task<string> EnviarTextoLivreAsync(
        string telefoneDestino,
        string texto,
        string? contextoWaMessageId = null,
        CancellationToken cancellationToken = default)
    {
        var request = new EnviarMensagemTextRequest
        {
            To = NormalizarTelefone(telefoneDestino),
            Text = new TextBody { Body = texto },
            Context = contextoWaMessageId != null ? new MessageContext { MessageId = contextoWaMessageId } : null
        };

        _logger.LogInformation(
            "[WHATSAPP] Enviando texto livre para {Telefone}",
            RedactarTelefone(telefoneDestino));

        var json = JsonSerializer.Serialize(request, JsonOpts);
        return await PostMensagemAsync(json, cancellationToken);
    }

    public async Task<MediaInfoValueObject> ObterInfoMediaAsync(
        string waMediaId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[WHATSAPP] Obtendo info da mídia {MediaId}", waMediaId);

        var url = _options.MediaEndpoint(waMediaId);
        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("[WHATSAPP] Falha ao obter info da mídia {MediaId}. Status: {Status}", waMediaId, response.StatusCode);
            throw new WhatsAppGatewayException(
                $"Falha ao obter info da mídia {waMediaId}",
                (int)response.StatusCode, errorBody);
        }

        var info = await response.Content.ReadFromJsonAsync<MediaInfoResponse>(JsonOpts, cancellationToken)
            ?? throw new WhatsAppGatewayException("Meta retornou resposta vazia para info da mídia.");

        return new MediaInfoValueObject
        {
            WaMediaId = waMediaId,
            Url = info.Url ?? throw new WhatsAppGatewayException("Meta não retornou URL da mídia."),
            MimeType = info.MimeType ?? "application/octet-stream",
            TamanhoBytes = info.FileSize ?? 0,
            NomeArquivo = null
        };
    }

    public async Task<byte[]> DownloadMediaAsync(
        string mediaUrl,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[WHATSAPP] Iniciando download de mídia");

        using var request = new HttpRequestMessage(HttpMethod.Get, mediaUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.AccessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("[WHATSAPP] Falha no download de mídia. Status: {Status}", response.StatusCode);
            throw new WhatsAppGatewayException(
                "Falha ao fazer download de mídia do Meta",
                (int)response.StatusCode, errorBody);
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        _logger.LogInformation("[WHATSAPP] Download concluído. Tamanho: {Bytes} bytes", bytes.Length);
        return bytes;
    }

    public async Task MarcarMensagemComoLidaAsync(
        string waMessageId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new MarcarComoLidaRequest { MessageId = waMessageId };
            var json = JsonSerializer.Serialize(request, JsonOpts);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(_options.MessagesEndpoint, content, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "[WHATSAPP] Falha ao marcar mensagem {WaMessageId} como lida",
                waMessageId);
        }
    }

    public bool ValidarAssinaturaWebhook(string payloadRaw, string assinaturaRecebida)
    {
        try
        {
            var receivedHash = assinaturaRecebida.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase)
                ? assinaturaRecebida["sha256=".Length..]
                : assinaturaRecebida;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.AppSecret));
            var computedBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadRaw));
            var receivedBytes = Convert.FromHexString(receivedHash);

            return CryptographicOperations.FixedTimeEquals(computedBytes, receivedBytes);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[WHATSAPP] Falha na validação de assinatura do webhook");
            return false;
        }
    }

    public bool ValidarTokenVerificacao(string mode, string token, string challenge, out string challengeResponse)
    {
        if (mode == "subscribe" && token == _options.VerifyToken)
        {
            _logger.LogInformation("[WHATSAPP] Token de verificação do webhook validado com sucesso");
            challengeResponse = challenge;
            return true;
        }

        _logger.LogWarning("[WHATSAPP] Token de verificação inválido. Mode: {Mode}", mode);
        challengeResponse = string.Empty;
        return false;
    }

    private EnviarTemplateRequest CriarTemplateRequest(string telefone, string templateName, params string[] parametros)
    {
        var parameters = parametros
            .Select(p => new TemplateParameter { Type = "text", Text = p })
            .ToList();

        return new EnviarTemplateRequest
        {
            To = NormalizarTelefone(telefone),
            Template = new TemplateBody
            {
                Name = templateName,
                Language = new TemplateLanguage { Code = _options.TemplateIdioma },
                Components = [new TemplateComponent { Type = "body", Parameters = parameters }]
            }
        };
    }

    private async Task<string> EnviarMensagemAsync(EnviarTemplateRequest request, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(request, JsonOpts);
        return await PostMensagemAsync(json, ct);
    }

    private async Task<string> PostMensagemAsync(string jsonPayload, CancellationToken ct)
    {
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_options.MessagesEndpoint, content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError(
                "[WHATSAPP] Falha no envio. Status: {Status}, Erro: {Error}",
                (int)response.StatusCode, errorBody);
            throw new WhatsAppGatewayException(
                $"Falha ao enviar mensagem WhatsApp (HTTP {(int)response.StatusCode})",
                (int)response.StatusCode, errorBody);
        }

        var result = await response.Content.ReadFromJsonAsync<EnviarMensagemResponse>(JsonOpts, ct);
        var wamid = result?.Messages?.FirstOrDefault()?.Id;

        if (string.IsNullOrEmpty(wamid))
            throw new WhatsAppGatewayException("Meta não retornou wamid na resposta de envio.");

        _logger.LogInformation("[WHATSAPP] Mensagem enviada com sucesso. WaMessageId: {WaMessageId}", wamid);
        return wamid;
    }

    private static string NormalizarTelefone(string telefone)
    {
        var limpo = new string(telefone.Where(c => char.IsDigit(c) || c == '+').ToArray());
        if (!limpo.StartsWith('+'))
            limpo = "+" + limpo;
        return limpo;
    }

    private static string RedactarTelefone(string telefone)
    {
        if (telefone.Length <= 4) return "****";
        return telefone[..^4].PadRight(telefone.Length - 4, '*') + telefone[^4..];
    }
}
