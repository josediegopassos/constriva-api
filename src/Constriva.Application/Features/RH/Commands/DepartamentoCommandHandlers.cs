using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

// ─── Create ──────────────────────────────────────────────────────────────────
public record CreateDepartamentoCommand(Guid EmpresaId, CreateDepartamentoDto Dto) : IRequest<DepartamentoDto>, ITenantRequest;

public class CreateDepartamentoHandler : IRequestHandler<CreateDepartamentoCommand, DepartamentoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateDepartamentoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<DepartamentoDto> Handle(CreateDepartamentoCommand r, CancellationToken ct)
    {
        var dep = new Departamento
        {
            EmpresaId = r.EmpresaId,
            Nome = r.Dto.Nome,
            Descricao = r.Dto.Descricao,
            GestorId = r.Dto.GestorId,
            DepartamentoPaiId = r.Dto.DepartamentoPaiId
        };

        await _repo.AddDepartamentoAsync(dep, ct);
        await _uow.SaveChangesAsync(ct);

        return new DepartamentoDto(dep.Id, dep.Nome, dep.Descricao, dep.GestorId, dep.Gestor?.Nome, dep.DepartamentoPaiId, dep.Ativo);
    }
}

// ─── Update ──────────────────────────────────────────────────────────────────
public record UpdateDepartamentoCommand(Guid Id, Guid EmpresaId, UpdateDepartamentoDto Dto) : IRequest<DepartamentoDto>, ITenantRequest;

public class UpdateDepartamentoHandler : IRequestHandler<UpdateDepartamentoCommand, DepartamentoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateDepartamentoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<DepartamentoDto> Handle(UpdateDepartamentoCommand r, CancellationToken ct)
    {
        var dep = await _repo.GetDepartamentoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Departamento {r.Id} não encontrado.");

        if (r.Dto.Nome != null)                  dep.Nome = r.Dto.Nome;
        if (r.Dto.Descricao != null)             dep.Descricao = r.Dto.Descricao;
        if (r.Dto.GestorId.HasValue)             dep.GestorId = r.Dto.GestorId;
        if (r.Dto.DepartamentoPaiId.HasValue)    dep.DepartamentoPaiId = r.Dto.DepartamentoPaiId;
        if (r.Dto.Ativo.HasValue)                dep.Ativo = r.Dto.Ativo.Value;

        dep.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);

        return new DepartamentoDto(dep.Id, dep.Nome, dep.Descricao, dep.GestorId, dep.Gestor?.Nome, dep.DepartamentoPaiId, dep.Ativo);
    }
}

// ─── Delete ──────────────────────────────────────────────────────────────────
public record DeleteDepartamentoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest;

public class DeleteDepartamentoHandler : IRequestHandler<DeleteDepartamentoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteDepartamentoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteDepartamentoCommand r, CancellationToken ct)
    {
        var dep = await _repo.GetDepartamentoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Departamento {r.Id} não encontrado.");

        dep.IsDeleted = true;
        dep.DeletedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

// ─── Toggle Ativo ────────────────────────────────────────────────────────────
public record ToggleAtivoDepartamentoCommand(Guid Id, Guid EmpresaId, bool Ativo) : IRequest<Unit>, ITenantRequest;

public class ToggleAtivoDepartamentoHandler : IRequestHandler<ToggleAtivoDepartamentoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public ToggleAtivoDepartamentoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(ToggleAtivoDepartamentoCommand r, CancellationToken ct)
    {
        var dep = await _repo.GetDepartamentoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Departamento {r.Id} não encontrado.");

        dep.Ativo = r.Ativo;
        dep.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
