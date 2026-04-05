using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record CreateCotacaoCommand(Guid EmpresaId, Guid UsuarioId, CreateCotacaoDto Dto)
    : IRequest<CotacaoDto>, ITenantRequest;

public class CreateCotacaoHandler : IRequestHandler<CreateCotacaoCommand, CotacaoDto>
{
    private readonly IComprasRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IUnitOfWork _uow;

    public CreateCotacaoHandler(IComprasRepository repo, IObraRepository obraRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _uow = uow;
    }

    public async Task<CotacaoDto> Handle(CreateCotacaoCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        var numero = $"CT-{DateTime.UtcNow:yyMMddHHmmss}";

        var cotacao = new Cotacao
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            Numero = numero,
            Titulo = dto.Titulo,
            Status = StatusCotacaoEnum.Aberta,
            DataAbertura = DateTime.UtcNow,
            DataLimiteResposta = dto.DataLimiteResposta,
            Observacoes = dto.Observacoes,
            CondicoesGerais = dto.CondicoesGerais,
            CriadoPor = request.UsuarioId
        };

        var ordem = 1;
        foreach (var item in dto.Itens ?? Enumerable.Empty<CreateItemCotacaoDto>())
        {
            cotacao.Itens.Add(new ItemCotacao
            {
                EmpresaId = request.EmpresaId,
                MaterialId = item.MaterialId,
                Descricao = item.Descricao,
                UnidadeMedida = item.UnidadeMedida,
                Quantidade = item.Quantidade,
                Especificacao = item.Especificacao,
                PrecoReferencia = item.PrecoReferencia,
                Ordem = ordem++
            });
        }

        await _repo.AddCotacaoAsync(cotacao, ct);
        await _uow.SaveChangesAsync(ct);

        return CotacaoMapper.ToDto(cotacao);
    }
}
