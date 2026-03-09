using Constriva.Application.Features.Obras.Commands;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class DeleteObraHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IContratoRepository> _contratoRepo = new();
    private readonly Mock<IOrcamentoRepository> _orcamentoRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly DeleteObraCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public DeleteObraHandlerTests()
    {
        _handler = new DeleteObraCommandHandler(_obraRepo.Object, _contratoRepo.Object, _orcamentoRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveMarcarComoDeleted_QuandoObraExiste()
    {
        var obraId = Guid.NewGuid();
        var obra = new Obra { EmpresaId = EmpresaId, IsDeleted = false, Status = StatusObraEnum.Orcamento };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);
        _obraRepo.Setup(r => r.Update(It.IsAny<Obra>()));
        _contratoRepo.Setup(r => r.GetPagedAsync(EmpresaId, obraId, null, 1, 1, default))
            .ReturnsAsync((Enumerable.Empty<Domain.Entities.Contratos.Contrato>(), 0));
        _orcamentoRepo.Setup(r => r.GetByObraAsync(obraId, EmpresaId, default))
            .ReturnsAsync(Enumerable.Empty<Domain.Entities.Orcamento.Orcamento>());
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(new DeleteObraCommand(obraId, EmpresaId, Guid.NewGuid()), default);

        obra.IsDeleted.Should().BeTrue();
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_NaoDeveSalvar_QuandoObraNaoEncontrada()
    {
        var obraId = Guid.NewGuid();
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default))
            .ReturnsAsync((Obra?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(new DeleteObraCommand(obraId, EmpresaId, Guid.NewGuid()), default));

        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoExistemContratosVinculados()
    {
        var obraId = Guid.NewGuid();
        var obra = new Obra { EmpresaId = EmpresaId, Status = StatusObraEnum.Aprovada };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);
        _contratoRepo.Setup(r => r.GetPagedAsync(EmpresaId, obraId, null, 1, 1, default))
            .ReturnsAsync((Enumerable.Empty<Domain.Entities.Contratos.Contrato>(), 1));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new DeleteObraCommand(obraId, EmpresaId, Guid.NewGuid()), default));

        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoExistemOrcamentosVinculados()
    {
        var obraId = Guid.NewGuid();
        var obra = new Obra { EmpresaId = EmpresaId, Status = StatusObraEnum.Orcamento };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);
        _contratoRepo.Setup(r => r.GetPagedAsync(EmpresaId, obraId, null, 1, 1, default))
            .ReturnsAsync((Enumerable.Empty<Domain.Entities.Contratos.Contrato>(), 0));
        _orcamentoRepo.Setup(r => r.GetByObraAsync(obraId, EmpresaId, default))
            .ReturnsAsync([new Domain.Entities.Orcamento.Orcamento { EmpresaId = EmpresaId }]);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new DeleteObraCommand(obraId, EmpresaId, Guid.NewGuid()), default));

        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }
}
