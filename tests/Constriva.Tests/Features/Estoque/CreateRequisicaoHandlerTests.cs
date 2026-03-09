using Constriva.Application.Features.Estoque.Commands;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Estoque;

public class CreateRequisicaoHandlerTests
{
    private readonly Mock<IEstoqueRepository> _repo;
    private readonly Mock<IObraRepository> _obraRepo;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly CreateRequisicaoHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    public CreateRequisicaoHandlerTests()
    {
        _repo = new Mock<IEstoqueRepository>();
        _obraRepo = new Mock<IObraRepository>();
        _uow = new Mock<IUnitOfWork>();
        _handler = new CreateRequisicaoHandler(_repo.Object, _obraRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarRequisicao_ComStatusPendente()
    {
        var obraId = Guid.NewGuid();
        var almoxId = Guid.NewGuid();

        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Obra { EmpresaId = EmpresaId });
        _repo.Setup(r => r.GetAlmoxarifadoByIdAsync(almoxId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Almoxarifado { EmpresaId = EmpresaId });
        _repo.Setup(r => r.AddRequisicaoAsync(It.IsAny<RequisicaoMaterial>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var cmd = new CreateRequisicaoCommand(EmpresaId, UserId,
            new CreateRequisicaoDto(obraId, almoxId, "Cimento Portland"));

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Status.Should().Be(StatusRequisicaoEnum.Pendente);
        result.ObraId.Should().Be(obraId);
        result.AlmoxarifadoId.Should().Be(almoxId);
        result.Motivo.Should().Be("Cimento Portland");
        result.Numero.Should().StartWith("REQ-");
        _repo.Verify(r => r.AddRequisicaoAsync(It.IsAny<RequisicaoMaterial>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoObraNaoEncontrada()
    {
        var obraId = Guid.NewGuid();
        var almoxId = Guid.NewGuid();

        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Obra?)null);

        var cmd = new CreateRequisicaoCommand(EmpresaId, UserId,
            new CreateRequisicaoDto(obraId, almoxId, "Material"));

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{obraId}*");
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoAlmoxarifadoNaoEncontrado()
    {
        var obraId = Guid.NewGuid();
        var almoxId = Guid.NewGuid();

        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Obra { EmpresaId = EmpresaId });
        _repo.Setup(r => r.GetAlmoxarifadoByIdAsync(almoxId, EmpresaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Almoxarifado?)null);

        var cmd = new CreateRequisicaoCommand(EmpresaId, UserId,
            new CreateRequisicaoDto(obraId, almoxId, "Material"));

        var act = async () => await _handler.Handle(cmd, default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{almoxId}*");
    }
}
