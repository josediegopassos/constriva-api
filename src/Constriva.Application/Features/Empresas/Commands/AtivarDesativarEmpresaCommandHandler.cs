using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Empresas.Commands;

public record AtivarDesativarEmpresaCommand(Guid Id, bool Ativo) : IRequest<bool>;

public class AtivarDesativarEmpresaHandler : IRequestHandler<AtivarDesativarEmpresaCommand, bool>
{
    private readonly IEmpresaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public AtivarDesativarEmpresaHandler(IEmpresaRepository repo, IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(AtivarDesativarEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (empresa == null || empresa.IsDeleted) return false;

        empresa.Status = request.Ativo ? StatusEmpresaEnum.Ativa : StatusEmpresaEnum.Suspensa;
        _repo.Update(empresa);

        // Ao desativar: revogar todas as sessões ativas dos usuários da empresa.
        // Nota: bloqueio individual de usuários é uma operação separada, gerenciada pelo módulo Usuários.
        if (!request.Ativo)
        {
            var usuarios = await _usuarioRepo.GetByEmpresaAsync(empresa.Id, cancellationToken);
            foreach (var u in usuarios.Where(u => !u.IsDeleted))
            {
                u.RefreshToken = null;
                u.RefreshTokenExpiry = null;
                _usuarioRepo.Update(u);
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
