using Constriva.Application.Features.Financeiro.Commands;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Financeiro;

public class BaixarLancamentoHandlerTests
{
    private readonly Mock<ILancamentoFinanceiroRepository> _repo;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly BaixarLancamentoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public BaixarLancamentoHandlerTests()
    {
        _repo = new Mock<ILancamentoFinanceiroRepository>();
        _uow = new Mock<IUnitOfWork>();
        _handler = new BaixarLancamentoHandler(_repo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveBaixarLancamento_ComDadosValidos()
    {
        var lancId = Guid.NewGuid();
        var lanc = new LancamentoFinanceiro
        {
            EmpresaId = EmpresaId, Descricao = "Parcela",
            Tipo = TipoLancamentoEnum.Despesa, Valor = 1000m,
            Status = StatusLancamentoEnum.Previsto, DataVencimento = DateTime.Today
        };
        _repo.Setup(r => r.GetByIdAndEmpresaAsync(lancId, EmpresaId, It.IsAny<CancellationToken>())).ReturnsAsync(lanc);
        _repo.Setup(r => r.Update(It.IsAny<LancamentoFinanceiro>()));
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var cmd = new BaixarLancamentoCommand(lancId, EmpresaId, 980m, DateTime.Today);

        await _handler.Handle(cmd, default);

        lanc.Status.Should().Be(StatusLancamentoEnum.Realizado);
        lanc.ValorRealizado.Should().Be(980m);
        lanc.DataPagamento.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoLancamentoNaoEncontrado()
    {
        var lancId = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAndEmpresaAsync(lancId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LancamentoFinanceiro?)null);

        var cmd = new BaixarLancamentoCommand(lancId, EmpresaId, 1000m, DateTime.Today);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(cmd, default));

        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }
}
