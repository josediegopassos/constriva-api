using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record UpdatePontoCommand(Guid Id, Guid EmpresaId, DateTime Entrada,
    DateTime? Saida, string? Observacoes)
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

        // Cada registro representa um único evento de entrada OU saída.
        // O campo DataHora é atualizado com o valor correto conforme o tipo do registro.
        ponto.DataHora = ponto.Tipo switch
        {
            TipoRegistroPontoEnum.Saida or TipoRegistroPontoEnum.FimIntervalo =>
                request.Saida
                ?? throw new InvalidOperationException(
                    $"Campo 'Saida' é obrigatório para registros do tipo {ponto.Tipo}."),
            _ => request.Entrada
        };

        if (request.Observacoes != null) ponto.Justificativa = request.Observacoes;
        ponto.Manual = true;

        await _uow.SaveChangesAsync(cancellationToken);

        return new RegistroPontoDto(ponto.Id, ponto.FuncionarioId, "", ponto.Tipo, ponto.DataHora, ponto.HorarioPrevisto, ponto.HorasExtras);
    }
}
