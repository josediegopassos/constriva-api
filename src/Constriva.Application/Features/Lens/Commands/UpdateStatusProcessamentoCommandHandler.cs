using MediatR;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Commands;

public record UpdateStatusProcessamentoCommand(
    Guid ProcessamentoId,
    StatusProcessamentoLensEnum Status,
    float? ConfidenceScore,
    List<string>? Warnings,
    string? MensagemErro,
    string? CodigoErro,
    bool? PodeReprocessar,
    int? TempoProcessamentoMs,
    int? PaginasProcessadas,
    string? NumeroDocumentoExtraido,
    string? DataEmissaoExtraida,
    decimal? ValorTotalExtraido,
    string? CnpjFornecedorExtraido,
    string? NomeFornecedorExtraido,
    string? TipoDocumentoDetectado,
    bool? TiposConferem) : IRequest;

public class UpdateStatusProcessamentoCommandHandler : IRequestHandler<UpdateStatusProcessamentoCommand>
{
    private readonly IRepository<DocumentoLens> _repo;
    private readonly IUnitOfWork _uow;

    public UpdateStatusProcessamentoCommandHandler(IRepository<DocumentoLens> repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(UpdateStatusProcessamentoCommand r, CancellationToken ct)
    {
        var documento = await _repo.GetByIdAsync(r.ProcessamentoId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.ProcessamentoId} não encontrado.");

        documento.Status = r.Status;
        documento.UpdatedAt = DateTime.UtcNow;

        if (r.ConfidenceScore.HasValue) documento.ConfidenceScore = r.ConfidenceScore.Value;
        if (r.Warnings is not null) documento.Warnings = r.Warnings;
        if (r.MensagemErro is not null) documento.MensagemErro = r.MensagemErro;
        if (r.CodigoErro is not null) documento.CodigoErro = r.CodigoErro;
        if (r.PodeReprocessar.HasValue) documento.PodeReprocessar = r.PodeReprocessar.Value;
        if (r.TempoProcessamentoMs.HasValue) documento.TempoProcessamentoMs = r.TempoProcessamentoMs.Value;
        if (r.PaginasProcessadas.HasValue) documento.PaginasProcessadas = r.PaginasProcessadas.Value;
        if (r.NumeroDocumentoExtraido is not null) documento.NumeroDocumentoExtraido = r.NumeroDocumentoExtraido;
        if (r.DataEmissaoExtraida is not null) documento.DataEmissaoExtraida = r.DataEmissaoExtraida;
        if (r.ValorTotalExtraido.HasValue) documento.ValorTotalExtraido = r.ValorTotalExtraido.Value;
        if (r.CnpjFornecedorExtraido is not null) documento.CnpjFornecedorExtraido = r.CnpjFornecedorExtraido;
        if (r.NomeFornecedorExtraido is not null) documento.NomeFornecedorExtraido = r.NomeFornecedorExtraido;
        if (r.TiposConferem.HasValue) documento.TiposConferem = r.TiposConferem.Value;

        if (r.TipoDocumentoDetectado is not null &&
            Enum.TryParse<TipoDocumentoLensEnum>(r.TipoDocumentoDetectado, true, out var tipoDetectado))
        {
            documento.TipoDocumento = tipoDetectado;
        }

        _repo.Update(documento);
        await _uow.SaveChangesAsync(ct);
    }
}
