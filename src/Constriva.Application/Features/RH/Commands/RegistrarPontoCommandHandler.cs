using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record RegistrarPontoCommand(Guid EmpresaId, RegistrarPontoDto Dto)
    : IRequest<RegistroPontoDto>, ITenantRequest;

public class RegistrarPontoCommandHandler : IRequestHandler<RegistrarPontoCommand, RegistroPontoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public RegistrarPontoCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RegistroPontoDto> Handle(RegistrarPontoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Duplicate check: same funcionário, same day, same type
        var dataInicio = dto.DataHora.Date;
        var dataFim = dataInicio.AddDays(1);
        var pontosHoje = await _repo.GetPontosAsync(
            request.EmpresaId, dto.FuncionarioId, dataInicio, dataFim, cancellationToken);
        if (pontosHoje.Any(p => p.Tipo == dto.Tipo))
            throw new InvalidOperationException(
                $"Já existe um registro de ponto do tipo '{dto.Tipo}' para este funcionário hoje.");

        var ponto = new RegistroPonto
        {
            EmpresaId = request.EmpresaId,
            FuncionarioId = dto.FuncionarioId,
            Tipo = dto.Tipo,
            DataHora = dto.DataHora,
            HorarioPrevisto = dto.HorarioPrevisto,
            Manual = false,
            Online = true
        };

        await _repo.AddPontoAsync(ponto, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new RegistroPontoDto(ponto.Id, ponto.FuncionarioId, "", ponto.Tipo, ponto.DataHora, ponto.HorarioPrevisto, ponto.HorasExtras);
    }
}
