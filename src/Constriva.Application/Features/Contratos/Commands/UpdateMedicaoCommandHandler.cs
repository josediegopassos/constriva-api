using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record UpdateMedicaoCommand(Guid Id, Guid ContratoId, Guid EmpresaId, UpdateMedicaoDto Dto)
    : IRequest<MedicaoContratualDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateMedicaoHandler : IRequestHandler<UpdateMedicaoCommand, MedicaoContratualDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateMedicaoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<MedicaoContratualDto> Handle(UpdateMedicaoCommand request, CancellationToken cancellationToken)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Medição {request.Id} não encontrada.");

        if (medicao.Status != StatusMedicaoEnum.Rascunho)
            throw new InvalidOperationException(
                $"Medição no status '{medicao.Status}' não pode ser editada. Apenas rascunhos podem ser alterados.");

        var dto = request.Dto;
        medicao.Numero = dto.Numero;
        medicao.DataInicio = dto.DataInicio;
        medicao.DataFim = dto.DataFim;
        medicao.ValorMedicao = dto.ValorMedicao;
        medicao.PercentualMedicao = dto.PercentualMedicao;
        medicao.Observacoes = dto.Observacoes;
        medicao.ArquivoUrl = dto.ArquivoUrl;

        await _uow.SaveChangesAsync(cancellationToken);

        return new MedicaoContratualDto(
            medicao.Id, medicao.ContratoId, medicao.Numero,
            medicao.DataInicio, medicao.DataFim,
            medicao.ValorMedicao, medicao.PercentualMedicao,
            medicao.Observacoes, medicao.ArquivoUrl, medicao.CreatedAt);
    }
}
