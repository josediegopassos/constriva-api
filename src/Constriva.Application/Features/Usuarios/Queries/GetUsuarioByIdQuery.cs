using MediatR;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Usuarios.DTOs;

namespace Constriva.Application.Features.Usuarios;

public record GetUsuarioByIdQuery(Guid Id, Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin) : IRequest<UsuarioDetalheDto?>;

public class GetUsuarioByIdHandler : IRequestHandler<GetUsuarioByIdQuery, UsuarioDetalheDto?>
{
    private readonly IUsuarioRepository _repo;
    public GetUsuarioByIdHandler(IUsuarioRepository repo) => _repo = repo;
    public async Task<UsuarioDetalheDto?> Handle(GetUsuarioByIdQuery r, CancellationToken ct)
    {
        var usuario = await _repo.GetWithPermissoesAsync(r.Id, ct);
        if (usuario == null || usuario.IsDeleted) return null;
        if (!r.RequesterIsSuperAdmin && usuario.EmpresaId != r.RequesterEmpresaId) return null;
        var perm = usuario.Permissoes.Select(p => new PermissaoDto(p.Id, p.Modulo, p.PodeVisualizar, p.PodeCriar, p.PodeEditar, p.PodeDeletar, p.PodeAprovar, p.PodeExportar, p.PodeAdministrar));
        return new UsuarioDetalheDto(new(usuario.Id, usuario.Nome, usuario.Email, usuario.Perfil, usuario.Perfil.ToString(), usuario.Telefone, usuario.Cargo, usuario.AvatarUrl, usuario.Ativo, usuario.IsSuperAdmin, usuario.IsAdminEmpresa, usuario.EmpresaId, usuario.UltimoAcesso, usuario.CreatedAt), perm);
    }
}
