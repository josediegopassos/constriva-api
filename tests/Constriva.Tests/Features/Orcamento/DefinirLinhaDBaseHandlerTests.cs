using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Orcamento;

public class DefinirLinhaDBaseHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly DefinirLinhaDBaseHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();

    public DefinirLinhaDBaseHandlerTests()
    {
        _handler = new DefinirLinhaDBaseHandler(_orcRepo.Object, _uow.Object);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveDefinirLinhaDBase_QuandoOrcamentoAprovado()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId, ObraId = ObraId,
            Status = StatusOrcamentoEnum.Aprovado, ELinhaDBase = false, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);
        _orcRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default))
            .ReturnsAsync(new List<Domain.Entities.Orcamento.Orcamento> { orcamento });
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));

        await _handler.Handle(new DefinirLinhaDBaseCommand(orcId, EmpresaId), default);

        orcamento.ELinhaDBase.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_DeveRemoverLinhaDBaseAnterior_AoDefinirNova()
    {
        var orcId = Guid.NewGuid();
        var orcNovo = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId, ObraId = ObraId,
            Status = StatusOrcamentoEnum.Aprovado, ELinhaDBase = false, Nome = "Novo"
        };
        var orcAntigo = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId, ObraId = ObraId,
            Status = StatusOrcamentoEnum.Aprovado, ELinhaDBase = true, Nome = "Antigo"
        };

        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcNovo);
        _orcRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default))
            .ReturnsAsync(new List<Domain.Entities.Orcamento.Orcamento> { orcNovo, orcAntigo });
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));

        await _handler.Handle(new DefinirLinhaDBaseCommand(orcId, EmpresaId), default);

        orcNovo.ELinhaDBase.Should().BeTrue();
        orcAntigo.ELinhaDBase.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoNaoAprovado()
    {
        var orcId = Guid.NewGuid();
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        await _handler.Invoking(h => h.Handle(new DefinirLinhaDBaseCommand(orcId, EmpresaId), default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovados*");
    }
}
