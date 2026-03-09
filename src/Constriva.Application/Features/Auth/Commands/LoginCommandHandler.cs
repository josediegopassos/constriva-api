using MediatR;
using BCrypt.Net;
using Constriva.Application.Features.Auth.DTOs;
using Constriva.Application.Features.Auth.Interfaces;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Senha, Guid? EmpresaId = null) : IRequest<AuthResponseDto>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private const int MaxTentativas = 5;
    private const int LockoutMinutes = 30;

    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _uow;

    public LoginCommandHandler(
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

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLowerInvariant().Trim();

        var usuario = request.EmpresaId.HasValue
            ? await _usuarioRepo.GetByEmailAndEmpresaAsync(email, request.EmpresaId.Value, cancellationToken)
            : await _usuarioRepo.GetByEmailAsync(email, cancellationToken);

        // Resposta genérica para usuário não encontrado ou deletado
        if (usuario == null || usuario.IsDeleted)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        // Lockout ativo
        if (usuario.BloqueadoAte.HasValue && usuario.BloqueadoAte > DateTime.UtcNow)
            throw new UnauthorizedAccessException(
                $"Conta bloqueada até {usuario.BloqueadoAte.Value.ToLocalTime():dd/MM/yyyy HH:mm}. Tente novamente mais tarde.");

        // Senha incorreta
        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.PasswordHash))
        {
            usuario.TentativasLogin++;

            if (usuario.TentativasLogin >= MaxTentativas)
            {
                usuario.BloqueadoAte = DateTime.UtcNow.AddMinutes(LockoutMinutes);
                usuario.TentativasLogin = 0;
            }

            _usuarioRepo.Update(usuario);
            await _uow.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        // Login bem-sucedido: gerar tokens e atualizar estado
        var (accessToken, refreshToken, expiresAt) = _jwtService.GenerateTokens(usuario);

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);
        usuario.UltimoAcesso = DateTime.UtcNow;
        usuario.TentativasLogin = 0;
        usuario.BloqueadoAte = null;

        _usuarioRepo.Update(usuario);

        var permissoes = await _usuarioRepo.GetPermissoesAsync(usuario.Id, cancellationToken);

        Empresa? empresa = null;
        if (usuario.EmpresaId.HasValue)
            empresa = await _empresaRepo.GetByIdAsync(usuario.EmpresaId.Value, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var usuarioDto = new UsuarioDto(
            usuario.Id,
            usuario.EmpresaId,
            usuario.Nome,
            usuario.Email,
            null,
            usuario.Perfil.ToString(),
            usuario.IsSuperAdmin,
            usuario.IsAdminEmpresa,
            empresa?.RazaoSocial,
            empresa == null ? null : new ModulosEmpresaDto(
                empresa.ModuloObras, empresa.ModuloEstoque, empresa.ModuloCronograma,
                empresa.ModuloOrcamento, empresa.ModuloCompras, empresa.ModuloQualidade,
                empresa.ModuloContratos, empresa.ModuloRH, empresa.ModuloFinanceiro,
                empresa.ModuloSST, empresa.ModuloGED, empresa.ModuloRelatorios),
            permissoes.Select(p => new PermissaoDto(p.Modulo, p.PodeVisualizar, p.PodeCriar, p.PodeEditar, p.PodeDeletar, p.PodeAprovar, p.PodeExportar, p.PodeAdministrar)));

        return new AuthResponseDto(accessToken, refreshToken, expiresAt, usuarioDto);
    }
}
