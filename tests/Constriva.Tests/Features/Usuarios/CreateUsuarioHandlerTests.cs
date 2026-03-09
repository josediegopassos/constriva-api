using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class CreateUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateUsuarioHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public CreateUsuarioHandlerTests()
    {
        _handler = new CreateUsuarioHandler(_usuarioRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarUsuario_ComSenhaHasheada()
    {
        _usuarioRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), null, default)).ReturnsAsync(false);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        Usuario? criado = null;
        _usuarioRepo.Setup(r => r.AddAsync(It.IsAny<Usuario>(), default))
            .Callback<Usuario, CancellationToken>((u, _) => criado = u)
            .Returns(Task.CompletedTask);

        var cmd = new CreateUsuarioCommand(false, EmpresaId, new CreateUsuarioDto(
            "João Silva", "joao@empresa.com", "Senha@123!", null, null, PerfilUsuarioEnum.Engenheiro, EmpresaId));

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("João Silva");
        criado!.PasswordHash.Should().NotBe("Senha@123!");
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoEmailJaExiste()
    {
        // Não-super-admin: verificação de e-mail é por tenant via GetByEmailAndEmpresaAsync
        _usuarioRepo.Setup(r => r.GetByEmailAndEmpresaAsync("joao@empresa.com", EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Email = "joao@empresa.com" });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new CreateUsuarioCommand(false, EmpresaId, new CreateUsuarioDto(
                "João", "joao@empresa.com", "Senha@123!", null, null, PerfilUsuarioEnum.AdminEmpresa, EmpresaId)),
                default));
    }
}
