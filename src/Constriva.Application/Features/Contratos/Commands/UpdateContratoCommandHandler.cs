using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record UpdateContratoCommand(Guid Id, Guid EmpresaId, UpdateContratoDto Dto)
    : IRequest<ContratoDetalheDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateContratoHandler : IRequestHandler<UpdateContratoCommand, ContratoDetalheDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateContratoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<ContratoDetalheDto> Handle(UpdateContratoCommand request, CancellationToken cancellationToken)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Contrato {request.Id} não encontrado.");

        var dto = request.Dto;

        if (dto.DataVigenciaFim <= dto.DataVigenciaInicio)
            throw new InvalidOperationException("A data de fim deve ser posterior à data de início do contrato.");

        contrato.Objeto = dto.Objeto;
        contrato.Tipo = dto.Tipo;
        contrato.FornecedorId = dto.FornecedorId;
        contrato.Descricao = dto.Descricao;
        contrato.ValorGlobal = dto.ValorGlobal;
        contrato.PercentualRetencao = dto.PercentualRetencao;
        contrato.DataAssinatura = dto.DataAssinatura;
        contrato.DataVigenciaInicio = dto.DataVigenciaInicio;
        contrato.DataVigenciaFim = dto.DataVigenciaFim;
        contrato.CondicoesPagamento = dto.CondicoesPagamento;
        contrato.DiasParaMedicao = dto.DiasParaMedicao;
        contrato.DiasParaPagamento = dto.DiasParaPagamento;
        contrato.ArquivoUrl = dto.ArquivoUrl;
        contrato.AssinadoPor = dto.AssinadoPor;
        contrato.FiscalId = dto.FiscalId;
        contrato.Observacoes = dto.Observacoes;
        contrato.Status = StatusContratoEnum.Rascunho;

        _repo.Update(contrato);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ContratoDetalheDto(
            contrato.Id, contrato.Numero, contrato.Objeto, contrato.Descricao,
            contrato.Tipo, contrato.Status,
            contrato.ObraId, contrato.FornecedorId, null,
            contrato.ValorGlobal, contrato.ValorAditivos, contrato.ValorTotal,
            contrato.ValorMedidoAcumulado, contrato.ValorPagoAcumulado,
            contrato.PercentualRetencao, contrato.ValorRetencao,
            contrato.CondicoesPagamento, contrato.DiasParaMedicao, contrato.DiasParaPagamento,
            contrato.DataAssinatura, contrato.DataVigenciaInicio, contrato.DataVigenciaFim,
            contrato.DataEncerramento,
            contrato.ArquivoUrl, contrato.AssinadoPor, contrato.FiscalId,
            contrato.Observacoes, contrato.CreatedAt);
    }
}
