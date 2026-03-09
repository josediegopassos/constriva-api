using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class ImportarSinapiHandlerTests
{
    private readonly Mock<IGrupoOrcamentoRepository> _grupoRepo = new();
    private readonly Mock<IItemOrcamentoRepository> _itemRepo = new();
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly ImportarSinapiHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid OrcamentoId = Guid.NewGuid();
    private static readonly Guid GrupoId = Guid.NewGuid();

    public ImportarSinapiHandlerTests()
    {
        _handler = new ImportarSinapiHandler(
            _grupoRepo.Object, _itemRepo.Object, _orcRepo.Object, _uow.Object);

        _itemRepo.Setup(r => r.AddAsync(It.IsAny<ItemOrcamento>(), default)).Returns(Task.CompletedTask);
        _grupoRepo.Setup(r => r.Update(It.IsAny<GrupoOrcamento>()));
        _orcRepo.Setup(r => r.Update(It.IsAny<Domain.Entities.Orcamento.Orcamento>()));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveImportarItens_QuandoGrupoExiste()
    {
        var grupo = new GrupoOrcamento
        {
            EmpresaId = EmpresaId, OrcamentoId = OrcamentoId, Nome = "SINAPI", ValorTotal = 0
        };
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, BDI = 30m, Nome = "Orc"
        };

        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);
        _grupoRepo.Setup(r => r.GetByIdAsync(GrupoId, EmpresaId, default)).ReturnsAsync(grupo);
        _itemRepo.Setup(r => r.GetMaxOrdemAsync(GrupoId, default)).ReturnsAsync(0);

        var dto = new ImportarSinapiDto(GrupoId, new List<ItemSinapiDto>
        {
            new("74209/001", "CONCRETO FCKGREATER=25MPA", "m3", 350m, 10m),
            new("87453", "ARMAÇÃO AÇO CA-50", "kg", 8.50m, 500m),
        });

        var cmd = new ImportarSinapiCommand(OrcamentoId, EmpresaId, dto);

        var result = await _handler.Handle(cmd, default);

        result.Should().Be(2);
        _itemRepo.Verify(r => r.AddAsync(
            It.Is<ItemOrcamento>(i => i.Fonte == FontePrecoEnum.SINAPI), default),
            Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_DeveRetornarZero_QuandoListaVazia()
    {
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, BDI = 25m, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);

        var dto = new ImportarSinapiDto(null, new List<ItemSinapiDto>());
        var cmd = new ImportarSinapiCommand(OrcamentoId, EmpresaId, dto);

        var result = await _handler.Handle(cmd, default);

        result.Should().Be(0);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoAprovado()
    {
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Aprovado, BDI = 25m, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);

        var dto = new ImportarSinapiDto(null, new List<ItemSinapiDto>
        {
            new("123", "Item", "m2", 100m, 10m)
        });

        await _handler.Invoking(h => h.Handle(new ImportarSinapiCommand(OrcamentoId, EmpresaId, dto), default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovado*");
    }
}
