using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Tests.Features.Orcamento;

public class CreateOrcamentoHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateOrcamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    public CreateOrcamentoHandlerTests()
    {
        _handler = new CreateOrcamentoHandler(_orcRepo.Object, _uow.Object);
        _orcRepo.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Orcamento.Orcamento>(), default))
            .Returns(Task.CompletedTask);
        _orcRepo.Setup(r => r.AddHistoricoAsync(It.IsAny<OrcamentoHistorico>(), default))
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_DeveRetornarOrcamentoCriado_QuandoDadosValidos()
    {
        _orcRepo.Setup(r => r.GetMaxVersaoAsync(ObraId, EmpresaId, default)).ReturnsAsync(0);
        var cmd = BuildCreateCommand("Orçamento Principal", 25m);

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Orçamento Principal");
        result.BDI.Should().Be(25m);
        result.Versao.Should().Be(1);
        result.Status.Should().Be(StatusOrcamentoEnum.Rascunho);
        result.ValorTotal.Should().Be(0);
        _orcRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Orcamento.Orcamento>(), default), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveIncrementarVersao_QuandoJaExisteOrcamento()
    {
        _orcRepo.Setup(r => r.GetMaxVersaoAsync(ObraId, EmpresaId, default)).ReturnsAsync(2);
        var cmd = BuildCreateCommand("Orçamento V3", 20m);

        var result = await _handler.Handle(cmd, default);

        result.Versao.Should().Be(3);
    }

    [Fact]
    public async Task Handle_DeveCriarCodigoCorreto_ComBaseNoObraId()
    {
        _orcRepo.Setup(r => r.GetMaxVersaoAsync(ObraId, EmpresaId, default)).ReturnsAsync(0);
        var cmd = BuildCreateCommand("Orçamento", 20m);

        var result = await _handler.Handle(cmd, default);

        result.Codigo.Should().StartWith("ORC-");
        result.Codigo.Should().EndWith("-V1");
    }

    [Fact]
    public async Task Handle_DeveRegistrarHistorico_QuandoCriado()
    {
        _orcRepo.Setup(r => r.GetMaxVersaoAsync(ObraId, EmpresaId, default)).ReturnsAsync(0);
        var cmd = BuildCreateCommand("Orçamento com Histórico", 15m);

        await _handler.Handle(cmd, default);

        _orcRepo.Verify(r => r.AddHistoricoAsync(
            It.Is<OrcamentoHistorico>(h => h.Descricao == "Orçamento criado." && h.UsuarioId == UserId),
            default), Times.Once);
    }

    private CreateOrcamentoCommand BuildCreateCommand(string nome, decimal bdi) =>
        new(ObraId, EmpresaId, UserId,
            new CreateOrcamentoDto(nome, bdi, DateTime.Today,
                "Orçamento para obra residencial", "SINAPI 01/2024", "SP"));
}
