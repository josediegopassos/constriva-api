using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Empresas.Commands;
using Constriva.Application.Features.Empresas.DTOs;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Empresas;

public class CreateEmpresaHandlerTests
{
    private readonly Mock<IEmpresaRepository> _repo = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IEmailService> _emailService = new();
    private readonly CreateEmpresaCommandHandler _handler;

    public CreateEmpresaHandlerTests()
    {
        _handler = new CreateEmpresaCommandHandler(_repo.Object, _usuarioRepo.Object, _uow.Object, _emailService.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarEmpresa_QuandoCnpjDisponivel()
    {
        _repo.Setup(r => r.GetByCnpjAsync("12345678000190", default)).ReturnsAsync((Empresa?)null);
        _repo.Setup(r => r.AddAsync(It.IsAny<Empresa>(), default)).Returns(Task.CompletedTask);
        _usuarioRepo.Setup(r => r.AddAsync(It.IsAny<Usuario>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _emailService.Setup(e => e.SendTemplateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), default))
            .Returns(Task.CompletedTask);

        var dto = new CreateEmpresaDto("Construtora XYZ", "XYZ", "12.345.678/0001-90",
            "xyz@email.com", "(21) 99999-9999",
            "Av Central", "200", null, "Botafogo", "Rio de Janeiro", "RJ", "22250-040",
            PlanoEmpresaEnum.Basico, 5, 3,
            "Admin XYZ", "admin@xyz.com", "Admin@123");

        var result = await _handler.Handle(new CreateEmpresaCommand(dto), default);

        result.Should().NotBeNull();
        result.RazaoSocial.Should().Be("Construtora XYZ");
        result.Ativo.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_DeveLancarException_QuandoCnpjJaCadastrado()
    {
        _repo.Setup(r => r.GetByCnpjAsync("12345678000190", default))
            .ReturnsAsync(new Empresa { Cnpj = "12345678000190" });

        var dto = new CreateEmpresaDto("Duplicada", "DUP", "12.345.678/0001-90",
            "dup@email.com", "(11) 99999-9999",
            "Rua B", "1", null, "Bairro", "São Paulo", "SP", "00000-000",
            PlanoEmpresaEnum.Basico, 5, 3,
            "Admin", "admin@dup.com", "Admin@123");

        var act = async () => await _handler.Handle(new CreateEmpresaCommand(dto), default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*CNPJ*");
    }
}
