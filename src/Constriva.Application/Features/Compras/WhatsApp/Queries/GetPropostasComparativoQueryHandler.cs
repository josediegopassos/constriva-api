using MediatR;

using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.WhatsApp.Queries;

public record GetPropostasComparativoQuery(Guid EmpresaId, Guid CotacaoId)
    : IRequest<PropostasComparativoDto>, ITenantRequest;

public record PropostasComparativoDto
{
    public required Guid CotacaoId { get; init; }
    public required string NumeroCotacao { get; init; }
    public required string TituloCotacao { get; init; }
    public required IReadOnlyList<ItemComparativoDto> Itens { get; init; }
    public required IReadOnlyList<FornecedorComparativoDto> Fornecedores { get; init; }
    public Guid? FornecedorMenorValorId { get; init; }
    public required bool PossuiVencedora { get; init; }
    public Guid? PropostaVencedoraId { get; init; }
}

public record ItemComparativoDto
{
    public required Guid ItemCotacaoId { get; init; }
    public required string Descricao { get; init; }
    public required string UnidadeMedida { get; init; }
    public required decimal QuantidadeSolicitada { get; init; }
    public decimal? PrecoReferencia { get; init; }
    public required IReadOnlyList<PrecoFornecedorDto> PrecosPorFornecedor { get; init; }
}

public record PrecoFornecedorDto
{
    public required Guid FornecedorId { get; init; }
    public required Guid PropostaCotacaoId { get; init; }
    public required Guid ItemPropostaCotacaoId { get; init; }
    public required decimal PrecoUnitario { get; init; }
    public required decimal Quantidade { get; init; }
    public required decimal ValorTotal { get; init; }
    public string? Marca { get; init; }
    public required bool Disponivel { get; init; }
    public required bool MenorPreco { get; init; }
    public string? Observacao { get; init; }
}

public record FornecedorComparativoDto
{
    public required Guid FornecedorId { get; init; }
    public required Guid PropostaCotacaoId { get; init; }
    public required string NomeFornecedor { get; init; }
    public required decimal ValorTotal { get; init; }
    public string? CondicoesPagamento { get; init; }
    public int? PrazoEntregaDias { get; init; }
    public DateTime? ValidadeProposta { get; init; }
    public required bool Vencedora { get; init; }
    public required int TotalItensRespondidos { get; init; }
    public required int TotalItensSolicitados { get; init; }
    public required decimal PercentualCobertura { get; init; }
}

public class GetPropostasComparativoHandler
    : IRequestHandler<GetPropostasComparativoQuery, PropostasComparativoDto>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IComprasRepository _comprasRepo;

    public GetPropostasComparativoHandler(
        IWhatsAppCotacaoRepository repo, IComprasRepository comprasRepo)
    {
        _repo = repo;
        _comprasRepo = comprasRepo;
    }

    public async Task<PropostasComparativoDto> Handle(
        GetPropostasComparativoQuery request, CancellationToken ct)
    {
        var empresaId = request.EmpresaId;

        var cotacao = await _comprasRepo.GetCotacaoByIdAsync(request.CotacaoId, empresaId, ct)
            ?? throw new KeyNotFoundException("Cotação não encontrada");

        var cotacaoCompleta = await _repo.GetCotacaoComFornecedoresEItensAsync(request.CotacaoId, empresaId, ct)
            ?? throw new KeyNotFoundException("Cotação não encontrada");

        var propostas = await _repo.GetPropostasCotacaoComItensAsync(request.CotacaoId, empresaId, ct);

        var totalItensCotacao = cotacaoCompleta.Itens.Count;

        var itens = cotacaoCompleta.Itens
            .OrderBy(i => i.Ordem)
            .Select(itemCotacao =>
            {
                var precos = propostas
                    .SelectMany(p => p.Itens
                        .Where(ip => ip.ItemCotacaoId == itemCotacao.Id)
                        .Select(ip => new PrecoFornecedorDto
                        {
                            FornecedorId = p.FornecedorId,
                            PropostaCotacaoId = p.Id,
                            ItemPropostaCotacaoId = ip.Id,
                            PrecoUnitario = ip.PrecoUnitario,
                            Quantidade = ip.Quantidade,
                            ValorTotal = ip.PrecoUnitario * ip.Quantidade,
                            Marca = ip.Marca,
                            Disponivel = ip.Disponivel,
                            MenorPreco = ip.MenorPreco,
                            Observacao = ip.Observacao
                        }))
                    .ToList();

                return new ItemComparativoDto
                {
                    ItemCotacaoId = itemCotacao.Id,
                    Descricao = itemCotacao.Descricao,
                    UnidadeMedida = itemCotacao.UnidadeMedida,
                    QuantidadeSolicitada = itemCotacao.Quantidade,
                    PrecoReferencia = itemCotacao.PrecoReferencia,
                    PrecosPorFornecedor = precos
                };
            })
            .ToList();

        var fornecedores = propostas
            .Select(p => new FornecedorComparativoDto
            {
                FornecedorId = p.FornecedorId,
                PropostaCotacaoId = p.Id,
                NomeFornecedor = p.Fornecedor.NomeFantasia ?? p.Fornecedor.RazaoSocial,
                ValorTotal = p.ValorTotal,
                CondicoesPagamento = p.CondicoesPagamento,
                PrazoEntregaDias = p.PrazoEntrega,
                ValidadeProposta = p.DataValidade,
                Vencedora = p.Vencedora,
                TotalItensRespondidos = p.Itens.Count,
                TotalItensSolicitados = totalItensCotacao,
                PercentualCobertura = totalItensCotacao > 0
                    ? Math.Round((decimal)p.Itens.Count / totalItensCotacao * 100, 1)
                    : 0
            })
            .OrderBy(f => f.ValorTotal)
            .ToList();

        var vencedora = propostas.FirstOrDefault(p => p.Vencedora);

        return new PropostasComparativoDto
        {
            CotacaoId = request.CotacaoId,
            NumeroCotacao = cotacao.Numero,
            TituloCotacao = cotacao.Titulo,
            Itens = itens,
            Fornecedores = fornecedores,
            FornecedorMenorValorId = fornecedores.MinBy(f => f.ValorTotal)?.FornecedorId,
            PossuiVencedora = vencedora != null,
            PropostaVencedoraId = vencedora?.Id
        };
    }
}
