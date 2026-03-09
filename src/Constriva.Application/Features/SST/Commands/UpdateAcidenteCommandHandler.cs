using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record UpdateAcidenteCommand(Guid Id, Guid EmpresaId, string Descricao, DateTime DataAcidente,
    bool AfastamentoMedico, int? DiasAfastamento, string? CausaRaiz, string? AcoesCorretivas)
    : IRequest<AcidenteDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAcidenteCommandHandler : IRequestHandler<UpdateAcidenteCommand, AcidenteDto>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAcidenteCommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AcidenteDto> Handle(UpdateAcidenteCommand request, CancellationToken cancellationToken)
    {
        var acidente = await _repo.GetAcidenteByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Acidente {request.Id} não encontrado.");

        if (request.AfastamentoMedico && (request.DiasAfastamento == null || request.DiasAfastamento <= 0))
            throw new InvalidOperationException(
                "Dias de afastamento deve ser informado e maior que zero quando há afastamento médico.");

        acidente.DescricaoAcidente = request.Descricao;
        acidente.DataHoraAcidente = request.DataAcidente;
        acidente.AfastamentoMedico = request.AfastamentoMedico;
        acidente.DiasAfastamento = request.AfastamentoMedico ? request.DiasAfastamento : null;
        if (request.CausaRaiz != null) acidente.CausaBasica = request.CausaRaiz;
        if (request.AcoesCorretivas != null) acidente.MedidasCorretivas = request.AcoesCorretivas;

        await _uow.SaveChangesAsync(cancellationToken);

        return new AcidenteDto(
            acidente.Id, acidente.ObraId, acidente.Tipo, acidente.NomeFuncionario,
            acidente.DescricaoAcidente, acidente.Local,
            acidente.AfastamentoMedico, acidente.DiasAfastamento,
            acidente.DataHoraAcidente, acidente.NumeroCAT, acidente.CreatedAt);
    }
}
