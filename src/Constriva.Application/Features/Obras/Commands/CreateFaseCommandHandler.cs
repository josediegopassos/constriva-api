using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record CreateFaseCommand(
    Guid ObraId, Guid EmpresaId, string Nome, int Ordem,
    DateTime Inicio, DateTime Fim, decimal ValorPrevisto,
    string? Descricao = null, string? Cor = null, Guid? FasePaiId = null)
    : IRequest<FaseObraDto>, ITenantRequest;

public class CreateFaseCommandHandler : IRequestHandler<CreateFaseCommand, FaseObraDto>
{
    private readonly IObraRepository _obraRepo;
    private readonly IFaseObraRepository _faseRepo;
    private readonly IUnitOfWork _uow;
    public CreateFaseCommandHandler(IObraRepository obraRepo, IFaseObraRepository faseRepo, IUnitOfWork uow)
    { _obraRepo = obraRepo; _faseRepo = faseRepo; _uow = uow; }

    public async Task<FaseObraDto> Handle(CreateFaseCommand r, CancellationToken ct)
    {
        var obra = await _obraRepo.GetByIdAndEmpresaAsync(r.ObraId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.ObraId} não encontrada.");

        var fase = new FaseObra
        {
            ObraId = r.ObraId, EmpresaId = r.EmpresaId,
            Nome = r.Nome, Descricao = r.Descricao, Ordem = r.Ordem, Cor = r.Cor,
            DataInicioPrevista = r.Inicio, DataFimPrevista = r.Fim,
            ValorPrevisto = r.ValorPrevisto, FasePaiId = r.FasePaiId
        };

        await _faseRepo.AddAsync(fase, ct);
        await _uow.SaveChangesAsync(ct);

        return new FaseObraDto(
            fase.Id, fase.Nome, fase.Descricao, fase.Ordem, fase.Status,
            fase.PercentualConcluido,
            fase.DataInicioPrevista, fase.DataFimPrevista,
            fase.DataInicioReal, fase.DataFimReal,
            fase.ValorPrevisto, fase.ValorRealizado,
            fase.FasePaiId, fase.Cor);
    }
}

