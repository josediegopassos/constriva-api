using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record UpdateMedicaoCommand(Guid Id, Guid ContratoId, Guid EmpresaId,
    string Numero, DateTime DataMedicao, decimal ValorMedido, string? Observacoes)
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

        medicao.Numero = request.Numero;
        medicao.DataFim = request.DataMedicao; // DataMedicao = data da medição = fim do período
        medicao.ValorMedicao = request.ValorMedido;
        medicao.Observacoes = request.Observacoes;

        await _uow.SaveChangesAsync(cancellationToken);

        return new MedicaoContratualDto(
            medicao.Id, medicao.ContratoId, medicao.Numero,
            medicao.DataFim, medicao.ValorMedicao, medicao.Observacoes, medicao.CreatedAt);
    }
}
