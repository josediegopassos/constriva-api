using Constriva.Application.Features.RH;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.RH;

public class GetFuncionariosHandlerTests
{
    private readonly Mock<IRHRepository> _repo = new();
    private readonly GetFuncionariosHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetFuncionariosHandlerTests()
    {
        _handler = new GetFuncionariosHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarFuncionariosPaginados()
    {
        var funcionarios = new List<Funcionario>
        {
            new() { Nome = "Maria Santos", Cpf = "111.222.333-44",
                SalarioBase = 4000m, Status = StatusFuncionarioEnum.Ativo }
        };
        _repo.Setup(r => r.GetFuncionariosPagedAsync(EmpresaId, null, null, null, 1, 20, default))
            .ReturnsAsync((funcionarios, 1));

        var result = await _handler.Handle(new GetFuncionariosQuery(EmpresaId), default);

        result.TotalCount.Should().Be(1);
        result.Items.First().Nome.Should().Be("Maria Santos");
    }
}
