using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record UpdateInspecaoCommand(Guid Id, Guid EmpresaId, string NumeroInspecao,
    DateTime DataProgramada, DateTime? DataRealizada, string? Responsavel,
    string? Observacoes, string Status)
    : IRequest<InspecaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateInspecaoCommandHandler : IRequestHandler<UpdateInspecaoCommand, InspecaoDto>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateInspecaoCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<InspecaoDto> Handle(UpdateInspecaoCommand request, CancellationToken cancellationToken)
    {
        var inspecao = await _repo.GetInspecaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inspeção {request.Id} não encontrada.");

        inspecao.Numero = request.NumeroInspecao;
        inspecao.DataProgramada = request.DataProgramada;
        inspecao.DataRealizacao = request.DataRealizada;
        inspecao.ResponsavelInsId = request.Responsavel;
        inspecao.Observacoes = request.Observacoes;
        if (Enum.TryParse<StatusInspecaoEnum>(request.Status, out var newStatus))
        {
            if (newStatus is StatusInspecaoEnum.Aprovada or StatusInspecaoEnum.Reprovada or StatusInspecaoEnum.ComPendencias)
            {
                if (!request.DataRealizada.HasValue)
                    throw new InvalidOperationException(
                        $"A data de realização é obrigatória para concluir a inspeção com status '{newStatus}'.");
            }
            inspecao.Status = newStatus;
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new InspecaoDto(
            inspecao.Id, inspecao.ObraId, inspecao.Numero, inspecao.Titulo, inspecao.Descricao,
            inspecao.Status, inspecao.DataProgramada, inspecao.DataRealizacao,
            inspecao.Localicacao, inspecao.TemNaoConformidade, inspecao.CreatedAt);
    }
}
