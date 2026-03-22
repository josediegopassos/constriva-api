using ClosedXML.Excel;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Estoque.Queries;

public record ExportRequisicoesQuery(Guid EmpresaId, Guid? ObraId = null, StatusRequisicaoEnum? Status = null)
    : IRequest<byte[]>, ITenantRequest;

public class ExportRequisicoesHandler : IRequestHandler<ExportRequisicoesQuery, byte[]>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public ExportRequisicoesHandler(IEstoqueRepository repo, IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<byte[]> Handle(ExportRequisicoesQuery r, CancellationToken ct)
    {
        var (items, _) = await _repo.GetRequisicoesPagedAsync(r.EmpresaId, r.ObraId, r.Status, 1, int.MaxValue, ct);

        var solicitanteIds = items.Select(i => i.SolicitanteId).Distinct();
        var usuarios = await _usuarioRepo.FindAsync(u => solicitanteIds.Contains(u.Id), ct);
        var nomes = usuarios.ToDictionary(u => u.Id, u => u.Nome);

        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Requisições");

        // Cabeçalho
        var headers = new[]
        {
            "Número", "Status", "Solicitante", "Motivo",
            "Data Requisição", "Data Necessidade", "Data Aprovação",
            "Motivo Rejeição", "Observações"
        };

        for (var i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");
            cell.Style.Font.FontColor = XLColor.White;
        }

        // Dados
        var row = 2;
        foreach (var req in items)
        {
            ws.Cell(row, 1).Value = req.Numero;
            ws.Cell(row, 2).Value = req.Status.ToString();
            ws.Cell(row, 3).Value = nomes.GetValueOrDefault(req.SolicitanteId, "");
            ws.Cell(row, 4).Value = req.Motivo;
            ws.Cell(row, 5).Value = req.DataRequisicao;
            ws.Cell(row, 5).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            if (req.DataNecessidade.HasValue)
            {
                ws.Cell(row, 6).Value = req.DataNecessidade.Value;
                ws.Cell(row, 6).Style.DateFormat.Format = "dd/MM/yyyy";
            }
            if (req.DataAprovacao.HasValue)
            {
                ws.Cell(row, 7).Value = req.DataAprovacao.Value;
                ws.Cell(row, 7).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            }
            ws.Cell(row, 8).Value = req.MotivoRejeicao ?? "";
            ws.Cell(row, 9).Value = req.Observacoes ?? "";
            row++;
        }

        // Formatação
        ws.Columns().AdjustToContents();
        ws.RangeUsed()!.SetAutoFilter();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
