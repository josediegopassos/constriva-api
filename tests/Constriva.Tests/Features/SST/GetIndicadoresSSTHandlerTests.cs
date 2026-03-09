using Constriva.Application.Features.SST;
using Constriva.Application.Features.SST.DTOs;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.SST;

public class GetIndicadoresSSTHandlerTests
{
    private readonly Mock<ISSTRepository> _repo = new();
    private readonly GetIndicadoresSSTHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetIndicadoresSSTHandlerTests()
    {
        _handler = new GetIndicadoresSSTHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveCalcularDiasDesdeUltimoAcidente()
    {
        var dataUltimoAcidente = DateTime.Today.AddDays(-30);
        _repo.Setup(r => r.GetIndicadoresAsync(EmpresaId, null, default))
            .ReturnsAsync(new SSTIndicadoresData(0, 0, 0, 0, 0m, 0m));
        _repo.Setup(r => r.GetAcidentesAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<RegistroAcidente>
            {
                new() { DataHoraAcidente = dataUltimoAcidente, AfastamentoMedico = false }
            });

        var result = await _handler.Handle(new GetIndicadoresSSTQuery(EmpresaId), default);

        result.DiasDesdeUltimoAcidente.Should().Be(30);
    }

    [Fact]
    public async Task Handle_DeveContarAcidentesGraves()
    {
        _repo.Setup(r => r.GetIndicadoresAsync(EmpresaId, null, default))
            .ReturnsAsync(new SSTIndicadoresData(0, 0, 0, 0, 0m, 0m));
        _repo.Setup(r => r.GetAcidentesAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<RegistroAcidente>
            {
                new() { AfastamentoMedico = true, DiasAfastamento = 10, DataHoraAcidente = DateTime.Today },
                new() { AfastamentoMedico = false, DataHoraAcidente = DateTime.Today.AddDays(-5) },
                new() { AfastamentoMedico = true, DiasAfastamento = 5, DataHoraAcidente = DateTime.Today.AddDays(-10) }
            });

        var result = await _handler.Handle(new GetIndicadoresSSTQuery(EmpresaId), default);

        result.TotalAcidentes.Should().Be(3);
        result.AcidentesGraves.Should().Be(2);
        result.TotalDiasAfastamento.Should().Be(15);
    }

    [Fact]
    public async Task Handle_DeveRetornarZeros_QuandoSemAcidentes()
    {
        _repo.Setup(r => r.GetIndicadoresAsync(EmpresaId, null, default))
            .ReturnsAsync(new SSTIndicadoresData(0, 0, 0, 0, 0m, 0m));
        _repo.Setup(r => r.GetAcidentesAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<RegistroAcidente>());

        var result = await _handler.Handle(new GetIndicadoresSSTQuery(EmpresaId), default);

        result.TotalAcidentes.Should().Be(0);
        result.DiasDesdeUltimoAcidente.Should().Be(0);
    }
}
