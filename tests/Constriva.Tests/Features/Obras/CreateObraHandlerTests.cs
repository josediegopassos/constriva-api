using Constriva.Application.Features.Obras.Commands;
using Constriva.Application.Features.Obras.DTOs;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class CreateObraHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateObraCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    public CreateObraHandlerTests()
    {
        _handler = new CreateObraCommandHandler(_obraRepo.Object, _uow.Object);
    }

    private static CreateObraDto DefaultDto() => new(
        "Residencial Vista Mar", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
        null, "João Responsável",
        DateTime.Today, DateTime.Today.AddMonths(12),
        5_000_000m,
        "Rua das Flores, 100", "1000", null, "Centro",
        "São Paulo", "SP", "01310-100");

    [Fact]
    public async Task Handle_DeveCriarObra_QuandoDadosValidos()
    {
        _obraRepo.Setup(r => r.AddAsync(It.IsAny<Obra>(), default)).Returns(Task.CompletedTask);
        _obraRepo.Setup(r => r.CountByEmpresaAsync(EmpresaId, null, default)).ReturnsAsync(0);
        _obraRepo.Setup(r => r.GetByCodigoAsync(EmpresaId, It.IsAny<string>(), default)).ReturnsAsync((Obra?)null);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _handler.Handle(new CreateObraCommand(EmpresaId, UserId, DefaultDto()), default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Residencial Vista Mar");
        result.Status.Should().Be(StatusObraEnum.Orcamento);
        _obraRepo.Verify(r => r.AddAsync(It.IsAny<Obra>(), default), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveAssociarEmpresaECriador_NaObra()
    {
        Obra? obraCriada = null;
        _obraRepo.Setup(r => r.AddAsync(It.IsAny<Obra>(), default))
            .Callback<Obra, CancellationToken>((o, _) => obraCriada = o)
            .Returns(Task.CompletedTask);
        _obraRepo.Setup(r => r.CountByEmpresaAsync(EmpresaId, null, default)).ReturnsAsync(0);
        _obraRepo.Setup(r => r.GetByCodigoAsync(EmpresaId, It.IsAny<string>(), default)).ReturnsAsync((Obra?)null);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(new CreateObraCommand(EmpresaId, UserId, DefaultDto()), default);

        obraCriada.Should().NotBeNull();
        obraCriada!.EmpresaId.Should().Be(EmpresaId);
        obraCriada.CreatedBy.Should().Be(UserId);
    }

    [Fact]
    public async Task Handle_DeveGerarCodigo_NoFormatoCorreto()
    {
        Obra? obraCriada = null;
        _obraRepo.Setup(r => r.AddAsync(It.IsAny<Obra>(), default))
            .Callback<Obra, CancellationToken>((o, _) => obraCriada = o)
            .Returns(Task.CompletedTask);
        _obraRepo.Setup(r => r.CountByEmpresaAsync(EmpresaId, null, default)).ReturnsAsync(0);
        _obraRepo.Setup(r => r.GetByCodigoAsync(EmpresaId, It.IsAny<string>(), default)).ReturnsAsync((Obra?)null);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(new CreateObraCommand(EmpresaId, UserId, DefaultDto()), default);

        obraCriada!.Codigo.Should().NotBeNullOrEmpty();
        obraCriada.Codigo.Should().Contain("OBR-");
    }

    [Fact]
    public async Task Handle_DeveIncrementarSequencial_QuandoCodigoJaExiste()
    {
        Obra? obraCriada = null;
        _obraRepo.Setup(r => r.AddAsync(It.IsAny<Obra>(), default))
            .Callback<Obra, CancellationToken>((o, _) => obraCriada = o)
            .Returns(Task.CompletedTask);
        _obraRepo.Setup(r => r.CountByEmpresaAsync(EmpresaId, null, default)).ReturnsAsync(0);
        _obraRepo.Setup(r => r.GetByCodigoAsync(EmpresaId, "OBR-0001", default))
            .ReturnsAsync(new Obra { EmpresaId = EmpresaId });
        _obraRepo.Setup(r => r.GetByCodigoAsync(EmpresaId, "OBR-0002", default))
            .ReturnsAsync((Obra?)null);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(new CreateObraCommand(EmpresaId, UserId, DefaultDto()), default);

        obraCriada!.Codigo.Should().Be("OBR-0002");
    }
}
