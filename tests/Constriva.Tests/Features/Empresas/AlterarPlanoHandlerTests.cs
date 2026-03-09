using Constriva.Application.Features.Empresas.Commands;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Empresas;

public class AlterarPlanoHandlerTests
{
    private readonly Mock<IEmpresaRepository> _repo = new();
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AlterarPlanoHandler _handler;

    public AlterarPlanoHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new AlterarPlanoHandler(_repo.Object, _obraRepo.Object, _uow.Object);
    }

    private static Empresa EmpresaComUsuarios(int qtd)
    {
        var e = new Empresa { Cnpj = "00000000000000", Email = "a@a.com", Telefone = "0",
            Logradouro = "R", Numero = "1", Bairro = "B", Cidade = "C", Estado = "SP", Cep = "00000-000" };
        for (var i = 0; i < qtd; i++)
            e.Usuarios.Add(new Usuario { Nome = $"User{i}", Email = $"u{i}@a.com",
                PasswordHash = "x", Ativo = true });
        return e;
    }

    [Fact]
    public async Task Handle_DeveAlterarPlano_QuandoDadosValidos()
    {
        var empresa = EmpresaComUsuarios(3);
        _repo.Setup(r => r.GetWithUsuariosAsync(It.IsAny<Guid>(), default)).ReturnsAsync(empresa);
        // Empresa tem 3 obras cadastradas; novo limite é 5 → aprovado
        _obraRepo.Setup(r => r.CountByEmpresaAsync(It.IsAny<Guid>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(3);

        var cmd = new AlterarPlanoCommand(Guid.NewGuid(), PlanoEmpresaEnum.Profissional, 10, 5,
            DateTime.UtcNow.AddYears(1), null);

        var result = await _handler.Handle(cmd, default);

        result.Should().BeTrue();
        empresa.Plano.Should().Be(PlanoEmpresaEnum.Profissional);
        empresa.MaxUsuarios.Should().Be(10);
        _repo.Verify(r => r.Update(empresa), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoDowngradeAbaixoUsoAtual()
    {
        var empresa = EmpresaComUsuarios(5);
        _repo.Setup(r => r.GetWithUsuariosAsync(It.IsAny<Guid>(), default)).ReturnsAsync(empresa);

        var cmd = new AlterarPlanoCommand(Guid.NewGuid(), PlanoEmpresaEnum.Basico, 3, 2,
            DateTime.UtcNow.AddYears(1), null);

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*3 usuários*5 usuários ativos*");
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoDowngradeObrasAbaixoUsoAtual()
    {
        var empresa = EmpresaComUsuarios(0);
        _repo.Setup(r => r.GetWithUsuariosAsync(It.IsAny<Guid>(), default)).ReturnsAsync(empresa);
        // Empresa tem 4 obras; tentar reduzir para MaxObras=2 → erro
        _obraRepo.Setup(r => r.CountByEmpresaAsync(It.IsAny<Guid>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(4);

        var cmd = new AlterarPlanoCommand(Guid.NewGuid(), PlanoEmpresaEnum.Basico, 10, 2,
            DateTime.UtcNow.AddYears(1), null);

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*2 obras*4 obras cadastradas*");
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoDataVencimentoNoPassado()
    {
        var empresa = EmpresaComUsuarios(0);
        _repo.Setup(r => r.GetWithUsuariosAsync(It.IsAny<Guid>(), default)).ReturnsAsync(empresa);

        var cmd = new AlterarPlanoCommand(Guid.NewGuid(), PlanoEmpresaEnum.Profissional, 10, 5,
            DateTime.UtcNow.AddDays(-1), null);

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*DataVencimento*");
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoMaxUsuariosZero()
    {
        var cmd = new AlterarPlanoCommand(Guid.NewGuid(), PlanoEmpresaEnum.Profissional, 0, 5,
            DateTime.UtcNow.AddYears(1), null);

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*MaxUsuarios*");
    }
}
