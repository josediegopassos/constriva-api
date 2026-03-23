using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateAlmoxarifadoCommand(Guid Id, Guid EmpresaId, UpdateAlmoxarifadoDto Dto)
    : IRequest<AlmoxarifadoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAlmoxarifadoHandler : IRequestHandler<UpdateAlmoxarifadoCommand, AlmoxarifadoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAlmoxarifadoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AlmoxarifadoDto> Handle(UpdateAlmoxarifadoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var almoxarifado = await _repo.GetAlmoxarifadoByIdComEnderecoAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {request.Id} não encontrado.");

        almoxarifado.Nome = dto.Nome;
        almoxarifado.Descricao = dto.Descricao;
        almoxarifado.Endereco ??= new Endereco { EmpresaId = request.EmpresaId };
        almoxarifado.Endereco.Logradouro = dto.Logradouro;
        almoxarifado.Endereco.Cidade = dto.Cidade;
        almoxarifado.ResponsavelId = dto.ResponsavelId;
        almoxarifado.Principal = dto.Principal;

        await _uow.SaveChangesAsync(cancellationToken);

        var codigo = $"ALM-{almoxarifado.Id.ToString()[..8].ToUpper()}";
        return new AlmoxarifadoDto(almoxarifado.Id, almoxarifado.Nome, codigo, almoxarifado.ObraId,
            almoxarifado.Principal, almoxarifado.Descricao, almoxarifado.Endereco?.Logradouro,
            almoxarifado.Endereco?.Cidade, almoxarifado.ResponsavelId, almoxarifado.Ativo);
    }
}
