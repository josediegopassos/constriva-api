using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.GED.Commands;
using Constriva.Application.Features.GED.DTOs;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.GED;

public class UploadDocumentoHandlerTests
{
    private readonly Mock<IGEDRepository> _repo = new();
    private readonly Mock<IFileStorageService> _storage = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UploadDocumentoCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UploadDocumentoHandlerTests()
    {
        _handler = new UploadDocumentoCommandHandler(_repo.Object, _storage.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveUploadarDocumento_ERetornarDto()
    {
        var pastaId = Guid.NewGuid();
        _storage.Setup(s => s.UploadAsync(
            It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/doc.pdf");
        _repo.Setup(r => r.AddDocumentoAsync(It.IsAny<DocumentoGED>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new UploadDocumentoDto(
            pastaId, "PLT-003", "Planta Estrutural Rev.3",
            TipoDocumentoGEDEnum.Projeto, null, null);

        var result = await _handler.Handle(new UploadDocumentoCommand(EmpresaId, dto), default);

        result.Should().NotBeNull();
        result.Titulo.Should().Be("Planta Estrutural Rev.3");
        _repo.Verify(r => r.AddDocumentoAsync(It.IsAny<DocumentoGED>(), default), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}
