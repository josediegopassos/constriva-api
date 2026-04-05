using System.Text.Json;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Infrastructure.Persistence;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Messaging.Contracts.Lens.Events;
using Constriva.API.Hubs;

namespace Constriva.API.Consumers;

public class DocumentoLensConcluidoConsumer : IConsumer<DocumentoLensCompletedEvent>
{
    private readonly AppDbContext _db;
    private readonly ILensNotificationService _notificacao;
    private readonly ILogger<DocumentoLensConcluidoConsumer> _logger;

    public DocumentoLensConcluidoConsumer(
        AppDbContext db,
        ILensNotificationService notificacao,
        ILogger<DocumentoLensConcluidoConsumer> logger)
    {
        _db = db;
        _notificacao = notificacao;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DocumentoLensCompletedEvent> context)
    {
        var evento = context.Message;
        var ct = context.CancellationToken;

        _logger.LogInformation("Processamento {ProcessamentoId} concluído. Processando resultado...", evento.ProcessamentoId);

        var documento = await _db.DocumentosLens
            .FirstOrDefaultAsync(d => d.Id == evento.ProcessamentoId && !d.IsDeleted, ct);

        if (documento is null)
        {
            _logger.LogWarning("Documento {ProcessamentoId} não encontrado no banco.", evento.ProcessamentoId);
            return;
        }

        // Atualizar campos do documento
        documento.ConfidenceScore = evento.ConfidenceScore;
        documento.Warnings = evento.Warnings;
        documento.TempoProcessamentoMs = evento.TempoProcessamentoMs;
        documento.TiposConferem = evento.TiposConferem;
        documento.MetodoExtracao = evento.MetodoExtracao == "VISION_AI"
            ? MetodoExtracaoLensEnum.VisionAI
            : MetodoExtracaoLensEnum.OCR;

        if (Enum.TryParse<TipoDocumentoLensEnum>(evento.TipoDocumento, true, out var tipoDetectado))
            documento.TipoDocumento = tipoDetectado;

        documento.CnpjFornecedorExtraido = evento.CnpjFornecedor;
        documento.NomeFornecedorExtraido = evento.FornecedorSugerido;
        documento.ValorTotalExtraido = evento.ValorTotal;
        documento.DataEmissaoExtraida = evento.DataEmissao;

        // Deserializar e criar itens se houver dados extraídos
        if (!string.IsNullOrEmpty(evento.DadosExtraidosJson))
        {
            try
            {
                var dadosJson = JsonDocument.Parse(evento.DadosExtraidosJson);
                var root = dadosJson.RootElement;

                // Extrair itens — suporta todos os tipos de documento do Lens
                // NFE/NFCE/NFSE/CupomFiscal/Boleto/Recibo/Fatura/RPA: "items"
                // CTE: "cargo_items"
                // BoletimMedicao: "items"
                var itensArrays = new[] { "items", "cargo_items" };
                var ordem = 1;
                foreach (var arrayName in itensArrays)
                {
                    var itensElement = TryGetProp(root, arrayName);
                    if (itensElement is null || itensElement.Value.ValueKind != JsonValueKind.Array)
                        continue;

                    foreach (var itemJson in itensElement.Value.EnumerateArray())
                    {
                        var item = new ItemDocumentoLens
                        {
                            DocumentoLensId = documento.Id,
                            OrdemItem = ordem++,
                            Codigo = GetStr(itemJson, "code", "codigo"),
                            Descricao = GetStr(itemJson, "description") ?? "Item sem descrição",
                            Ncm = GetStr(itemJson, "ncm"),
                            Cfop = GetStr(itemJson, "cfop"),
                            Unidade = GetStr(itemJson, "unit"),
                            Quantidade = GetDec(itemJson, "quantity", "contracted_quantity", "current_quantity"),
                            PrecoUnitario = GetDec(itemJson, "unit_price"),
                            PrecoTotal = GetDec(itemJson, "total_price", "total_value", "current_value"),
                            Desconto = GetDec(itemJson, "discount"),
                            AliquotaIcms = GetDec(itemJson, "icms_rate"),
                            AliquotaIpi = GetDec(itemJson, "ipi_rate", "tax_rate"),
                            Status = StatusItemLensEnum.Pendente
                        };
                        _db.ItensDocumentoLens.Add(item);
                    }
                }

                // Extrair número do documento (varia por tipo)
                documento.NumeroDocumentoExtraido ??=
                    GetStr(root, "number", "service_invoice_number", "document_number", "our_number");

                // Extrair parte emitente/fornecedor (cada tipo tem nome diferente)
                var parteEmitente = TryGetProp(root, "emitter", "provider", "issuer", "autonomous",
                    "contractor", "assignor", "beneficiary", "sender_name");
                if (parteEmitente.HasValue && parteEmitente.Value.ValueKind == JsonValueKind.Object)
                {
                    documento.CnpjFornecedorExtraido ??= GetStr(parteEmitente.Value, "cnpj");
                    documento.NomeFornecedorExtraido ??= GetStr(parteEmitente.Value, "name");
                }
                // Boleto/ComprovantePagamento: campos diretos
                documento.CnpjFornecedorExtraido ??= GetStr(root, "assignor_cnpj", "sender_cpf_cnpj", "recipient_cpf_cnpj");
                documento.NomeFornecedorExtraido ??= GetStr(root, "assignor", "sender_name", "recipient_name");

                // Extrair valor total (cada tipo tem campo diferente)
                documento.ValorTotalExtraido ??=
                    GetDec(root, "total_invoice")        // NFE, NFCE
                    ?? GetDec(root, "service_value")     // NFSE
                    ?? GetDec(root, "net_value")         // NFSE, Recibo, RPA
                    ?? GetDec(root, "total")             // CupomFiscal, Fatura
                    ?? GetDec(root, "value")             // Boleto, ComprovantePagamento
                    ?? GetDec(root, "charged_value")     // Boleto
                    ?? GetDec(root, "gross_value")       // Recibo, RPA
                    ?? GetDec(root, "total_service_value") // CTE
                    ?? GetDec(root, "current_total")     // BoletimMedicao
                    ?? GetDec(root, "total_products");

                // Data emissão
                documento.DataEmissaoExtraida ??=
                    GetStr(root, "issue_date", "transaction_date", "due_date", "approval_date");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deserializar dados extraídos do processamento {ProcessamentoId}.", evento.ProcessamentoId);
            }
        }

        // Tentar matching de fornecedor por CNPJ
        if (!string.IsNullOrEmpty(documento.CnpjFornecedorExtraido))
        {
            var fornecedor = await _db.Fornecedores
                .FirstOrDefaultAsync(f => f.Documento == documento.CnpjFornecedorExtraido
                    && f.EmpresaId == documento.EmpresaId && !f.IsDeleted, ct);
            if (fornecedor is not null)
                documento.FornecedorId = fornecedor.Id;
        }

        documento.Status = StatusProcessamentoLensEnum.AguardandoRevisao;
        documento.PodeReprocessar = true;

        await _db.SaveChangesAsync(ct);

        // Notificar via SignalR
        await _notificacao.NotificarProcessamentoConcluido(
            documento, evento.ConfidenceScore, evento.TotalItens,
            evento.Warnings, evento.FornecedorSugerido, evento.CnpjFornecedor,
            evento.ValorTotal, evento.DataEmissao, evento.TempoProcessamentoMs);

        _logger.LogInformation("Resultado do processamento {ProcessamentoId} salvo com sucesso.", evento.ProcessamentoId);
    }

    /// <summary>Tenta obter uma propriedade JSON por múltiplos nomes (EN/PT).</summary>
    private static JsonElement? TryGetProp(JsonElement el, params string[] names)
    {
        foreach (var name in names)
            if (el.TryGetProperty(name, out var prop) && prop.ValueKind != JsonValueKind.Null)
                return prop;
        return null;
    }

    /// <summary>Obtém string de um JSON tentando múltiplos nomes.</summary>
    private static string? GetStr(JsonElement el, params string[] names)
    {
        var prop = TryGetProp(el, names);
        return prop?.ValueKind == JsonValueKind.String ? prop.Value.GetString() : null;
    }

    /// <summary>Obtém decimal de um JSON tentando múltiplos nomes.</summary>
    private static decimal? GetDec(JsonElement el, params string[] names)
    {
        var prop = TryGetProp(el, names);
        if (prop is null) return null;
        return prop.Value.ValueKind == JsonValueKind.Number ? prop.Value.GetDecimal() : null;
    }
}
