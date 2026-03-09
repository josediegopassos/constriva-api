using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class UpdateItemOrcamentoHandlerTests
{
    private readonly Mock<IItemOrcamentoRepository> _itemRepo = new();
    private readonly Mock<IGrupoOrcamentoRepository> _grupoRepo = new();
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateItemOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UpdateItemOrcamentoHandlerTests()
    {
        _handler = new UpdateItemOrcamentoHandler(
            _itemRepo.Object, _grupoRepo.Object, _orcRepo.Object, _uow.Object);
        _itemRepo.Setup(r => r.Update(It.IsAny<ItemOrcamento>()));
        _grupoRepo.Setup(r => r.Update(It.IsAny<GrupoOrcamento>()));
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveAtualizarItem_ERecalcularTotais()
    {
        var itemId = Guid.NewGuid();
        var grupoId = Guid.NewGuid();
        var orcId = Guid.NewGuid();

        var item = new ItemOrcamento
        {
            EmpresaId = EmpresaId,
            GrupoId = grupoId, OrcamentoId = orcId,
            Descricao = "Item Original",
            Quantidade = 100m, CustoUnitario = 50m, BDI = 20m
        };
        var grupo = new GrupoOrcamento
        {
            EmpresaId = EmpresaId,
            OrcamentoId = orcId, ValorTotal = 5_000m
        };
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho,
            ValorCustoDirecto = 5_000m, BDI = 20m, Nome = "Orc"
        };

        _itemRepo.Setup(r => r.GetByIdAsync(itemId, EmpresaId, default)).ReturnsAsync(item);
        _grupoRepo.Setup(r => r.GetByIdAsync(grupoId, EmpresaId, default)).ReturnsAsync(grupo);
        _orcRepo.Setup(r => r.GetByIdAsync(orcId, EmpresaId, default)).ReturnsAsync(orcamento);

        var dto = new UpdateItemOrcDto("Item Atualizado", "m³", 150m, 60m);
        var cmd = new UpdateItemOrcamentoCommand(itemId, EmpresaId, dto);

        var result = await _handler.Handle(cmd, default);

        result.Descricao.Should().Be("Item Atualizado");
        result.Quantidade.Should().Be(150m);
        result.CustoUnitario.Should().Be(60m);
        result.CustoTotal.Should().Be(9_000m);

        grupo.ValorTotal.Should().Be(9_000m);
        orcamento.ValorCustoDirecto.Should().Be(9_000m);
    }
}
