using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

// ── Create ────────────────────────────────────────────────────────────────────

public record CreateEPICommand(Guid EmpresaId, CreateEPIDto Dto) : IRequest<EPIDto>, ITenantRequest;

public class CreateEPICommandHandler : IRequestHandler<CreateEPICommand, EPIDto>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateEPICommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<EPIDto> Handle(CreateEPICommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var epi = new EPI
        {
            EmpresaId     = request.EmpresaId,
            Codigo        = dto.Codigo,
            Nome          = dto.Nome,
            Tipo          = dto.Tipo,
            Descricao     = dto.Descricao,
            Fabricante    = dto.Fabricante,
            Modelo        = dto.Modelo,
            NumeroCA      = dto.NumeroCA,
            ValidadeCA    = dto.ValidadeCA,
            EstoqueAtual  = dto.EstoqueAtual,
            EstoqueMinimo = dto.EstoqueMinimo,
            VidaUtilMeses = dto.VidaUtilMeses,
            Ativo         = true
        };

        await _repo.AddEPIAsync(epi, ct);
        await _uow.SaveChangesAsync(ct);

        return new EPIDto(epi.Id, epi.Codigo, epi.Nome, epi.Descricao, epi.Tipo,
            epi.Fabricante, epi.Modelo, epi.NumeroCA, epi.ValidadeCA,
            epi.EstoqueAtual, epi.EstoqueMinimo, epi.VidaUtilMeses, epi.Ativo);
    }
}

// ── Update ────────────────────────────────────────────────────────────────────

public record UpdateEPICommand(Guid Id, Guid EmpresaId, UpdateEPIDto Dto) : IRequest<EPIDto>, ITenantRequest;

public class UpdateEPICommandHandler : IRequestHandler<UpdateEPICommand, EPIDto>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateEPICommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<EPIDto> Handle(UpdateEPICommand request, CancellationToken ct)
    {
        var epi = await _repo.GetEPIByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"EPI {request.Id} não encontrado.");

        var dto = request.Dto;
        epi.Nome          = dto.Nome;
        epi.Tipo          = dto.Tipo;
        epi.Descricao     = dto.Descricao;
        epi.Fabricante    = dto.Fabricante;
        epi.Modelo        = dto.Modelo;
        epi.NumeroCA      = dto.NumeroCA;
        epi.ValidadeCA    = dto.ValidadeCA;
        epi.EstoqueAtual  = dto.EstoqueAtual;
        epi.EstoqueMinimo = dto.EstoqueMinimo;
        epi.VidaUtilMeses = dto.VidaUtilMeses;
        epi.Ativo         = dto.Ativo;

        await _uow.SaveChangesAsync(ct);

        return new EPIDto(epi.Id, epi.Codigo, epi.Nome, epi.Descricao, epi.Tipo,
            epi.Fabricante, epi.Modelo, epi.NumeroCA, epi.ValidadeCA,
            epi.EstoqueAtual, epi.EstoqueMinimo, epi.VidaUtilMeses, epi.Ativo);
    }
}

// ── Delete ────────────────────────────────────────────────────────────────────

public record DeleteEPICommand(Guid Id, Guid EmpresaId) : IRequest, ITenantRequest;

public class DeleteEPICommandHandler : IRequestHandler<DeleteEPICommand>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteEPICommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteEPICommand request, CancellationToken ct)
    {
        var epi = await _repo.GetEPIByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"EPI {request.Id} não encontrado.");

        epi.IsDeleted = true;
        await _uow.SaveChangesAsync(ct);
    }
}
