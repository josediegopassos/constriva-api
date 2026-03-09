using Constriva.Application.Features.GED;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.GED;

public class GetPastasHandlerTests
{
    private readonly Mock<IGEDRepository> _repo = new();
    private readonly GetPastasHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetPastasHandlerTests()
    {
        _handler = new GetPastasHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDePastas()
    {
        _repo.Setup(r => r.GetPastasAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<PastaDocumento>
            {
                new() { Nome = "Projetos", Ativo = true }
            });

        var result = await _handler.Handle(new GetPastasQuery(EmpresaId), default);

        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("Projetos");
    }

    [Fact]
    public async Task Handle_DeveFiltrarPorObraId()
    {
        var obraId = Guid.NewGuid();
        _repo.Setup(r => r.GetPastasAsync(EmpresaId, obraId, default))
            .ReturnsAsync(new List<PastaDocumento>());

        await _handler.Handle(new GetPastasQuery(EmpresaId, obraId), default);

        _repo.Verify(r => r.GetPastasAsync(EmpresaId, obraId, default), Times.Once);
    }
}
