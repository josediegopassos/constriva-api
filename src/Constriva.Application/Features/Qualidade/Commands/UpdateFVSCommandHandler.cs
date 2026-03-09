using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record UpdateFVSCommand(Guid Id, Guid EmpresaId, string Servico, string? Responsavel,
    DateTime DataVerificacao, bool Aprovado, string? Observacoes)
    : IRequest<FvsDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateFVSCommandHandler : IRequestHandler<UpdateFVSCommand, FvsDto>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateFVSCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FvsDto> Handle(UpdateFVSCommand request, CancellationToken cancellationToken)
    {
        var fvs = await _repo.GetFVSByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"FVS {request.Id} não encontrado.");

        fvs.Servico = request.Servico;
        fvs.DataVerificacao = request.DataVerificacao;
        fvs.Aprovado = request.Aprovado;
        fvs.Observacoes = request.Observacoes;

        await _uow.SaveChangesAsync(cancellationToken);

        return new FvsDto(fvs.Id, fvs.Servico, fvs.ResponsavelId, fvs.DataVerificacao, fvs.Aprovado, fvs.Observacoes, fvs.ObraId);
    }
}
