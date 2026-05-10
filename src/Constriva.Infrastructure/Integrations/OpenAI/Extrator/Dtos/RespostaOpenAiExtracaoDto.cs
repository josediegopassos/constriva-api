using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.OpenAI.Extrator.Dtos;

public class RespostaOpenAiExtracaoDto
{
    [JsonPropertyName("nao_e_proposta")] public bool NaoEProposta { get; set; }
    [JsonPropertyName("motivo_nao_e_proposta")] public string? MotivoNaoEProposta { get; set; }
    [JsonPropertyName("condicoes_pagamento")] public string? CondicoesPagamento { get; set; }
    [JsonPropertyName("prazo_entrega_dias")] public int? PrazoEntregaDias { get; set; }
    [JsonPropertyName("validade_proposta")] public string? ValidadeProposta { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    [JsonPropertyName("itens")] public List<ItemExtracaoDto>? Itens { get; set; }
}

public class ItemExtracaoDto
{
    [JsonPropertyName("item_cotacao_id")] public string? ItemCotacaoId { get; set; }
    [JsonPropertyName("descricao_original")] public string? DescricaoOriginal { get; set; }
    [JsonPropertyName("preco_unitario")] public decimal? PrecoUnitario { get; set; }
    [JsonPropertyName("quantidade")] public decimal? Quantidade { get; set; }
    [JsonPropertyName("unidade_medida")] public string? UnidadeMedida { get; set; }
    [JsonPropertyName("marca")] public string? Marca { get; set; }
    [JsonPropertyName("disponivel")] public bool? Disponivel { get; set; }
    [JsonPropertyName("confianca_item")] public int? ConfiancaItem { get; set; }
    [JsonPropertyName("observacao")] public string? Observacao { get; set; }
}
