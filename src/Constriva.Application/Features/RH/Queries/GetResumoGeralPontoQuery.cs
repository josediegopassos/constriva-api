using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetResumoGeralPontoQuery(Guid EmpresaId, DateTime Inicio, DateTime Fim)
    : IRequest<ResumoGeralPontoDto>, ITenantRequest;

public class GetResumoGeralPontoHandler : IRequestHandler<GetResumoGeralPontoQuery, ResumoGeralPontoDto>
{
    private readonly IRHRepository _repo;
    public GetResumoGeralPontoHandler(IRHRepository repo) => _repo = repo;

    public async Task<ResumoGeralPontoDto> Handle(GetResumoGeralPontoQuery r, CancellationToken ct)
    {
        var funcionarios = (await _repo.GetFuncionariosAtivosAsync(r.EmpresaId, ct)).ToList();
        var pontos = (await _repo.GetPontosAsync(r.EmpresaId, null, r.Inicio, r.Fim, ct)).ToList();

        var diasUteis = 0;
        for (var d = r.Inicio.Date; d <= r.Fim.Date; d = d.AddDays(1))
            if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                diasUteis++;

        var pontosPorFunc = pontos.GroupBy(p => p.FuncionarioId).ToDictionary(g => g.Key, g => g.ToList());
        var resumos = new List<ResumoPontoDto>();

        foreach (var func in funcionarios)
        {
            var pts = pontosPorFunc.GetValueOrDefault(func.Id, new());

            var diasPresente = pts
                .Where(p => p.Tipo == TipoRegistroPontoEnum.Entrada)
                .Select(p => p.DataHora.Date).Distinct().Count();

            var faltas = Math.Max(0, diasUteis - diasPresente);
            var horasExtras = pts.Where(p => p.HorasExtras.HasValue).Sum(p => p.HorasExtras!.Value);

            var horasNormais = 0m;
            var porDia = pts.GroupBy(p => p.DataHora.Date);
            foreach (var dia in porDia)
            {
                var entradas = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.Entrada).OrderBy(p => p.DataHora).ToList();
                var saidas = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.Saida).OrderBy(p => p.DataHora).ToList();
                for (var i = 0; i < Math.Min(entradas.Count, saidas.Count); i++)
                    horasNormais += Math.Max(0, (decimal)(saidas[i].DataHora - entradas[i].DataHora).TotalHours);

                var inicioInt = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.InicioIntervalo).OrderBy(p => p.DataHora).ToList();
                var fimInt = dia.Where(p => p.Tipo == TipoRegistroPontoEnum.FimIntervalo).OrderBy(p => p.DataHora).ToList();
                for (var i = 0; i < Math.Min(inicioInt.Count, fimInt.Count); i++)
                    horasNormais -= Math.Max(0, (decimal)(fimInt[i].DataHora - inicioInt[i].DataHora).TotalHours);
            }

            horasNormais = Math.Max(0, Math.Round(horasNormais, 2));

            resumos.Add(new ResumoPontoDto(
                func.Id, func.Nome,
                diasUteis, diasPresente, faltas,
                horasNormais, horasExtras, Math.Round(horasNormais + horasExtras, 2),
                pts.Count,
                pts.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Pendente),
                pts.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Aprovado),
                pts.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Reprovado)));
        }

        return new ResumoGeralPontoDto(
            funcionarios.Count, diasUteis,
            resumos.Sum(r => r.HorasNormais),
            resumos.Sum(r => r.HorasExtras),
            resumos.Sum(r => r.HorasTotais),
            resumos.Sum(r => r.Faltas),
            pontos.Count,
            pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Pendente),
            pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Aprovado),
            pontos.Count(p => p.StatusAprovacao == StatusAprovacaoPontoEnum.Reprovado),
            resumos);
    }
}
