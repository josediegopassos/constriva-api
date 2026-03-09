using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Constriva.Application.Features.Auth;
using Constriva.Application.Features.Auth.Interfaces;
using Constriva.Domain.Entities.Tenant;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Constriva.Infrastructure.Identity;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public (string AccessToken, string RefreshToken, DateTime ExpiresAt) GenerateTokens(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresMinutes"] ?? "60"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Name, usuario.Nome),
            new("perfil", usuario.Perfil.ToString()),
            new("isSuperAdmin", usuario.IsSuperAdmin.ToString()),
            new("isAdminEmpresa", usuario.IsAdminEmpresa.ToString()),
        };

        if (usuario.EmpresaId.HasValue)
            claims.Add(new("empresaId", usuario.EmpresaId.Value.ToString()));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return (accessToken, refreshToken, expires);
    }

    public Guid? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = true
            }, out var validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;
            var userId = jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(userId);
        }
        catch { return null; }
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}

public class CurrentUserService : Constriva.Application.Common.Interfaces.ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claim != null ? Guid.Parse(claim) : Guid.Empty;
        }
    }

    public Guid? EmpresaId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("empresaId")?.Value;
            return claim != null ? Guid.Parse(claim) : null;
        }
    }

    public string Email => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    public bool IsSuperAdmin => _httpContextAccessor.HttpContext?.User.FindFirst("isSuperAdmin")?.Value == "True";
    public bool IsAdminEmpresa => _httpContextAccessor.HttpContext?.User.FindFirst("isAdminEmpresa")?.Value == "True";
    public string Perfil => _httpContextAccessor.HttpContext?.User.FindFirst("perfil")?.Value ?? "";

    public bool HasPermission(string modulo, string acao)
    {
        if (IsSuperAdmin || IsAdminEmpresa) return true;
        // In real scenario, load from cache
        return true;
    }
}
