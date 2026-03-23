using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record CreateContratoCommand(Guid EmpresaId, CreateContratoDto Dto)
    : IRequest<ContratoDto>, ITenantRequest;

public class CreateContratoCommandHandler : IRequestHandler<CreateContratoCommand, ContratoDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateContratoCommandHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<ContratoDto> Handle(CreateContratoCommand request, CancellationToken cancellationToken)
    {
        var count = await _repo.GetCountByEmpresaAsync(request.EmpresaId, cancellationToken);
        var numero = $"CTR-{(count + 1):D4}";
        var dto = request.Dto;

        var contrato = new Contrato
        {
            EmpresaId = request.EmpresaId,
            Numero = numero,
            ObraId = dto.ObraId,
            FornecedorId = dto.FornecedorId,
            Objeto = dto.Objeto,
            Descricao = dto.Descricao,
            Tipo = dto.Tipo,
            Status = StatusContratoEnum.Rascunho,
            ValorGlobal = dto.ValorGlobal,
            PercentualRetencao = dto.PercentualRetencao,
            DataAssinatura = dto.DataAssinatura,
            DataVigenciaInicio = dto.DataVigenciaInicio,
            DataVigenciaFim = dto.DataVigenciaFim,
            CondicoesPagamento = dto.CondicoesPagamento,
            DiasParaMedicao = dto.DiasParaMedicao,
            DiasParaPagamento = dto.DiasParaPagamento,
            ArquivoUrl = dto.ArquivoUrl,
            AssinadoPor = dto.AssinadoPor,
            FiscalId = dto.FiscalId,
            Observacoes = dto.Observacoes
        };

        await _repo.AddAsync(contrato, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ContratoDto(
            contrato.Id, contrato.Numero, contrato.Objeto, contrato.Tipo, contrato.Status,
            contrato.ObraId, contrato.FornecedorId, null,
            contrato.ValorGlobal, 0, 0,
            contrato.DataAssinatura, contrato.DataVigenciaInicio, contrato.DataVigenciaFim,
            contrato.Observacoes, contrato.CreatedAt);
    }
}
