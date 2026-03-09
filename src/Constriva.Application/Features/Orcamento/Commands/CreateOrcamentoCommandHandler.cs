using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record CreateOrcamentoCommand(Guid ObraId, Guid EmpresaId, Guid UsuarioId, CreateOrcamentoDto Dto)
    : IRequest<OrcamentoResumoDto>, ITenantRequest;

public class CreateOrcamentoHandler : IRequestHandler<CreateOrcamentoCommand, OrcamentoResumoDto>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OrcamentoResumoDto> Handle(CreateOrcamentoCommand request, CancellationToken ct)
    {
        var maxVersao = await _repo.GetMaxVersaoAsync(request.ObraId, request.EmpresaId, ct);
        var versao = maxVersao + 1;
        var dto = request.Dto;

        var obraIdShort = request.ObraId.ToString()[..8].ToUpper();
        var codigo = $"ORC-{obraIdShort}-V{versao}";

        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            ObraId = request.ObraId,
            EmpresaId = request.EmpresaId,
            Codigo = codigo,
            Nome = dto.Nome,
            Versao = versao,
            Status = StatusOrcamentoEnum.Rascunho,
            BDI = dto.BDI,
            DataReferencia = dto.DataReferencia,
            Descricao = dto.Descricao,
            BaseOrcamentaria = dto.BaseOrcamentaria,
            Localidade = dto.Localidade
        };

        await _repo.AddAsync(orcamento, ct);

        var historico = new OrcamentoHistorico
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = orcamento.Id,
            Descricao = "Orçamento criado.",
            UsuarioId = request.UsuarioId
        };

        await _repo.AddHistoricoAsync(historico, ct);
        await _uow.SaveChangesAsync(ct);

        return OrcamentoMapper.ToResumoDto(orcamento, 0);
    }
}
