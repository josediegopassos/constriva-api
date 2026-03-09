using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class CreateUsuarioTenantHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateUsuarioHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public CreateUsuarioTenantHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new CreateUsuarioHandler(_usuarioRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveNormalizarEmail_AntesDeVerificarDuplicidade()
    {
        // Não-super-admin: a verificação de e-mail é por tenant via GetByEmailAndEmpresaAsync
        string? emailVerificado = null;
        _usuarioRepo.Setup(r => r.GetByEmailAndEmpresaAsync(It.IsAny<string>(), EmpresaId, It.IsAny<CancellationToken>()))
            .Callback<string, Guid, CancellationToken>((e, _, _) => emailVerificado = e)
            .ReturnsAsync((Usuario?)null);
        _usuarioRepo.Setup(r => r.AddAsync(It.IsAny<Usuario>(), default)).Returns(Task.CompletedTask);

        var cmd = new CreateUsuarioCommand(false, EmpresaId, new CreateUsuarioDto(
            "João", "  JOAO@EMPRESA.COM  ", "Senha@123!", null, null, PerfilUsuarioEnum.Colaborador, EmpresaId));

        await _handler.Handle(cmd, default);

        emailVerificado.Should().Be("joao@empresa.com");
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoNaoSuperAdminCriaNaEmpresaErrada()
    {
        var outraEmpresa = Guid.NewGuid();
        var cmd = new CreateUsuarioCommand(false, EmpresaId,
            new CreateUsuarioDto("João", "j@a.com", "Senha@123!", null, null, PerfilUsuarioEnum.Colaborador, outraEmpresa));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(cmd, default));
    }

    [Fact]
    public async Task Handle_DevePermitir_QuandoSuperAdminCriaEmQualquerEmpresa()
    {
        var outraEmpresa = Guid.NewGuid();
        _usuarioRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), null, default)).ReturnsAsync(false);
        _usuarioRepo.Setup(r => r.AddAsync(It.IsAny<Usuario>(), default)).Returns(Task.CompletedTask);

        var cmd = new CreateUsuarioCommand(true, EmpresaId,
            new CreateUsuarioDto("João", "j@a.com", "Senha@123!", null, null, PerfilUsuarioEnum.Colaborador, outraEmpresa));

        var result = await _handler.Handle(cmd, default);
        result.Should().NotBeNull();
    }
}
