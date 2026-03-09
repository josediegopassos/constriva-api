using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class ResetSenhaHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly ResetSenhaHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public ResetSenhaHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new ResetSenhaHandler(_usuarioRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveInvalidarRefreshToken_AoResetarSenha()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario
        {
            EmpresaId = EmpresaId,
            PasswordHash = "old-hash",
            RefreshToken = "token-ativo",
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(30)
        };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);

        await _handler.Handle(new ResetSenhaCommand(usuarioId, EmpresaId, false, "NovaSenha@123"), default);

        usuario.RefreshToken.Should().BeNull();
        usuario.RefreshTokenExpiry.Should().BeNull();
        _usuarioRepo.Verify(r => r.Update(usuario), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoCruzaTenant()
    {
        var usuarioId = Guid.NewGuid();
        var outraEmpresa = Guid.NewGuid();
        var usuario = new Usuario { EmpresaId = outraEmpresa };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(new ResetSenhaCommand(usuarioId, EmpresaId, false, "Nova@123"), default));
    }
}
