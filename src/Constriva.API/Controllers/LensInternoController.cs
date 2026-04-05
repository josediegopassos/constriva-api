using Constriva.API.Hubs;
using Constriva.Domain.Enums;
using Constriva.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Constriva.API.Controllers;

[ApiController]
[Route("api/interno/lens")]
public sealed class LensInternoController : ControllerBase
{
    private readonly ILensNotificationService _notificacao;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LensInternoController> _logger;
    private readonly AppDbContext _db;

    public LensInternoController(
        ILensNotificationService notificacao,
        IConfiguration configuration,
        ILogger<LensInternoController> logger,
        AppDbContext db)
    {
        _notificacao = notificacao;
        _configuration = configuration;
        _logger = logger;
        _db = db;
    }

    [HttpPost("notificar/{evento}")]
    public async Task<IActionResult> Notificar(string evento, [FromBody] InternalNotificationDto dto, CancellationToken ct)
    {
        if (!ValidarChaveInterna())
        {
            _logger.LogWarning("Tentativa de acesso nao autorizado ao endpoint interno de notificacao.");
            return Unauthorized();
        }

        _logger.LogInformation("Notificacao interna recebida: {Evento} para usuario {UsuarioId}.", evento, dto.UsuarioId);

        try
        {
            // Delegate to specific notification methods based on event name
            // The Messaging service sends pre-formatted data
            await _notificacao.NotificarItemAtualizado(
                dto.ProcessamentoId ?? Guid.Empty,
                Guid.Empty,
                evento,
                dto.UsuarioId,
                "Sistema",
                dto.ObraId,
                dto.EmpresaId);

            return Ok(new { sucesso = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar notificacao interna '{Evento}'.", evento);
            return StatusCode(500, new { sucesso = false, erro = "Erro interno ao processar notificacao." });
        }
    }
    [HttpGet("processamentos/{id:guid}")]
    public async Task<IActionResult> GetProcessamento(Guid id, CancellationToken ct)
    {
        if (!ValidarChaveInterna())
            return Unauthorized();

        var doc = await _db.DocumentosLens
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct);

        if (doc is null)
            return NotFound(new { sucesso = false, erros = new[] { "Processamento não encontrado." } });

        return Ok(new
        {
            sucesso = true,
            dados = new
            {
                doc.Id,
                doc.EmpresaId,
                doc.ObraId,
                doc.UsuarioId,
                tipoDocumento = doc.TipoDocumento.ToString(),
                doc.NomeArquivo,
                doc.CaminhoArquivo,
                doc.ExtensaoArquivo,
                doc.TamanhoBytes,
                status = doc.Status.ToString()
            }
        });
    }

    private bool ValidarChaveInterna()
    {
        var chaveEsperada = _configuration["ConstrivaApi:ChaveInterna"] ?? "";
        var chaveRecebida = Request.Headers["X-Constriva-Internal-Key"].FirstOrDefault();
        return !string.IsNullOrEmpty(chaveEsperada) && chaveRecebida == chaveEsperada;
    }
}

public class InternalNotificationDto
{
    public string Evento { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public Guid? ObraId { get; set; }
    public Guid EmpresaId { get; set; }
    public Guid? ProcessamentoId { get; set; }
    public object? Dados { get; set; }
}
