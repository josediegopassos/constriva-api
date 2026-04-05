using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetResultadoProcessamentoQuery(
    Guid Id,
    Guid EmpresaId)
    : IRequest<ResultadoProcessamentoDto?>, ITenantRequest;

public class GetResultadoProcessamentoHandler : IRequestHandler<GetResultadoProcessamentoQuery, ResultadoProcessamentoDto?>
{
    private readonly IDocumentoLensRepository _docRepo;
    private readonly ITenantRepository<Fornecedor> _fornecedorRepo;

    public GetResultadoProcessamentoHandler(
        IDocumentoLensRepository docRepo,
        ITenantRepository<Fornecedor> fornecedorRepo)
    {
        _docRepo = docRepo;
        _fornecedorRepo = fornecedorRepo;
    }

    public async Task<ResultadoProcessamentoDto?> Handle(GetResultadoProcessamentoQuery r, CancellationToken ct)
    {
        var doc = await _docRepo.GetResultadoWithMatchingAsync(r.Id, r.EmpresaId, ct);

        if (doc is null) return null;

        var sugestao = await BuildSugestaoMatching(doc, r.EmpresaId, ct);

        var itens = doc.Itens.OrderBy(i => i.OrdemItem).Select(MapItem).ToList();

        var podeConsolidar = doc.Status == StatusProcessamentoLensEnum.AguardandoRevisao
            || doc.Status == StatusProcessamentoLensEnum.EmRevisao
            || doc.Status == StatusProcessamentoLensEnum.Aprovado;

        // Verifica se todos os itens foram revisados (aprovados ou rejeitados)
        if (podeConsolidar && doc.Itens.Count > 0)
        {
            podeConsolidar = doc.Itens.All(i =>
                i.Status == StatusItemLensEnum.Aprovado ||
                i.Status == StatusItemLensEnum.Rejeitado);
        }

        var dados = new DadosResumidosDto(
            doc.NumeroDocumentoExtraido,
            doc.DataEmissaoExtraida,
            doc.ValorTotalExtraido,
            doc.NomeFornecedorExtraido,
            null);

        return new ResultadoProcessamentoDto(
            doc.Id,
            doc.TipoDocumento,
            doc.TipoDocumentoDeclarado,
            doc.TiposConferem,
            doc.Status,
            doc.ConfidenceScore,
            doc.Warnings,
            doc.TempoProcessamentoMs,
            doc.PaginasProcessadas,
            itens,
            sugestao,
            dados,
            doc.MetodoExtracao,
            doc.MetodoExtracao == MetodoExtracaoLensEnum.VisionAI ? "GPT-4o mini (Vision AI)" : "OCR (Tesseract)",
            doc.PodeReprocessar,
            podeConsolidar);
    }

    private async Task<SugestaoMatchingDto?> BuildSugestaoMatching(DocumentoLens doc, Guid empresaId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(doc.CnpjFornecedorExtraido))
            return null;

        var cnpjLimpo = doc.CnpjFornecedorExtraido.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

        var fornecedor = await _fornecedorRepo.FirstOrDefaultAsync(
            f => f.EmpresaId == empresaId && f.Documento.Replace(".", "").Replace("/", "").Replace("-", "") == cnpjLimpo, ct);

        if (fornecedor is null)
            return null;

        var confianca = 1.0f; // Match exato por CNPJ

        return new SugestaoMatchingDto(
            new FornecedorSugeridoDto(
                fornecedor.Id,
                fornecedor.RazaoSocial,
                fornecedor.NomeFantasia,
                fornecedor.Documento),
            null,
            confianca);
    }

    private static ItemDocumentoLensDto MapItem(ItemDocumentoLens i) => new(
        i.Id,
        i.OrdemItem,
        i.Codigo,
        i.Descricao,
        i.Ncm,
        i.Unidade,
        i.Quantidade,
        i.PrecoUnitario,
        i.PrecoTotal,
        i.Desconto,
        i.Status,
        i.Status.ToString(),
        i.MotivoRejeicao,
        i.EditadoPorUsuarioId.HasValue,
        i.DescricaoOriginalOcr,
        i.QuantidadeOriginalOcr,
        i.PrecoUnitarioOriginalOcr,
        null);
}
