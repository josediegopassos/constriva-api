using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record UpdateObraCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, UpdateObraDto Dto)
    : IRequest<Unit>, ITenantRequest;

public class UpdateObraCommandHandler : IRequestHandler<UpdateObraCommand, Unit>
{
    private readonly IObraRepository _repo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;
    public UpdateObraCommandHandler(IObraRepository repo, IClienteRepository clienteRepo, IUnitOfWork uow)
    { _repo = repo; _clienteRepo = clienteRepo; _uow = uow; }

    public async Task<Unit> Handle(UpdateObraCommand r, CancellationToken ct)
    {
        var obra = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.Id} não encontrada.");

        var dto = r.Dto;

        if (dto.Nome != null)              obra.Nome = dto.Nome;
        if (dto.Tipo.HasValue)             obra.Tipo = dto.Tipo.Value;
        if (dto.TipoContrato.HasValue)     obra.TipoContrato = dto.TipoContrato.Value;
        if (dto.ClienteId.HasValue)
        {
            obra.ClienteId = dto.ClienteId;
            var cliente = await _clienteRepo.GetByIdAndEmpresaAsync(dto.ClienteId.Value, r.EmpresaId, ct);
            if (cliente != null) obra.NomeCliente = cliente.Nome;
        }
        else if (dto.NomeCliente != null)  obra.NomeCliente = dto.NomeCliente;
        if (dto.ResponsavelTecnico != null) obra.ResponsavelTecnico = dto.ResponsavelTecnico;
        if (dto.CreaResponsavel != null)   obra.CreaResponsavel = dto.CreaResponsavel;
        if (dto.Descricao != null)         obra.Descricao = dto.Descricao;
        if (dto.Observacoes != null)       obra.Observacoes = dto.Observacoes;
        if (dto.NumeroART != null)         obra.NumeroART = dto.NumeroART;
        if (dto.NumeroRRT != null)         obra.NumeroRRT = dto.NumeroRRT;
        if (dto.NumeroAlvara != null)      obra.NumeroAlvara = dto.NumeroAlvara;
        if (dto.ValidadeAlvara.HasValue)   obra.ValidadeAlvara = dto.ValidadeAlvara;
        if (dto.AreaTotal.HasValue)        obra.AreaTotal = dto.AreaTotal;
        if (dto.AreaConstruida.HasValue)   obra.AreaConstruida = dto.AreaConstruida;
        if (dto.NumeroAndares.HasValue)    obra.NumeroAndares = dto.NumeroAndares;
        if (dto.NumeroUnidades.HasValue)   obra.NumeroUnidades = dto.NumeroUnidades;
        if (dto.ValorContrato.HasValue)    obra.ValorContrato = dto.ValorContrato.Value;
        if (dto.ValorOrcado.HasValue)      obra.ValorOrcado = dto.ValorOrcado.Value;
        if (dto.DataInicioPrevista.HasValue) obra.DataInicioPrevista = dto.DataInicioPrevista.Value;
        if (dto.DataFimPrevista.HasValue)  obra.DataFimPrevista = dto.DataFimPrevista.Value;
        if (dto.DataInicioReal.HasValue)   obra.DataInicioReal = dto.DataInicioReal;
        if (dto.DataFimReal.HasValue)      obra.DataFimReal = dto.DataFimReal;
        if (dto.Logradouro != null)        obra.Logradouro = dto.Logradouro;
        if (dto.Numero != null)            obra.Numero = dto.Numero;
        if (dto.Complemento != null)       obra.Complemento = dto.Complemento;
        if (dto.Bairro != null)            obra.Bairro = dto.Bairro;
        if (dto.Cidade != null)            obra.Cidade = dto.Cidade;
        if (dto.Estado != null)            obra.Estado = dto.Estado;
        if (dto.Cep != null)               obra.Cep = dto.Cep;
        if (dto.Latitude.HasValue)         obra.Latitude = dto.Latitude;
        if (dto.Longitude.HasValue)        obra.Longitude = dto.Longitude;
        if (dto.FotoUrl != null)           obra.FotoUrl = dto.FotoUrl;

        obra.UpdatedBy = r.UsuarioId;
        obra.UpdatedAt = DateTime.UtcNow;

        _repo.Update(obra);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
