using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Orcamento;

public class AprovarOrcamentoHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AprovarOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid UsuarioId = Guid.NewGuid();

    public AprovarOrcamentoHandlerTests()
    {
        _handler = new AprovarOrcamentoHandler(_orcRepo.Object, _uow.Object);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _orcRepo.Setup(r => r.AddHistoricoAsync(It.IsAny<OrcamentoHistorico>(), default))
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task Handle_DeveAprovar_QuandoOrcamentoValido()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.EmAnalise,
            ValorTotal = 500_000m, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));

        await _handler.Handle(new AprovarOrcamentoCommand(orcId, EmpresaId, UsuarioId, "Aprovado OK"), default);

        orcamento.Status.Should().Be(StatusOrcamentoEnum.Aprovado);
        orcamento.AprovadoPor.Should().Be(UsuarioId);
        orcamento.DataAprovacao.Should().NotBeNull();
        orcamento.Observacoes.Should().Be("Aprovado OK");
    }

    [Fact]
    public async Task Handle_DeveSerIdempotente_QuandoJaAprovado()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Aprovado, Nome = "Orc", ValorTotal = 100m
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        await _handler.Handle(new AprovarOrcamentoCommand(orcId, EmpresaId, UsuarioId, null), default);

        _orcRepo.Verify(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoValorTotalZero()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.EmAnalise, ValorTotal = 0, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        await _handler.Invoking(h => h.Handle(
                new AprovarOrcamentoCommand(orcId, EmpresaId, UsuarioId, null), default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*zero*");
    }
}
