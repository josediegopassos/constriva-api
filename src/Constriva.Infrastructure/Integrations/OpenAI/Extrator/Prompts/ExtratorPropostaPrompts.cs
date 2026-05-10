using System.Text;
using Constriva.Domain.ValueObjects.WhatsApp;

namespace Constriva.Infrastructure.Integrations.OpenAI.Extrator.Prompts;

public static class ExtratorPropostaPrompts
{
    public const string SystemPrompt = """
        Você é um especialista em análise de propostas comerciais da construção civil brasileira.
        Sua função é extrair dados estruturados de propostas de fornecedores recebidas via WhatsApp.

        SUAS RESPONSABILIDADES:
        1. Identificar se a mensagem é uma proposta comercial ou não
        2. Extrair preços, prazos e condições de pagamento
        3. Mapear cada item cotado ao ItemCotacaoId correspondente da lista fornecida
        4. Calcular um score de confiança para cada item e para a proposta geral
        5. Retornar APENAS JSON válido no schema especificado — sem texto adicional

        REGRAS DE MAPEAMENTO DE ITENS:
        - Compare a descrição do fornecedor com as descrições dos itens da cotação
        - Use similaridade semântica, não apenas correspondência exata de texto
        - Se um item não puder ser mapeado com certeza, deixe item_cotacao_id como null
        - Um item da proposta pode corresponder a apenas um item da cotação

        REGRAS DE CONFIANÇA POR ITEM (confianca_item de 0 a 100):
        - 90-100: mapeamento exato + preço coerente com referência (±20%)
        - 70-89: mapeamento provável + preço presente
        - 50-69: mapeamento possível mas ambíguo
        - 30-49: mapeamento incerto ou preço inconsistente
        - 0-29: não foi possível mapear com confiança mínima

        REGRAS PARA nao_e_proposta = true:
        - Mensagem é uma pergunta do fornecedor
        - Mensagem é um agradecimento ou confirmação sem preços
        - Mensagem é completamente fora de contexto
        - Mensagem solicita prazo ou esclarecimentos antes de cotar

        REGRAS PARA DATAS (validade_proposta):
        - Converter qualquer formato de data para ISO 8601: "YYYY-MM-DD"
        - Se não informada, retornar null

        REGRAS PARA VALORES MONETÁRIOS:
        - Retornar sempre como número decimal (não string)
        - Remover R$, pontos de milhar e converter vírgula decimal para ponto
        - Exemplo: "R$ 1.250,75" → 1250.75

        FORMATO DE RESPOSTA OBRIGATÓRIO — retorne APENAS este JSON:
        {
          "nao_e_proposta": false,
          "motivo_nao_e_proposta": null,
          "condicoes_pagamento": "30/60/90 dias",
          "prazo_entrega_dias": 15,
          "validade_proposta": "2025-02-15",
          "observacoes": "Preços sujeitos a disponibilidade de estoque",
          "itens": [
            {
              "item_cotacao_id": "uuid-do-item",
              "descricao_original": "Parafuso sextavado M8x25 zincado",
              "preco_unitario": 0.45,
              "quantidade": 500,
              "unidade_medida": "UN",
              "marca": "Tramontina",
              "disponivel": true,
              "confianca_item": 92,
              "observacao": null
            }
          ]
        }
        """;

    public static string ConstruirContextoCotacao(
        string numeroCotacao,
        string nomeFornecedor,
        IReadOnlyList<ItemCotacaoReferenciaValueObject> itensCotacao)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"COTAÇÃO: {numeroCotacao}");
        sb.AppendLine($"FORNECEDOR: {nomeFornecedor}");
        sb.AppendLine();
        sb.AppendLine("ITENS ESPERADOS NA COTAÇÃO:");

        foreach (var item in itensCotacao)
        {
            sb.AppendLine($"- ID: {item.ItemCotacaoId}");
            sb.AppendLine($"  Descrição: {item.Descricao}");
            sb.AppendLine($"  Quantidade: {item.Quantidade} {item.UnidadeMedida}");
            if (!string.IsNullOrEmpty(item.Especificacao))
                sb.AppendLine($"  Especificação: {item.Especificacao}");
            if (item.PrecoReferencia.HasValue)
                sb.AppendLine($"  Preço referência: R$ {item.PrecoReferencia:N2}");
            sb.AppendLine();
        }

        sb.AppendLine("MENSAGEM DO FORNECEDOR A SER ANALISADA:");
        return sb.ToString();
    }

    public const string InstrucaoAnaliseImagem = """
        A imagem acima contém uma proposta comercial ou tabela de preços.
        Extraia todos os itens, preços, condições e informações visíveis.
        Se houver texto escrito à mão, tente interpretar com melhor esforço.
        """;

    public const string InstrucaoAnalisePdf = """
        O documento acima é uma proposta comercial em PDF.
        Extraia todos os itens, preços, condições e informações do documento.
        Considere tabelas, listas e texto corrido na extração.
        """;

    public const string InstrucaoAnaliseTextoEImagem = """
        Analise tanto o texto quanto a imagem fornecidos.
        O texto pode ser uma explicação ou complemento da proposta mostrada na imagem.
        Combine as informações de ambas as fontes na extração.
        """;
}
