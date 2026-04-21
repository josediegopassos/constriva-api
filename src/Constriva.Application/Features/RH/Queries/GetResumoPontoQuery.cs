using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetResumoPontoQuery(Guid EmpresaId, Guid FuncionarioId, DateTime Inicio, DateTime Fim)
    : IRequest<ResumoPontoDto>, ITenantRequest;

public class GetResumoPontoHandler : IRequestHandler<GetResumoPontoQuery, ResumoPontoDto>
{
    private readonly IRHRepository _repo;
    public GetResumoPontoHandler(IRHRepository repo) => _repo = repo;

    public async Task<ResumoPontoDto> Handle(GetResumoPontoQuery r, CancellationToken ct)
    {
        var funcionario = await _repo.GetFuncionarioByIdAsync(r.FuncionarioId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Funcionário não encontrado.");

        var pontos = (await _repo.GetPontosAsync(r.EmpresaId, r.FuncionarioId, r.Inicio, r.Fim, ct)).ToList();

        // Dias úteis (seg-sex) no período
        var diasUteis = 0;
        for (var d = r.Inicio.Date; d <= r.Fim.Date; d = d.AddDays(1))
            if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                diasUteis++;

        // Dias com pelo menos uma entrada registrada
        var diasPresente = pontos
            .Where(p => p.Tipo == TipoRegistroPontoEnum.Entrada)
            .Select(p => p.DataHora.Date)
            .Distinct()
            .Count();

        var faltas = Math.Max(0, diasUteis - diasPresente);

        // Calcular horas por pares Entrada/Saída no mesmo dia
        var horasNormais = 0m;
        var horasExtras = pontos.Where(p => p.HorasExtras.HasValue).Sum(p => p.HorasExtras!.Value);

        var porDia = pontos.GroupBy(p => p.DataHora.Date);
        foreach (var dia in porDia)
        {
            var entradas = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.Entrada)
                .OrderBy(p => p.DataHora).ToList();
            var saidas = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.Saida)
                .OrderBy(p => p.DataHora).ToList();

            var pares = Math.Min(entradas.Count, saidas.Count);
            for (var i = 0; i < pares; i++)
            {
                var diff = (decimal)(saidas[i].DataHora - entradas[i].DataHora).TotalHours;
                horasNormais += Math.Max(0, Math.Round(diff, 2));
            }
        }

        // Descontar intervalos
        foreach (var dia in porDia)
        {
            var inicioInt = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.InicioIntervalo)
                .OrderBy(p => p.DataHora).ToList();
            var fimInt = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.FimIntervalo)
                .OrderBy(p => p.DataHora).ToList();

            var pares = Math.Min(inicioInt.Count, fimInt.Count);
            for (var i = 0; i < pares; i++)
            {
                var diff = (decimal)(fimInt[i].DataHora - inicioInt[i].DataHora).TotalHours;
                horasNormais -= Math.Max(0, Math.Round(diff, 2));
            }
        }

        horasNormais = Math.Max(0, Math.Round(horasNormais, 2));
        var horasTotais = Math.Round(horasNormais + horasExtras, 2);

        var totalRegistros = pontos.Count;
        var pendentes = pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Pendente);
        var aprovados = pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Aprovado);
        var reprovados = pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Reprovado);

        return new ResumoPontoDto(
            r.FuncionarioId, funcionario.Nome,
            diasUteis, diasPresente, faltas,
            horasNormais, horasExtras, horasTotais,
            totalRegistros, pendentes, aprovados, reprovados);
    }
}
