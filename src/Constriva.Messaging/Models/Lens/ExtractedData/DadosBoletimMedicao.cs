using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Boletim de Medição de obra.
/// </summary>
public class DadosBoletimMedicao
{
    [JsonPropertyName("numero_medicao")]
    public string? NumeroMedicao { get; set; }

    [JsonPropertyName("periodo_inicio")]
    public string? PeriodoInicio { get; set; }

    [JsonPropertyName("periodo_fim")]
    public string? PeriodoFim { get; set; }

    [JsonPropertyName("contratante")]
    public ParteExtraida? Contratante { get; set; }

    [JsonPropertyName("contratada")]
    public ParteExtraida? Contratada { get; set; }

    [JsonPropertyName("nome_obra")]
    public string? NomeObra { get; set; }

    [JsonPropertyName("contrato_referencia")]
    public string? ContratoReferencia { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemMedicaoExtraido> Itens { get; set; } = new();

    [JsonPropertyName("valor_medicao_atual")]
    public decimal? ValorMedicaoAtual { get; set; }

    [JsonPropertyName("valor_acumulado")]
    public decimal? ValorAcumulado { get; set; }

    [JsonPropertyName("percentual_total")]
    public decimal? PercentualTotal { get; set; }

    [JsonPropertyName("observacoes")]
    public string? Observacoes { get; set; }
}
