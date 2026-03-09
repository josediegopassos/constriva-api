using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record UpdateObraCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, UpdateObraDto Dto)
    : IRequest<Unit>, ITenantRequest;

public class UpdateObraCommandHandler : IRequestHandler<UpdateObraCommand, Unit>
{
    private readonly IObraRepository _repo;
    private readonly IUnitOfWork _uow;
    public UpdateObraCommandHandler(IObraRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(UpdateObraCommand r, CancellationToken ct)
    {
        var obra = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.Id} não encontrada.");

        obra.Nome = r.Dto.Nome ?? obra.Nome;
        obra.Tipo = r.Dto.Tipo;
        obra.NomeCliente = r.Dto.NomeCliente ?? obra.NomeCliente;
        obra.ResponsavelTecnico = r.Dto.ResponsavelTecnico ?? obra.ResponsavelTecnico;
        obra.Descricao = r.Dto.Descricao ?? obra.Descricao;
        obra.Observacoes = r.Dto.Observacoes ?? obra.Observacoes;
        obra.ValorContrato = r.Dto.ValorContrato ?? obra.ValorContrato;
        if (r.Dto.DataInicioPrevista.HasValue) obra.DataInicioPrevista = r.Dto.DataInicioPrevista.Value;
        if (r.Dto.DataFimPrevista.HasValue) obra.DataFimPrevista = r.Dto.DataFimPrevista.Value;
        if (!string.IsNullOrEmpty(r.Dto.Logradouro)) obra.Logradouro = r.Dto.Logradouro;
        if (!string.IsNullOrEmpty(r.Dto.Numero)) obra.Numero = r.Dto.Numero;
        if (r.Dto.Complemento != null) obra.Complemento = r.Dto.Complemento;
        if (!string.IsNullOrEmpty(r.Dto.Bairro)) obra.Bairro = r.Dto.Bairro;
        if (!string.IsNullOrEmpty(r.Dto.FotoUrl)) obra.FotoUrl = r.Dto.FotoUrl;
        obra.UpdatedBy = r.UsuarioId;
        obra.UpdatedAt = DateTime.UtcNow;

        _repo.Update(obra);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

