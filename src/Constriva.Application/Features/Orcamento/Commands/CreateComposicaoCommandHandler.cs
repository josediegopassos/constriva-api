using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record CreateComposicaoCommand(Guid OrcamentoId, Guid EmpresaId, CreateComposicaoDto Dto)
    : IRequest<ComposicaoOrcamentoDto>, ITenantRequest;

public class CreateComposicaoHandler : IRequestHandler<CreateComposicaoCommand, ComposicaoOrcamentoDto>
{
    private readonly IComposicaoOrcamentoRepository _composicaoRepo;
    private readonly IUnitOfWork _uow;

    public CreateComposicaoHandler(IComposicaoOrcamentoRepository composicaoRepo, IUnitOfWork uow)
    {
        _composicaoRepo = composicaoRepo;
        _uow = uow;
    }

    public async Task<ComposicaoOrcamentoDto> Handle(CreateComposicaoCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var composicao = new ComposicaoOrcamento
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = request.OrcamentoId,
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            UnidadeMedida = dto.UnidadeMedida,
            Fonte = dto.Fonte,
            CodigoFonte = dto.CodigoFonte,
            Observacoes = dto.Observacoes
        };

        await _composicaoRepo.AddAsync(composicao, ct);
        await _uow.SaveChangesAsync(ct);

        return OrcamentoMapper.ToComposicaoDto(composicao);
    }
}
