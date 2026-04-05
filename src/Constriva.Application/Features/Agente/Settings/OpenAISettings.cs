namespace Constriva.Application.Features.Agente.Settings;

public class OpenAISettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openai.com";
    public string Model { get; set; } = "gpt-4o-mini";
    public int MaxTokensResposta { get; set; } = 800;
    public double Temperature { get; set; } = 0.2;
    public int MaxIteracoesToolCall { get; set; } = 5;
    public int MaxMensagensHistorico { get; set; } = 10;
    public int MaxTokensContexto { get; set; } = 3000;
    public int CacheToolResultSegundos { get; set; } = 60;
}
