using Constriva.Application.Features.Obras.Commands;
using Constriva.Application.Features.Obras.DTOs;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class UpdateObraHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateObraCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    public UpdateObraHandlerTests()
    {
        _handler = new UpdateObraCommandHandler(_obraRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveAtualizar_QuandoObraExiste()
    {
        var obra = new Obra { EmpresaId = EmpresaId, Nome = "Obra Antiga" };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(ObraId, EmpresaId, default)).ReturnsAsync(obra);
        _obraRepo.Setup(r => r.Update(It.IsAny<Obra>()));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new UpdateObraDto("Obra Atualizada", TipoObraEnum.Comercial,
            null, "Gerente", null, null,
            "Av. Paulista, 1000", "1", null, "Bela Vista",
            DateTime.Today, DateTime.Today.AddMonths(24), 10_000_000m, null);
        var cmd = new UpdateObraCommand(ObraId, EmpresaId, UserId, dto);

        await _handler.Handle(cmd, default);

        obra.Nome.Should().Be("Obra Atualizada");
        obra.Tipo.Should().Be(TipoObraEnum.Comercial);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoObraNaoEncontrada()
    {
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(ObraId, EmpresaId, default))
            .ReturnsAsync((Obra?)null);

        var cmd = new UpdateObraCommand(ObraId, EmpresaId, UserId,
            new UpdateObraDto("Nome", TipoObraEnum.Residencial, null, null, null, null,
                null, null, null, null, null, null, null, null));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(cmd, default));
    }
}
