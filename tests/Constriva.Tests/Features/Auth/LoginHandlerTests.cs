using FluentAssertions;
using Moq;
using Constriva.Application.Features.Auth;
using Constriva.Application.Features.Auth.Interfaces;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Auth.Commands;

namespace Constriva.Tests.Features.Auth;

public class LoginHandlerTests
{
    private readonly Mock<IUsuarioRepository> _repo = new();
    private readonly Mock<IEmpresaRepository> _empresaRepo = new();
    private readonly Mock<IJwtService> _jwt = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly LoginCommandHandler _handler;

    public LoginHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new LoginCommandHandler(_repo.Object, _empresaRepo.Object, _jwt.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveAutenticar_QuandoCredenciaisValidas()
    {
        var empresaId = Guid.NewGuid();
        var usuario = new Usuario
        {
            Email = "joao@empresa.com.br",
            Nome = "João Silva",
            EmpresaId = empresaId,
            Ativo = true,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
        };

        _repo.Setup(r => r.GetByEmailAsync("joao@empresa.com.br", default)).ReturnsAsync(usuario);
        _repo.Setup(r => r.GetPermissoesAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(new List<UsuarioPermissao>());
        _jwt.Setup(j => j.GenerateTokens(It.IsAny<Usuario>()))
            .Returns(("eyJ.token.here", "refresh-token-xyz", DateTime.UtcNow.AddHours(1)));

        var result = await _handler.Handle(
            new LoginCommand("joao@empresa.com.br", "Senha@123"), default);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("eyJ.token.here");
        result.RefreshToken.Should().Be("refresh-token-xyz");
        result.Usuario.Id.Should().Be(usuario.Id);
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorized_QuandoEmailNaoEncontrado()
    {
        _repo.Setup(r => r.GetByEmailAsync("naoexiste@email.com", default)).ReturnsAsync((Usuario?)null);

        var act = async () => await _handler.Handle(
            new LoginCommand("naoexiste@email.com", "qualquer"), default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorized_QuandoSenhaIncorreta()
    {
        var usuario = new Usuario
        {
            Email = "joao@email.com",
            Ativo = true,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta")
        };
        _repo.Setup(r => r.GetByEmailAsync("joao@email.com", default)).ReturnsAsync(usuario);

        var act = async () => await _handler.Handle(
            new LoginCommand("joao@email.com", "SenhaErrada"), default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_DeveLancarUnauthorized_QuandoUsuarioInativo()
    {
        var usuario = new Usuario
        {
            Email = "inativo@email.com",
            Ativo = false,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Senha@123")
        };
        _repo.Setup(r => r.GetByEmailAsync("inativo@email.com", default)).ReturnsAsync(usuario);

        var act = async () => await _handler.Handle(
            new LoginCommand("inativo@email.com", "Senha@123"), default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
