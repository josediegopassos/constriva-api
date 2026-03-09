using Constriva.Application.Features.GED;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.GED;

public class GetDocumentosHandlerTests
{
    private readonly Mock<IGEDRepository> _repo = new();
    private readonly GetDocumentosHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetDocumentosHandlerTests()
    {
        _handler = new GetDocumentosHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarPaginado_ComDocumentos()
    {
        var docs = new List<DocumentoGED>
        {
            new() { Titulo = "Planta Baixa",
                Codigo = "PLT-001", Tipo = TipoDocumentoGEDEnum.Projeto,
                Arquivos = new List<ArquivoDocumento>
                {
                    new() { Atual = true, Url = "http://url.com/file",
                        NomeArquivo = "planta.pdf", TipoArquivo = "application/pdf",
                        TamanhoBytes = 1024 }
                }}
        };
        _repo.Setup(r => r.GetDocumentosPagedAsync(EmpresaId, null, null, null, 1, 20, default))
            .ReturnsAsync((docs, 1));

        var result = await _handler.Handle(new GetDocumentosQuery(EmpresaId), default);

        result.TotalCount.Should().Be(1);
        result.Items.Should().HaveCount(1);
        result.Items.First().Titulo.Should().Be("Planta Baixa");
    }
}
