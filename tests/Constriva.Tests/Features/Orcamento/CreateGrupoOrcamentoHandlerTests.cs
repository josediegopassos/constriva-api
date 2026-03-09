using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class CreateGrupoOrcamentoHandlerTests
{
    private readonly Mock<IGrupoOrcamentoRepository> _grupoRepo = new();
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateGrupoOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid OrcamentoId = Guid.NewGuid();

    public CreateGrupoOrcamentoHandlerTests()
    {
        _handler = new CreateGrupoOrcamentoHandler(_grupoRepo.Object, _orcRepo.Object, _uow.Object);
        _grupoRepo.Setup(r => r.AddAsync(It.IsAny<GrupoOrcamento>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveCriarGrupo_QuandoOrcamentoRascunho()
    {
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Rascunho, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);
        _grupoRepo.Setup(r => r.GetMaxOrdemAsync(OrcamentoId, default)).ReturnsAsync(2);

        var cmd = new CreateGrupoOrcamentoCommand(OrcamentoId, EmpresaId, new CreateGrupoDto("Serviços Preliminares"));

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Serviços Preliminares");
        result.Ordem.Should().Be(3);
        result.Codigo.Should().Be("03");
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoOrcamentoAprovado()
    {
        var orcamento = new Domain.Entities.Orcamento.Orcamento
        {
            EmpresaId = EmpresaId,
            Status = StatusOrcamentoEnum.Aprovado, Nome = "Orc"
        };
        _orcRepo.Setup(r => r.GetByIdAsync(OrcamentoId, EmpresaId, default)).ReturnsAsync(orcamento);

        var cmd = new CreateGrupoOrcamentoCommand(OrcamentoId, EmpresaId, new CreateGrupoDto("Grupo"));

        await _handler.Invoking(h => h.Handle(cmd, default))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*aprovado*");
    }
}
