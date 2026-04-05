using System.Net.Http.Headers;
using System.Text.Json;
using Constriva.Messaging.Models.Lens;
using Microsoft.Extensions.Logging;

namespace Constriva.Messaging.Services.Lens;

/// <summary>
/// Implementação do serviço de comunicação com o Constriva.Lens.
/// </summary>
public class ConstrivaLensService : IConstrivaLensService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILensAuthenticationService _autenticacao;
    private readonly ILogger<ConstrivaLensService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do serviço Constriva.Lens.
    /// </summary>
    public ConstrivaLensService(
        IHttpClientFactory httpClientFactory,
        ILensAuthenticationService autenticacao,
        ILogger<ConstrivaLensService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _autenticacao = autenticacao;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<LensExtracaoResposta> ProcessarDocumentoAsync(string caminhoArquivo, string tipoDocumento, CancellationToken ct)
    {
        // Normalizar: "Nfce" → "NFCE", "CupomFiscal" → "CUPOM_FISCAL", etc.
        var tipoNormalizado = NormalizarTipoDocumento(tipoDocumento);

        _logger.LogInformation("Iniciando processamento OCR do arquivo {Arquivo} como {TipoDocumento}.", caminhoArquivo, tipoNormalizado);

        var client = _httpClientFactory.CreateClient("ConstrivaLens");
        var token = await _autenticacao.ObterTokenAsync(ct);

        using var conteudoArquivo = new StreamContent(File.OpenRead(caminhoArquivo));
        conteudoArquivo.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        using var formulario = new MultipartFormDataContent();
        formulario.Add(conteudoArquivo, "file", Path.GetFileName(caminhoArquivo));
        formulario.Add(new StringContent(tipoNormalizado), "document_type");

        using var requisicao = new HttpRequestMessage(HttpMethod.Post, "/lens/extract");
        requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        requisicao.Content = formulario;

        var resposta = await client.SendAsync(requisicao, ct);

        if (!resposta.IsSuccessStatusCode)
        {
            var corpoErro = await resposta.Content.ReadAsStringAsync(ct);
            _logger.LogError(
                "Erro na chamada ao Constriva.Lens. Status: {StatusCode}, Corpo: {Corpo}",
                resposta.StatusCode, corpoErro);
            throw new HttpRequestException(
                $"Erro ao processar documento no Constriva.Lens. Status: {resposta.StatusCode}. Detalhes: {corpoErro}");
        }

        var json = await resposta.Content.ReadAsStringAsync(ct);
        var resultado = JsonSerializer.Deserialize<LensExtracaoResposta>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (resultado is null)
        {
            _logger.LogError("Resposta do Constriva.Lens retornou nula para o arquivo {Arquivo}.", caminhoArquivo);
            throw new InvalidOperationException("Resposta do Constriva.Lens não pôde ser desserializada.");
        }

        _logger.LogInformation(
            "Processamento OCR concluído. Tipo: {TipoDocumento}, Confidence: {Confidence}, Itens: {TotalItens}.",
            resultado.TipoDocumento, resultado.ConfidenceScore, resultado.DadosExtraidos.HasValue ? 1 : 0);

        return resultado;
    }

    /// <inheritdoc />
    public async Task<bool> VerificarSaudeAsync(CancellationToken ct)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ConstrivaLens");
            var resposta = await client.GetAsync("/ping", ct);
            return resposta.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao verificar saúde do Constriva.Lens.");
            return false;
        }
    }

    private static string NormalizarTipoDocumento(string tipo)
    {
        // Se já está no formato correto (ex: "NFCE", "BOLETO"), retorna direto
        if (tipo == tipo.ToUpperInvariant() && !tipo.Any(char.IsLower))
            return tipo;

        // Mapa de enum C# para API Lens
        return tipo switch
        {
            "Nfe" => "NFE",
            "Nfce" => "NFCE",
            "Nfse" => "NFSE",
            "CupomFiscal" => "CUPOM_FISCAL",
            "Boleto" => "BOLETO",
            "ComprovantePagamento" => "COMPROVANTE_PAGAMENTO",
            "Recibo" => "RECIBO",
            "Fatura" => "FATURA",
            "Rpa" => "RPA",
            "Cte" => "CTE",
            "BoletimMedicao" => "BOLETIM_MEDICAO",
            _ => tipo.ToUpperInvariant()
        };
    }
}
