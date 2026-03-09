using Constriva.Application.Features.Empresas;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Empresas;

public class GetEmpresasHandlerTests
{
    private readonly Mock<IEmpresaRepository> _repo = new();
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly GetEmpresasHandler _handler;

    public GetEmpresasHandlerTests()
    {
        _handler = new GetEmpresasHandler(_repo.Object, _usuarioRepo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarEmpresasPaginadas()
    {
        var empresas = new List<Empresa>
        {
            new() { RazaoSocial = "Empresa A", NomeFantasia = "A",
                Cnpj = "11111111000111", Status = StatusEmpresaEnum.Ativa, Plano = PlanoEmpresaEnum.Profissional }
        };
        _repo.Setup(r => r.Query()).Returns(empresas.AsQueryable());

        var result = await _handler.Handle(new GetEmpresasQuery(), default);

        result.TotalCount.Should().Be(1);
        result.Items.First().RazaoSocial.Should().Be("Empresa A");
    }
}
