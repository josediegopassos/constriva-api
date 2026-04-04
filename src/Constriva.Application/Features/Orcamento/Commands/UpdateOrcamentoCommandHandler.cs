using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record UpdateOrcamentoCommand(Guid Id, Guid EmpresaId, UpdateOrcamentoDto Dto)
    : IRequest<OrcamentoResumoDto>, ITenantRequest;

public class UpdateOrcamentoHandler : IRequestHandler<UpdateOrcamentoCommand, OrcamentoResumoDto>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OrcamentoResumoDto> Handle(UpdateOrcamentoCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível editar um orçamento aprovado.");

        var dto = request.Dto;
        orcamento.Nome = dto.Nome;
        orcamento.BDI = dto.BDI;
        orcamento.DataReferencia = dto.DataReferencia;
        orcamento.Descricao = dto.Descricao;
        orcamento.Observacoes = dto.Observacoes;
        orcamento.BaseOrcamentaria = dto.BaseOrcamentaria;
        orcamento.Localidade = dto.Localidade;
        orcamento.BDIDetalhado_Administracao = dto.BDIDetalhadoAdministracao;
        orcamento.BDIDetalhado_Seguro = dto.BDIDetalhadoSeguro;
        orcamento.BDIDetalhado_Risco = dto.BDIDetalhadoRisco;
        orcamento.BDIDetalhado_Financeiro = dto.BDIDetalhadoFinanceiro;
        orcamento.BDIDetalhado_Lucro = dto.BDIDetalhadoLucro;
        orcamento.BDIDetalhado_Tributos = dto.BDIDetalhadoTributos;
        orcamento.EncargosHoristas = dto.EncargosHoristas;
        orcamento.EncargosMensalistas = dto.EncargosMensalistas;

        _repo.Update(orcamento);
        await _uow.SaveChangesAsync(ct);

        var totalGrupos = orcamento.Grupos?.Count(g => !g.IsDeleted) ?? 0;
        return OrcamentoMapper.ToResumoDto(orcamento, totalGrupos);
    }
}
