using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Contratos.Commands;

public record SubmeterMedicaoCommand(Guid MedicaoId, Guid EmpresaId, Guid UsuarioId) : IRequest<Unit>, ITenantRequest;
public record AnalisarMedicaoCommand(Guid MedicaoId, Guid EmpresaId, Guid UsuarioId) : IRequest<Unit>, ITenantRequest;
public record AprovarMedicaoCommand(Guid MedicaoId, Guid EmpresaId, Guid UsuarioId) : IRequest<Unit>, ITenantRequest;
public record PagarMedicaoCommand(Guid MedicaoId, Guid EmpresaId, Guid UsuarioId) : IRequest<Unit>, ITenantRequest;
public record RejeitarMedicaoCommand(Guid MedicaoId, Guid EmpresaId, Guid UsuarioId, string Motivo) : IRequest<Unit>, ITenantRequest;

public class SubmeterMedicaoHandler : IRequestHandler<SubmeterMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public SubmeterMedicaoHandler(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(SubmeterMedicaoCommand r, CancellationToken ct)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(r.MedicaoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Medição {r.MedicaoId} não encontrada.");
        if (medicao.Status != StatusMedicaoEnum.Rascunho)
            throw new InvalidOperationException($"Apenas medições em rascunho podem ser submetidas. Status atual: '{medicao.Status}'.");
        medicao.Status = StatusMedicaoEnum.Submetida;
        medicao.DataSubmissao = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class AnalisarMedicaoHandler : IRequestHandler<AnalisarMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public AnalisarMedicaoHandler(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AnalisarMedicaoCommand r, CancellationToken ct)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(r.MedicaoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Medição {r.MedicaoId} não encontrada.");
        if (medicao.Status != StatusMedicaoEnum.Submetida)
            throw new InvalidOperationException($"Apenas medições submetidas podem ser analisadas. Status atual: '{medicao.Status}'.");
        medicao.Status = StatusMedicaoEnum.EmAnalise;
        medicao.AnalisadoPor = r.UsuarioId;
        medicao.DataAnalise = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class AprovarMedicaoHandler : IRequestHandler<AprovarMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public AprovarMedicaoHandler(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AprovarMedicaoCommand r, CancellationToken ct)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(r.MedicaoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Medição {r.MedicaoId} não encontrada.");
        if (medicao.Status != StatusMedicaoEnum.EmAnalise)
            throw new InvalidOperationException($"Apenas medições em análise podem ser aprovadas. Status atual: '{medicao.Status}'.");
        medicao.Status = StatusMedicaoEnum.Aprovada;
        medicao.AprovadoPor = r.UsuarioId;
        medicao.DataAprovacao = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class PagarMedicaoHandler : IRequestHandler<PagarMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public PagarMedicaoHandler(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(PagarMedicaoCommand r, CancellationToken ct)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(r.MedicaoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Medição {r.MedicaoId} não encontrada.");
        if (medicao.Status != StatusMedicaoEnum.Aprovada)
            throw new InvalidOperationException($"Apenas medições aprovadas podem ser pagas. Status atual: '{medicao.Status}'.");
        medicao.Status = StatusMedicaoEnum.Paga;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class RejeitarMedicaoHandler : IRequestHandler<RejeitarMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;
    public RejeitarMedicaoHandler(IContratoRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(RejeitarMedicaoCommand r, CancellationToken ct)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(r.MedicaoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Medição {r.MedicaoId} não encontrada.");
        if (medicao.Status is StatusMedicaoEnum.Paga or StatusMedicaoEnum.Rejeitada)
            throw new InvalidOperationException($"Não é possível rejeitar uma medição com status '{medicao.Status}'.");
        medicao.Status = StatusMedicaoEnum.Rejeitada;
        medicao.MotivoRejeicao = r.Motivo;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
