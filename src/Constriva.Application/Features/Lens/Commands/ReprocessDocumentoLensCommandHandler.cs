using MediatR;
using MassTransit;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Messaging.Contracts.Lens.Commands;

namespace Constriva.Application.Features.Lens.Commands;

public record ReprocessDocumentoLensCommand(
    Guid Id,
    Guid EmpresaId,
    Guid UsuarioId,
    string? Motivo) : IRequest, ITenantRequest;

public class ReprocessDocumentoLensCommandHandler : IRequestHandler<ReprocessDocumentoLensCommand>
{
    private readonly ITenantRepository<DocumentoLens> _repo;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;

    public ReprocessDocumentoLensCommandHandler(
        ITenantRepository<DocumentoLens> repo,
        IUnitOfWork uow,
        IBus bus)
    {
        _repo = repo;
        _uow = uow;
        _bus = bus;
    }

    public async Task Handle(ReprocessDocumentoLensCommand r, CancellationToken ct)
    {
        var documento = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.Id} não encontrado.");

        if (documento.Status != StatusProcessamentoLensEnum.Pendente &&
            documento.Status != StatusProcessamentoLensEnum.Erro &&
            documento.Status != StatusProcessamentoLensEnum.AguardandoRevisao)
            throw new InvalidOperationException(
                $"Documento com status '{documento.Status}' não pode ser reprocessado. Apenas documentos com status 'Pendente', 'Erro' ou 'AguardandoRevisao' são permitidos.");

        documento.TentativaNumero++;
        documento.Status = StatusProcessamentoLensEnum.Pendente;
        documento.MensagemErro = null;
        documento.CodigoErro = null;
        documento.UpdatedAt = DateTime.UtcNow;
        documento.UpdatedBy = r.UsuarioId;

        _repo.Update(documento);
        await _uow.SaveChangesAsync(ct);

        await _bus.Publish(new Constriva.Messaging.Contracts.Lens.Commands.ReprocessDocumentoLensCommand
        {
            ProcessamentoId = documento.Id,
            UsuarioId = r.UsuarioId,
            MotivoReprocessamento = r.Motivo
        }, ct);
    }
}
