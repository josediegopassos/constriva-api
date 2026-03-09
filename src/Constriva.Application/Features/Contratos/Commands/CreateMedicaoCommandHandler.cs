using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record CreateMedicaoCommand(Guid ContratoId, Guid EmpresaId, CreateMedicaoDto Dto)
    : IRequest<MedicaoContratoDto>, ITenantRequest;

public class CreateMedicaoCommandHandler : IRequestHandler<CreateMedicaoCommand, MedicaoContratoDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateMedicaoCommandHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<MedicaoContratoDto> Handle(CreateMedicaoCommand request, CancellationToken cancellationToken)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(request.ContratoId, request.EmpresaId, cancellationToken);
        if (contrato == null)
            throw new KeyNotFoundException("Contrato não encontrado.");

        if (contrato.Status != StatusContratoEnum.Ativo)
            throw new InvalidOperationException("Contrato não está ativo.");

        var medicoes = await _repo.GetMedicoesAsync(request.ContratoId, request.EmpresaId, cancellationToken);
        var medicoesList = medicoes.ToList();
        var periodo = medicoesList.Count + 1;
        var dto = request.Dto;

        var percentualAcumulado = medicoesList
            .Where(m => !m.IsDeleted)
            .Sum(m => m.PercentualMedicao);
        if (percentualAcumulado + dto.PercentualMedicao > 100)
            throw new InvalidOperationException(
                $"Percentual acumulado ({percentualAcumulado:F2}%) + nova medição ({dto.PercentualMedicao:F2}%) excede 100%.");

        var medicao = new MedicaoContratual
        {
            ContratoId = request.ContratoId,
            EmpresaId = request.EmpresaId,
            Periodo = periodo,
            Numero = dto.Numero,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            ValorMedicao = dto.ValorMedicao,
            PercentualMedicao = dto.PercentualMedicao,
            Status = StatusMedicaoEnum.Rascunho,
            Observacoes = dto.Observacoes
        };

        await _repo.AddMedicaoAsync(medicao, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new MedicaoContratoDto(
            medicao.Id, medicao.ContratoId, medicao.Periodo, medicao.Numero,
            medicao.ValorMedicao, medicao.ValorMedicao,
            medicao.Status, medicao.DataInicio, medicao.DataFim,
            null, null);
    }
}
