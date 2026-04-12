using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record UpdatePontoCommand(Guid Id, Guid EmpresaId, UpdatePontoDto Dto)
    : IRequest<RegistroPontoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdatePontoCommandHandler : IRequestHandler<UpdatePontoCommand, RegistroPontoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePontoCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RegistroPontoDto> Handle(UpdatePontoCommand request, CancellationToken cancellationToken)
    {
        var ponto = await _repo.GetPontoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Registro de ponto {request.Id} não encontrado.");

        var dto = request.Dto;

        ponto.ObraId = dto.ObraId ?? ponto.ObraId;
        ponto.DataHora = dto.DataHora;
        ponto.HorarioPrevisto = dto.HorarioPrevisto;
        ponto.HorasExtras = dto.HorasExtras;
        ponto.Latitude = dto.Latitude;
        ponto.Longitude = dto.Longitude;
        ponto.Dispositivo = dto.Dispositivo;
        if (dto.Online.HasValue) ponto.Online = dto.Online.Value;
        if (dto.Manual.HasValue) ponto.Manual = dto.Manual.Value;
        ponto.Justificativa = dto.Justificativa;

        await _uow.SaveChangesAsync(cancellationToken);

        return new RegistroPontoDto(ponto.Id, ponto.FuncionarioId, "",
            ponto.ObraId, ponto.Tipo, ponto.DataHora,
            ponto.HorarioPrevisto, ponto.HorasExtras,
            ponto.Latitude, ponto.Longitude, ponto.Dispositivo,
            ponto.Online, ponto.Manual, ponto.Justificativa,
            ponto.StatusAprovacao, ponto.AprovadoPor, null,
            ponto.ReprovadoPor, null,
            ponto.CreatedAt);
    }
}
