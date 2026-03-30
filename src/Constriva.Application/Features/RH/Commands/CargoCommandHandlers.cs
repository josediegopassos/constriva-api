using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

// ─── Create ──────────────────────────────────────────────────────────────────
public record CreateCargoCommand(Guid EmpresaId, CreateCargoDto Dto) : IRequest<CargoDto>, ITenantRequest;

public class CreateCargoHandler : IRequestHandler<CreateCargoCommand, CargoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCargoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<CargoDto> Handle(CreateCargoCommand r, CancellationToken ct)
    {
        var count = await _repo.GetCargosCountAsync(r.EmpresaId, ct);
        var cargo = new Cargo
        {
            EmpresaId = r.EmpresaId,
            Codigo = $"CAR-{(count + 1):D4}",
            Nome = r.Dto.Nome,
            CBO = r.Dto.CBO,
            Descricao = r.Dto.Descricao,
            SalarioBase = r.Dto.SalarioBase,
            SalarioMaximo = r.Dto.SalarioMaximo
        };

        await _repo.AddCargoAsync(cargo, ct);
        await _uow.SaveChangesAsync(ct);

        return new CargoDto(cargo.Id, cargo.Codigo, cargo.Nome, cargo.CBO, cargo.Descricao, cargo.SalarioBase, cargo.SalarioMaximo, cargo.Ativo);
    }
}

// ─── Update ──────────────────────────────────────────────────────────────────
public record UpdateCargoCommand(Guid Id, Guid EmpresaId, UpdateCargoDto Dto) : IRequest<CargoDto>, ITenantRequest;

public class UpdateCargoHandler : IRequestHandler<UpdateCargoCommand, CargoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCargoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<CargoDto> Handle(UpdateCargoCommand r, CancellationToken ct)
    {
        var cargo = await _repo.GetCargoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cargo {r.Id} não encontrado.");

        if (r.Dto.Nome != null)              cargo.Nome = r.Dto.Nome;
        if (r.Dto.CBO != null)               cargo.CBO = r.Dto.CBO;
        if (r.Dto.Descricao != null)         cargo.Descricao = r.Dto.Descricao;
        if (r.Dto.SalarioBase.HasValue)      cargo.SalarioBase = r.Dto.SalarioBase.Value;
        if (r.Dto.SalarioMaximo.HasValue)    cargo.SalarioMaximo = r.Dto.SalarioMaximo.Value;
        if (r.Dto.Ativo.HasValue)            cargo.Ativo = r.Dto.Ativo.Value;

        cargo.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);

        return new CargoDto(cargo.Id, cargo.Codigo, cargo.Nome, cargo.CBO, cargo.Descricao, cargo.SalarioBase, cargo.SalarioMaximo, cargo.Ativo);
    }
}

// ─── Delete ──────────────────────────────────────────────────────────────────
public record DeleteCargoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest;

public class DeleteCargoHandler : IRequestHandler<DeleteCargoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCargoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteCargoCommand r, CancellationToken ct)
    {
        var cargo = await _repo.GetCargoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cargo {r.Id} não encontrado.");

        cargo.IsDeleted = true;
        cargo.DeletedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

// ─── Toggle Ativo ────────────────────────────────────────────────────────────
public record ToggleAtivoCargoCommand(Guid Id, Guid EmpresaId, bool Ativo) : IRequest<Unit>, ITenantRequest;

public class ToggleAtivoCargoHandler : IRequestHandler<ToggleAtivoCargoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public ToggleAtivoCargoHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(ToggleAtivoCargoCommand r, CancellationToken ct)
    {
        var cargo = await _repo.GetCargoByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cargo {r.Id} não encontrado.");

        cargo.Ativo = r.Ativo;
        cargo.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
