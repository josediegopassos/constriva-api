using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateAlmoxarifadoCommand(Guid EmpresaId, CreateAlmoxarifadoDto Dto)
    : IRequest<AlmoxarifadoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateAlmoxarifadoHandler : IRequestHandler<CreateAlmoxarifadoCommand, AlmoxarifadoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAlmoxarifadoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AlmoxarifadoDto> Handle(CreateAlmoxarifadoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var almoxarifado = new Almoxarifado
        {
            EmpresaId = request.EmpresaId,
            Nome = dto.Nome,
            ObraId = dto.ObraId,
            Descricao = dto.Descricao,
            Logradouro = dto.Logradouro,
            Cidade = dto.Cidade,
            ResponsavelId = dto.ResponsavelId,
            Principal = dto.Principal,
            Ativo = true
        };

        await _repo.AddAlmoxarifadoAsync(almoxarifado, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var codigo = $"ALM-{almoxarifado.Id.ToString()[..8].ToUpper()}";
        return new AlmoxarifadoDto(almoxarifado.Id, almoxarifado.Nome, codigo, almoxarifado.ObraId,
            almoxarifado.Principal, almoxarifado.Descricao, almoxarifado.Logradouro,
            almoxarifado.Cidade, almoxarifado.ResponsavelId, almoxarifado.Ativo);
    }
}
