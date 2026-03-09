using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record CreateAcidenteCommand(Guid EmpresaId, CreateAcidenteDto Dto)
    : IRequest<AcidenteDto>, ITenantRequest;

public class CreateAcidenteCommandHandler : IRequestHandler<CreateAcidenteCommand, AcidenteDto>
{
    private readonly ISSTRepository _repo;
    private readonly IRHRepository _rhRepo;
    private readonly IUnitOfWork _uow;

    public CreateAcidenteCommandHandler(ISSTRepository repo, IRHRepository rhRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _rhRepo = rhRepo;
        _uow = uow;
    }

    public async Task<AcidenteDto> Handle(CreateAcidenteCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (dto.FuncionarioId.HasValue)
            _ = await _rhRepo.GetFuncionarioByIdAsync(dto.FuncionarioId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Funcionário {dto.FuncionarioId} não encontrado.");

        if (dto.AfastamentoMedico && (dto.DiasAfastamento == null || dto.DiasAfastamento <= 0))
            throw new InvalidOperationException(
                "Dias de afastamento deve ser informado e maior que zero quando há afastamento médico.");

        var acidente = new RegistroAcidente
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            FuncionarioId = dto.FuncionarioId,
            NomeFuncionario = dto.NomeFuncionario,
            Tipo = dto.Tipo,
            DataHoraAcidente = dto.DataHoraAcidente,
            Local = dto.Local,
            DescricaoAcidente = dto.DescricaoAcidente,
            AfastamentoMedico = dto.AfastamentoMedico,
            DiasAfastamento = dto.DiasAfastamento
        };

        await _repo.AddAcidenteAsync(acidente, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new AcidenteDto(
            acidente.Id, acidente.ObraId, acidente.Tipo, acidente.NomeFuncionario,
            acidente.DescricaoAcidente, acidente.Local,
            acidente.AfastamentoMedico, acidente.DiasAfastamento,
            acidente.DataHoraAcidente, acidente.NumeroCAT, acidente.CreatedAt);
    }
}
