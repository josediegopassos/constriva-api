using System.Text.Json;
using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens;

/// <summary>
/// Modelo de resposta da extração de dados pelo Constriva.Lens.
/// Mapeado 1:1 com o LensResponse do Python.
/// </summary>
public class LensExtracaoResposta
{
    [JsonPropertyName("document_type")]
    public string TipoDocumento { get; set; } = string.Empty;

    [JsonPropertyName("declared_type")]
    public string TipoDocumentoDeclarado { get; set; } = string.Empty;

    [JsonPropertyName("type_match")]
    public bool TiposConferem { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("raw_text")]
    public string TextoBruto { get; set; } = string.Empty;

    [JsonPropertyName("extracted_data")]
    public JsonElement? DadosExtraidos { get; set; }

    [JsonPropertyName("confidence_score")]
    public float ConfidenceScore { get; set; }

    [JsonPropertyName("warnings")]
    public List<string> Warnings { get; set; } = new();

    [JsonPropertyName("processing_time_ms")]
    public int TempoProcessamentoMs { get; set; }

    [JsonPropertyName("pages_processed")]
    public int PaginasProcessadas { get; set; }

    [JsonPropertyName("metodo_extracao")]
    public string MetodoExtracao { get; set; } = "OCR";
}
