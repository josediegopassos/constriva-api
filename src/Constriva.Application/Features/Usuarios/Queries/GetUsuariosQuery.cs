using MediatR;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Application.Features.Usuarios;

public record GetUsuariosQuery(Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin, Guid? FilterEmpresaId = null, string? Search = null, bool? Ativo = null, int Page = 1, int PageSize = 20) : IRequest<PaginatedResult<UsuarioDto>>;

public class GetUsuariosHandler : IRequestHandler<GetUsuariosQuery, PaginatedResult<UsuarioDto>>
{
    private readonly IUsuarioRepository _repo;
    public GetUsuariosHandler(IUsuarioRepository repo) => _repo = repo;
    public async Task<PaginatedResult<UsuarioDto>> Handle(GetUsuariosQuery r, CancellationToken ct)
    {
        var empresaId = r.RequesterIsSuperAdmin ? r.FilterEmpresaId : r.RequesterEmpresaId;
        var (items, total) = await _repo.GetPagedAsync(empresaId, r.Search, r.Ativo, r.Page, r.PageSize, ct);
        return new PaginatedResult<UsuarioDto> { Items = items.Select(Map), TotalCount = total, Page = r.Page, PageSize = r.PageSize };
    }
    private static UsuarioDto Map(Usuario u) => new(u.Id, u.Nome, u.Email, u.Perfil, u.Perfil.ToString(), u.Telefone, u.Cargo, u.AvatarUrl, u.Ativo, u.IsSuperAdmin, u.IsAdminEmpresa, u.EmpresaId, u.UltimoAcesso, u.CreatedAt);
}
