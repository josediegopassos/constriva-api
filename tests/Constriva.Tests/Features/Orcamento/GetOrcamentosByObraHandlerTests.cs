using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Queries;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Orcamento;

public class GetOrcamentosByObraHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly GetOrcamentosByObraHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();

    public GetOrcamentosByObraHandlerTests()
    {
        _handler = new GetOrcamentosByObraHandler(_orcRepo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarListaDeOrcamentos()
    {
        var orcamentos = new List<Domain.Entities.Orcamento.Orcamento>
        {
            new() { EmpresaId = EmpresaId, ObraId = ObraId,
                    Nome = "Orc V2", Versao = 2, Status = StatusOrcamentoEnum.Aprovado },
            new() { EmpresaId = EmpresaId, ObraId = ObraId,
                    Nome = "Orc V1", Versao = 1, Status = StatusOrcamentoEnum.Rascunho },
        };

        _orcRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default))
            .ReturnsAsync(orcamentos);

        var result = await _handler.Handle(new GetOrcamentosByObraQuery(ObraId, EmpresaId), default);

        var list = result.ToList();
        list.Should().HaveCount(2);
        list[0].Nome.Should().Be("Orc V2");
        list[1].Nome.Should().Be("Orc V1");
    }

    [Fact]
    public async Task Handle_DeveRetornarListaVazia_QuandoNaoHaOrcamentos()
    {
        _orcRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default))
            .ReturnsAsync(new List<Domain.Entities.Orcamento.Orcamento>());

        var result = await _handler.Handle(new GetOrcamentosByObraQuery(ObraId, EmpresaId), default);

        result.Should().BeEmpty();
    }
}
