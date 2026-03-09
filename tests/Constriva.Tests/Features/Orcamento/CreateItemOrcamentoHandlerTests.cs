using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class CreateItemOrcamentoHandlerTests
{
    private readonly Mock<IItemOrcamentoRepository> _itemRepo = new();
    private readonly Mock<IGrupoOrcamentoRepository> _grupoRepo = new();
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateItemOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid OrcamentoId = Guid.NewGuid();
    private static readonly Guid GrupoId = Guid.NewGuid();

    public CreateItemOrcamentoHandlerTests()
    {
        _handler = new CreateItemOrcamentoHandler(
            _itemRepo.Object, _grupoRepo.Object, _orcRepo.Object, _uow.Object);

        _itemRepo.Setup(r => r.AddAsync(It.IsAny<ItemOrcamento>(), default)).Returns(Task.CompletedTask);
        _grupoRepo.Setup(r => r.Update(It.IsAny<GrupoOrcamento>()));
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveCriarItem_QuandoGrupoEOrcamentoValidos()
    {
        var grupo = new GrupoOrcamento
        {
            EmpresaId = EmpresaId,
            OrcamentoId = OrcamentoId, Nome = "Grupo", Codigo = "01"
        };
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, BDI = 25m, Nome = "Orc"
        };

        _grupoRepo.Setup(r => r.GetByIdAsync(GrupoId, EmpresaId, default)).ReturnsAsync(grupo);
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);
        _itemRepo.Setup(r => r.GetMaxOrdemAsync(GrupoId, default)).ReturnsAsync(0);

        var dto = new CreateItemOrcDto("Concreto Fck=25MPa", "m³", 100m, 450m);
        var cmd = new CreateItemOrcamentoCommand(GrupoId, EmpresaId, dto);

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Descricao.Should().Be("Concreto Fck=25MPa");
        result.Quantidade.Should().Be(100m);
        result.CustoUnitario.Should().Be(450m);
        result.CustoTotal.Should().Be(45_000m);
        result.BDI.Should().Be(25m);
        result.CustoComBDI.Should().Be(450m * 1.25m);
    }

    [Fact]
    public async Task Handle_DeveAtualizarTotalDoGrupo_QuandoItemAdicionado()
    {
        var grupo = new GrupoOrcamento
        {
            EmpresaId = EmpresaId,
            OrcamentoId = OrcamentoId, Nome = "Grupo",
            Codigo = "01", ValorTotal = 0
        };
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, BDI = 0m, Nome = "Orc",
            ValorCustoDirecto = 0
        };

        _grupoRepo.Setup(r => r.GetByIdAsync(GrupoId, EmpresaId, default)).ReturnsAsync(grupo);
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);
        _itemRepo.Setup(r => r.GetMaxOrdemAsync(GrupoId, default)).ReturnsAsync(0);

        var dto = new CreateItemOrcDto("Argamassa", "m²", 200m, 50m);
        var cmd = new CreateItemOrcamentoCommand(GrupoId, EmpresaId, dto);

        await _handler.Handle(cmd, default);

        grupo.ValorTotal.Should().Be(10_000m);
        orcamento.ValorCustoDirecto.Should().Be(10_000m);
        orcamento.ValorTotal.Should().Be(10_000m);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoAprovado()
    {
        var grupo = new GrupoOrcamento
        {
            EmpresaId = EmpresaId, OrcamentoId = OrcamentoId, Nome = "Grupo"
        };
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Aprovado, Nome = "Orc"
        };

        _grupoRepo.Setup(r => r.GetByIdAsync(GrupoId, EmpresaId, default)).ReturnsAsync(grupo);
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);

        var cmd = new CreateItemOrcamentoCommand(GrupoId, EmpresaId,
            new CreateItemOrcDto("Item", "un", 1, 100m));

        await _handler.Invoking(h => h.Handle(cmd, default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovado*");
    }
}
