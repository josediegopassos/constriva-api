using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record CreateObraCommand(Guid EmpresaId, Guid UsuarioId, CreateObraDto Dto)
    : IRequest<ObraResumoDto>, ITenantRequest;

public class CreateObraCommandHandler : IRequestHandler<CreateObraCommand, ObraResumoDto>
{
    private readonly IObraRepository _repo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;
    public CreateObraCommandHandler(IObraRepository repo, IClienteRepository clienteRepo, IUnitOfWork uow)
    { _repo = repo; _clienteRepo = clienteRepo; _uow = uow; }

    public async Task<ObraResumoDto> Handle(CreateObraCommand r, CancellationToken ct)
    {
        var count = await _repo.CountByEmpresaAsync(r.EmpresaId, null, ct);
        var seq = count + 1;
        string codigo;
        do { codigo = $"OBR-{seq:D4}"; seq++; }
        while (await _repo.GetByCodigoAsync(r.EmpresaId, codigo, ct) != null);

        string? nomeCliente = r.Dto.NomeCliente;
        if (r.Dto.ClienteId.HasValue && nomeCliente == null)
        {
            var cliente = await _clienteRepo.GetByIdAndEmpresaAsync(r.Dto.ClienteId.Value, r.EmpresaId, ct);
            nomeCliente = cliente?.Nome;
        }

        var obra = new Obra
        {
            EmpresaId = r.EmpresaId, CreatedBy = r.UsuarioId,
            Codigo = codigo, Nome = r.Dto.Nome, Tipo = r.Dto.Tipo,
            TipoContrato = r.Dto.TipoContrato,
            ClienteId = r.Dto.ClienteId, NomeCliente = nomeCliente,
            ResponsavelTecnico = r.Dto.ResponsavelTecnico, Descricao = r.Dto.Descricao,
            DataInicioPrevista = r.Dto.DataInicioPrevista, DataFimPrevista = r.Dto.DataFimPrevista,
            ValorContrato = r.Dto.ValorContrato,
            ValorOrcado = r.Dto.ValorOrcado,
            Endereco = new Endereco
            {
                EmpresaId = r.EmpresaId,
                Logradouro = r.Dto.Logradouro,
                Numero = r.Dto.Numero,
                Complemento = r.Dto.Complemento,
                Bairro = r.Dto.Bairro,
                Cidade = r.Dto.Cidade,
                Estado = r.Dto.Estado,
                Cep = r.Dto.Cep,
                Latitude = r.Dto.Latitude,
                Longitude = r.Dto.Longitude
            },
            Observacoes = r.Dto.Observacoes,
            CreaResponsavel = r.Dto.CreaResponsavel,
            NumeroART = r.Dto.NumeroART,
            NumeroRRT = r.Dto.NumeroRRT,
            NumeroAlvara = r.Dto.NumeroAlvara,
            ValidadeAlvara = r.Dto.ValidadeAlvara,
            AreaTotal = r.Dto.AreaTotal,
            AreaConstruida = r.Dto.AreaConstruida,
            NumeroAndares = r.Dto.NumeroAndares,
            NumeroUnidades = r.Dto.NumeroUnidades,
            Status = StatusObraEnum.Orcamento
        };

        await _repo.AddAsync(obra, ct);
        await _uow.SaveChangesAsync(ct);

        return new ObraResumoDto(obra.Id, obra.Codigo, obra.Nome, obra.Tipo,
            obra.Status, obra.Status.ToString(), obra.Endereco?.Cidade, obra.Endereco?.Estado,
            obra.DataInicioPrevista, obra.DataFimPrevista,
            obra.ValorContrato, obra.PercentualConcluido, false, null);
    }
}

