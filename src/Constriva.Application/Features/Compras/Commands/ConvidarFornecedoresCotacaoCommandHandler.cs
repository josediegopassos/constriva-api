using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record ConvidarFornecedoresCotacaoCommand(
    Guid CotacaoId,
    Guid EmpresaId,
    ConvidarFornecedoresDto Dto) : IRequest<IEnumerable<FornecedorCotacaoDto>>, ITenantRequest;

public class ConvidarFornecedoresCotacaoHandler
    : IRequestHandler<ConvidarFornecedoresCotacaoCommand, IEnumerable<FornecedorCotacaoDto>>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public ConvidarFornecedoresCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<IEnumerable<FornecedorCotacaoDto>> Handle(
        ConvidarFornecedoresCotacaoCommand request, CancellationToken ct)
    {
        var cotacao = await _repo.GetCotacaoByIdAsync(request.CotacaoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.CotacaoId} não encontrada.");

        if (cotacao.Status != StatusCotacaoEnum.Aberta)
            throw new InvalidOperationException(
                $"Cotação no status '{cotacao.Status}' não permite convites. Apenas cotações abertas podem receber fornecedores.");

        var existentes = await _repo.GetFornecedoresCotacaoAsync(request.CotacaoId, request.EmpresaId, ct);
        var idsExistentes = existentes.Select(f => f.FornecedorId).ToHashSet();

        var criados = new List<FornecedorCotacao>();

        foreach (var fornecedorId in request.Dto.FornecedorIds)
        {
            if (idsExistentes.Contains(fornecedorId))
                continue;

            var fc = new FornecedorCotacao
            {
                EmpresaId = request.EmpresaId,
                CotacaoId = request.CotacaoId,
                FornecedorId = fornecedorId,
                Status = StatusConviteCotacaoEnum.Convidado,
                ConvidadoEm = DateTime.UtcNow
            };

            await _repo.AddFornecedorCotacaoAsync(fc, ct);
            criados.Add(fc);
        }

        await _uow.SaveChangesAsync(ct);

        // Recarregar para obter dados de navegação (nome do fornecedor)
        var atualizados = await _repo.GetFornecedoresCotacaoAsync(request.CotacaoId, request.EmpresaId, ct);
        var idsCriados = criados.Select(c => c.FornecedorId).ToHashSet();

        return atualizados
            .Where(f => idsCriados.Contains(f.FornecedorId))
            .Select(f => new FornecedorCotacaoDto(
                f.Id, f.FornecedorId,
                f.Fornecedor?.NomeFantasia ?? f.Fornecedor?.RazaoSocial ?? "",
                f.Fornecedor?.Documento,
                f.Fornecedor?.Email,
                f.Status, f.ConvidadoEm, f.RespondeuEm));
    }
}
