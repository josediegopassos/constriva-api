using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record UpdateDDSCommand(Guid Id, Guid EmpresaId, string Tema, string Local,
    DateTime DataRealizacao, int QuantidadeParticipantes, string? Observacoes)
    : IRequest<DdsDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateDDSCommandHandler : IRequestHandler<UpdateDDSCommand, DdsDto>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateDDSCommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DdsDto> Handle(UpdateDDSCommand request, CancellationToken cancellationToken)
    {
        var dds = await _repo.GetDDSByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"DDS {request.Id} não encontrado.");

        dds.Tema = request.Tema;
        dds.Local = request.Local;
        dds.DataRealizacao = request.DataRealizacao;
        dds.NumeroParticipantes = request.QuantidadeParticipantes;

        await _uow.SaveChangesAsync(cancellationToken);

        return new DdsDto(dds.Id, dds.Tema, dds.Local, dds.DataRealizacao, dds.NumeroParticipantes, dds.CreatedAt);
    }
}
