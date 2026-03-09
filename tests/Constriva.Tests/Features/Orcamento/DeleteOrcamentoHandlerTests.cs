using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Orcamento;

public class DeleteOrcamentoHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly DeleteOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public DeleteOrcamentoHandlerTests()
    {
        _handler = new DeleteOrcamentoHandler(_orcRepo.Object, _uow.Object);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveExcluir_QuandoOrcamentoRascunho()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));

        await _handler.Handle(new DeleteOrcamentoCommand(orcId, EmpresaId), default);

        orcamento.IsDeleted.Should().BeTrue();
        _orcRepo.Verify(r => r.Update(It.Is<Domain.Entities.Orcamento.Orcamento>(o => o.IsDeleted)), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoAprovado()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Aprovado, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        await _handler.Invoking(h => h.Handle(new DeleteOrcamentoCommand(orcId, EmpresaId), default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovado*");
    }
}
