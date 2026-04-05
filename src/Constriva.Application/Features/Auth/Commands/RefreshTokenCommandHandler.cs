using MediatR;
using Constriva.Application.Features.Auth.DTOs;
using Constriva.Application.Features.Auth.Interfaces;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _uow;

    public RefreshTokenCommandHandler(
        IUsuarioRepository usuarioRepo,
        IEmpresaRepository empresaRepo,
        IJwtService jwtService,
        IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _empresaRepo = empresaRepo;
        _jwtService = jwtService;
        _uow = uow;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepo.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        // Token não encontrado, usuário deletado ou inativo
        if (usuario == null || usuario.IsDeleted || !usuario.Ativo)
            throw new UnauthorizedAccessException("Refresh token inválido.");

        // Token expirado
        if (usuario.RefreshTokenExpiry == null || usuario.RefreshTokenExpiry <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token inválido.");

        // Conta bloqueada
        if (usuario.BloqueadoAte.HasValue && usuario.BloqueadoAte > DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token inválido.");

        // Rotacionar tokens
        var (accessToken, refreshToken, expiresAt) = _jwtService.GenerateTokens(usuario);

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);
        usuario.UltimoAcesso = DateTime.UtcNow;

        _usuarioRepo.Update(usuario);

        // Carregar permissões e empresa para UsuarioDto completo
        var permissoes = await _usuarioRepo.GetPermissoesAsync(usuario.Id, cancellationToken);

        Empresa? empresa = null;
        if (usuario.EmpresaId.HasValue)
            empresa = await _empresaRepo.GetByIdAsync(usuario.EmpresaId.Value, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var usuarioDto = new UsuarioDto(
            usuario.Id, usuario.EmpresaId, usuario.Nome, usuario.Email,
            null, usuario.Perfil.ToString(), usuario.IsSuperAdmin, usuario.IsAdminEmpresa,
            empresa?.RazaoSocial,
            empresa == null ? null : new ModulosEmpresaDto(
                empresa.ModuloObras, empresa.ModuloEstoque, empresa.ModuloCronograma,
                empresa.ModuloOrcamento, empresa.ModuloCompras, empresa.ModuloQualidade,
                empresa.ModuloContratos, empresa.ModuloRH, empresa.ModuloFinanceiro,
                empresa.ModuloSST, empresa.ModuloGED, empresa.ModuloRelatorios, empresa.ModuloClientes, empresa.ModuloFornecedores, empresa.ModuloAgente, empresa.ModuloLens),
            permissoes.Select(p => new PermissaoDto(p.Modulo, p.PodeVisualizar, p.PodeCriar, p.PodeEditar, p.PodeDeletar, p.PodeAprovar, p.PodeExportar, p.PodeAdministrar)));

        return new AuthResponseDto(accessToken, refreshToken, expiresAt, usuarioDto);
    }
}
