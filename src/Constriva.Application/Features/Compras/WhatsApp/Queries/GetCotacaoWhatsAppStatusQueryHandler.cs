using MediatR;

using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.WhatsApp.Queries;

public record GetCotacaoWhatsAppStatusQuery(Guid EmpresaId, Guid CotacaoId)
    : IRequest<CotacaoWhatsAppStatusDto?>, ITenantRequest;

public record CotacaoWhatsAppStatusDto
{
    public required Guid CotacaoWhatsAppId { get; init; }
    public required Guid CotacaoId { get; init; }
    public required string NumeroCotacao { get; init; }
    public required DateTime DataLimiteResposta { get; init; }
    public required bool PrazoExpirado { get; init; }
    public required int TotalFornecedoresConvidados { get; init; }
    public required int TotalRespostas { get; init; }
    public required int TotalPropostasExtraidas { get; init; }
    public required bool Encerrada { get; init; }
    public DateTime? EncerradaEm { get; init; }
    public required IReadOnlyList<FornecedorStatusDto> Fornecedores { get; init; }
}

public record FornecedorStatusDto
{
    public required Guid FornecedorCotacaoId { get; init; }
    public required Guid FornecedorId { get; init; }
    public required string NomeFornecedor { get; init; }
    public required string TelefoneDestino { get; init; }
    public required StatusEnvioWhatsAppEnum StatusEnvio { get; init; }
    public DateTime? EnviadaEm { get; init; }
    public DateTime? RespondeuEm { get; init; }
    public required bool PropostaExtraida { get; init; }
    public int? NivelConfiancaExtracao { get; init; }
    public Guid? PropostaCotacaoId { get; init; }
    public decimal? ValorTotalProposta { get; init; }
    public MotivoFalhaProcessamentoEnum? MotivoFalhaExtracao { get; init; }
}

public class GetCotacaoWhatsAppStatusHandler
    : IRequestHandler<GetCotacaoWhatsAppStatusQuery, CotacaoWhatsAppStatusDto?>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IComprasRepository _comprasRepo;

    public GetCotacaoWhatsAppStatusHandler(
        IWhatsAppCotacaoRepository repo, IComprasRepository comprasRepo)
    {
        _repo = repo;
        _comprasRepo = comprasRepo;
    }

    public async Task<CotacaoWhatsAppStatusDto?> Handle(
        GetCotacaoWhatsAppStatusQuery request, CancellationToken ct)
    {
        var empresaId = request.EmpresaId;

        var cotacaoWa = await _repo.GetCotacaoWhatsAppComDetalhesAsync(request.CotacaoId, empresaId, ct);
        if (cotacaoWa == null)
            return null;

        var cotacao = await _comprasRepo.GetCotacaoByIdAsync(request.CotacaoId, empresaId, ct)
            ?? throw new KeyNotFoundException("Cotação não encontrada");

        var propostas = await _repo.GetPropostasCotacaoComItensAsync(request.CotacaoId, empresaId, ct);

        var fornecedores = cotacaoWa.Mensagens
            .Where(m => m.TipoMensagem == TipoMensagemWhatsAppEnum.ConviteCotacao)
            .Select(m =>
            {
                var resposta = cotacaoWa.Respostas
                    .Where(r => r.FornecedorCotacaoId == m.FornecedorCotacaoId)
                    .OrderByDescending(r => r.RecebidaEm)
                    .FirstOrDefault();

                var proposta = propostas.FirstOrDefault(p => p.FornecedorId == m.FornecedorId);

                return new FornecedorStatusDto
                {
                    FornecedorCotacaoId = m.FornecedorCotacaoId,
                    FornecedorId = m.FornecedorId,
                    NomeFornecedor = m.NomeFornecedor,
                    TelefoneDestino = m.TelefoneDestino,
                    StatusEnvio = m.Status,
                    EnviadaEm = m.EnviadaEm,
                    RespondeuEm = resposta?.RecebidaEm,
                    PropostaExtraida = resposta?.ExtraidaComSucesso ?? false,
                    NivelConfiancaExtracao = resposta?.NivelConfianca,
                    PropostaCotacaoId = proposta?.Id,
                    ValorTotalProposta = proposta?.ValorTotal,
                    MotivoFalhaExtracao = resposta?.MotivoFalha
                };
            })
            .ToList();

        return new CotacaoWhatsAppStatusDto
        {
            CotacaoWhatsAppId = cotacaoWa.Id,
            CotacaoId = request.CotacaoId,
            NumeroCotacao = cotacao.Numero,
            DataLimiteResposta = cotacaoWa.DataLimiteResposta,
            PrazoExpirado = cotacaoWa.PrazoExpirado(),
            TotalFornecedoresConvidados = cotacaoWa.TotalFornecedoresConvidados,
            TotalRespostas = cotacaoWa.TotalRespostas,
            TotalPropostasExtraidas = cotacaoWa.TotalPropostasExtraidas,
            Encerrada = cotacaoWa.EncerradaEm.HasValue,
            EncerradaEm = cotacaoWa.EncerradaEm,
            Fornecedores = fornecedores
        };
    }
}
