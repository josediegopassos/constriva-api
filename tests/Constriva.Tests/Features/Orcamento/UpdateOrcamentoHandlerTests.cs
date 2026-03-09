using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class UpdateOrcamentoHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UpdateOrcamentoHandlerTests()
    {
        _handler = new UpdateOrcamentoHandler(_orcRepo.Object, _uow.Object);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveAtualizar_QuandoOrcamentoExisteERascunho()
    {
        var orcId = Guid.NewGuid();
        var orcamento = CriarOrcamento(orcId, StatusOrcamentoEnum.Rascunho);
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));

        var cmd = new UpdateOrcamentoCommand(orcId, EmpresaId,
            new UpdateOrcamentoDto("Novo Nome", 30m, DateTime.Today));

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Novo Nome");
        result.BDI.Should().Be(30m);
        _orcRepo.Verify(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoAprovado()
    {
        var orcId = Guid.NewGuid();
        var orcamento = CriarOrcamento(orcId, StatusOrcamentoEnum.Aprovado);
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        var cmd = new UpdateOrcamentoCommand(orcId, EmpresaId,
            new UpdateOrcamentoDto("Novo Nome", 30m, DateTime.Today));

        await _handler.Invoking(h => h.Handle(cmd, default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovado*");
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFoundException_QuandoOrcamentoNaoEncontrado()
    {
        var orcId = Guid.NewGuid();
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default))
            .ReturnsAsync((Domain.Entities.Orcamento.Orcamento?)null);

        var cmd = new UpdateOrcamentoCommand(orcId, EmpresaId,
            new UpdateOrcamentoDto("Nome", 20m, DateTime.Today));

        await _handler.Invoking(h => h.Handle(cmd, default))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    private static Domain.Entities.Orcamento.Orcamento CriarOrcamento(Guid id, StatusOrcamentoEnum status) =>
        new() { EmpresaId = EmpresaId, Nome = "Orçamento Original", Status = status, BDI = 25m };
}
