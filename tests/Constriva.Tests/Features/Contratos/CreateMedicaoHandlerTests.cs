using Constriva.Application.Features.Contratos.Commands;
using Constriva.Application.Features.Contratos.DTOs;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Contratos;

public class CreateMedicaoHandlerTests
{
    private readonly Mock<IContratoRepository> _contratoRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateMedicaoCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ContratoId = Guid.NewGuid();

    public CreateMedicaoHandlerTests()
    {
        _handler = new CreateMedicaoCommandHandler(_contratoRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarMedicao_QuandoContratoVigente()
    {
        var contrato = new Contrato
        {
            EmpresaId = EmpresaId,
            Status = StatusContratoEnum.Ativo, ValorGlobal = 500_000m
        };
        _contratoRepo.Setup(r => r.GetByIdAndEmpresaAsync(ContratoId, EmpresaId, default)).ReturnsAsync(contrato);
        _contratoRepo.Setup(r => r.GetMedicoesAsync(ContratoId, EmpresaId, default)).ReturnsAsync(new List<MedicaoContratual>());
        _contratoRepo.Setup(r => r.AddMedicaoAsync(It.IsAny<MedicaoContratual>(), default)).Returns(Task.CompletedTask);
        _contratoRepo.Setup(r => r.GetTotalMedicoesAsync(ContratoId, default)).ReturnsAsync(0m);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateMedicaoDto(
            "MED-001",
            DateTime.Today.AddDays(-30), DateTime.Today,
            100_000m, 20m, "Medição #1 - Fundação");

        var result = await _handler.Handle(new CreateMedicaoCommand(ContratoId, EmpresaId, dto), default);

        result.Should().NotBeNull();
        result.ValorMedicao.Should().Be(100_000m);
        result.Periodo.Should().Be(1);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoContratoNaoVigente()
    {
        var contrato = new Contrato
        {
            EmpresaId = EmpresaId,
            Status = StatusContratoEnum.Encerrado
        };
        _contratoRepo.Setup(r => r.GetByIdAndEmpresaAsync(ContratoId, EmpresaId, default)).ReturnsAsync(contrato);
        _contratoRepo.Setup(r => r.GetMedicoesAsync(ContratoId, EmpresaId, default)).ReturnsAsync(new List<MedicaoContratual>());

        var dto = new CreateMedicaoDto("MED-001", DateTime.Today.AddDays(-30), DateTime.Today, 10_000m, 5m);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new CreateMedicaoCommand(ContratoId, EmpresaId, dto), default));
    }
}
