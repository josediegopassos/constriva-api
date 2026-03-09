using Constriva.Application.Features.Contratos.Commands;
using Constriva.Application.Features.Contratos.DTOs;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Contratos;

public class CreateContratoHandlerTests
{
    private readonly Mock<IContratoRepository> _contratoRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateContratoCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();
    private static readonly Guid FornecedorId = Guid.NewGuid();

    public CreateContratoHandlerTests()
    {
        _handler = new CreateContratoCommandHandler(_contratoRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarContrato_ComDadosValidos()
    {
        _contratoRepo.Setup(r => r.AddAsync(It.IsAny<Contrato>(), default)).Returns(Task.CompletedTask);
        _contratoRepo.Setup(r => r.GetCountByEmpresaAsync(EmpresaId, default)).ReturnsAsync(0);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateContratoDto(
            ObraId, FornecedorId, "Contrato de empreitada para fundações",
            TipoContratoFornecedorEnum.Global,
            500_000m, DateTime.Today, DateTime.Today, DateTime.Today.AddMonths(6));

        var result = await _handler.Handle(new CreateContratoCommand(EmpresaId, dto), default);

        result.Should().NotBeNull();
        result.Objeto.Should().Be("Contrato de empreitada para fundações");
        result.Tipo.Should().Be(TipoContratoFornecedorEnum.Global);
        result.Status.Should().Be(StatusContratoEnum.Ativo);
        result.ValorGlobal.Should().Be(500_000m);
    }

    [Fact]
    public async Task Handle_DeveGerarNumeroContrato_NoFormatoCorreto()
    {
        Contrato? contratoCriado = null;
        _contratoRepo.Setup(r => r.AddAsync(It.IsAny<Contrato>(), default))
            .Callback<Contrato, CancellationToken>((c, _) => contratoCriado = c)
            .Returns(Task.CompletedTask);
        _contratoRepo.Setup(r => r.GetCountByEmpresaAsync(EmpresaId, default)).ReturnsAsync(0);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateContratoDto(
            ObraId, FornecedorId, "Contrato de fornecimento",
            TipoContratoFornecedorEnum.Global,
            100_000m, DateTime.Today, DateTime.Today, DateTime.Today.AddMonths(3));

        await _handler.Handle(new CreateContratoCommand(EmpresaId, dto), default);

        contratoCriado!.Numero.Should().NotBeNullOrEmpty();
        contratoCriado.Numero.Should().Contain("CTR-");
    }
}
