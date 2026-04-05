using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Constriva.Messaging.Configuration;
using Constriva.Messaging.Models.Lens;

namespace Constriva.Messaging.Repositories.Lens;

/// <summary>
/// Implementação do repositório de logs de processamento Lens usando MongoDB.
/// </summary>
public class LogProcessamentoLensRepository : ILogProcessamentoLensRepository
{
    private readonly IMongoCollection<LogProcessamentoLens> _colecao;
    private readonly ILogger<LogProcessamentoLensRepository> _logger;

    /// <summary>
    /// Inicializa o repositório e cria os índices necessários.
    /// </summary>
    public LogProcessamentoLensRepository(
        IMongoDatabase database,
        IOptions<MongoDbConfiguration> config,
        ILogger<LogProcessamentoLensRepository> logger)
    {
        _logger = logger;
        var configuracao = config.Value;
        _colecao = database.GetCollection<LogProcessamentoLens>(configuracao.NomeColecaoLogs);

        CriarIndicesAsync(configuracao.TtlDiasRetencao).GetAwaiter().GetResult();
    }

    private async Task CriarIndicesAsync(int ttlDias)
    {
        try
        {
            var indices = new List<CreateIndexModel<LogProcessamentoLens>>
            {
                // Índice TTL em CriadoEm
                new(Builders<LogProcessamentoLens>.IndexKeys.Ascending(x => x.CriadoEm),
                    new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(ttlDias), Name = "idx_ttl_criado_em" }),

                // Índice único em ProcessamentoId
                new(Builders<LogProcessamentoLens>.IndexKeys.Ascending(x => x.ProcessamentoId),
                    new CreateIndexOptions { Unique = true, Name = "idx_processamento_id" }),

                // Índice composto para queries de analytics
                new(Builders<LogProcessamentoLens>.IndexKeys
                    .Ascending(x => x.EmpresaId)
                    .Ascending(x => x.CriadoEm),
                    new CreateIndexOptions { Name = "idx_empresa_criado_em" })
            };

            await _colecao.Indexes.CreateManyAsync(indices);
            _logger.LogInformation("Índices MongoDB criados com sucesso para a coleção de logs Lens.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao criar índices MongoDB (podem já existir).");
        }
    }

    /// <inheritdoc />
    public async Task InserirAsync(LogProcessamentoLens log, CancellationToken ct)
    {
        var filtro = Builders<LogProcessamentoLens>.Filter.Eq(x => x.ProcessamentoId, log.ProcessamentoId);
        var existente = await _colecao.Find(filtro).FirstOrDefaultAsync(ct);

        if (existente is not null)
        {
            log.Id = existente.Id;
            log.AtualizadoEm = DateTime.UtcNow;
            await _colecao.ReplaceOneAsync(filtro, log, cancellationToken: ct);
        }
        else
        {
            await _colecao.InsertOneAsync(log, cancellationToken: ct);
        }

        _logger.LogDebug("Log de processamento {ProcessamentoId} salvo no MongoDB.", log.ProcessamentoId);
    }

    /// <inheritdoc />
    public async Task<LogProcessamentoLens?> ObterPorProcessamentoIdAsync(Guid processamentoId, CancellationToken ct)
    {
        return await _colecao
            .Find(x => x.ProcessamentoId == processamentoId)
            .FirstOrDefaultAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<LogProcessamentoLens>> ListarPorEmpresaAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct)
    {
        return await _colecao
            .Find(x => x.EmpresaId == empresaId && x.CriadoEm >= de && x.CriadoEm <= ate)
            .SortByDescending(x => x.CriadoEm)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<ResumoProcessamentoLens> ObterResumoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct)
    {
        var filtro = Builders<LogProcessamentoLens>.Filter.And(
            Builders<LogProcessamentoLens>.Filter.Eq(x => x.EmpresaId, empresaId),
            Builders<LogProcessamentoLens>.Filter.Gte(x => x.CriadoEm, de),
            Builders<LogProcessamentoLens>.Filter.Lte(x => x.CriadoEm, ate));

        var pipeline = _colecao.Aggregate()
            .Match(filtro)
            .Group(x => 1, g => new
            {
                TotalDocumentos = g.Count(),
                TotalSucesso = g.Sum(x => x.Status == "Concluido" ? 1 : 0),
                TotalErro = g.Sum(x => x.Status == "Erro" ? 1 : 0),
                ConfidenceTotal = g.Sum(x => (double)x.ConfidenceScore),
                TempoTotal = g.Sum(x => x.TempoProcessamentoMs),
                TotalItens = g.Sum(x => x.TotalItens)
            });

        var resultado = await pipeline.FirstOrDefaultAsync(ct);

        if (resultado is null)
            return new ResumoProcessamentoLens();

        return new ResumoProcessamentoLens
        {
            TotalDocumentos = resultado.TotalDocumentos,
            TotalSucesso = resultado.TotalSucesso,
            TotalErro = resultado.TotalErro,
            TaxaSucesso = resultado.TotalDocumentos > 0
                ? (float)resultado.TotalSucesso / resultado.TotalDocumentos * 100
                : 0,
            ConfidenceMedio = resultado.TotalDocumentos > 0
                ? (float)(resultado.ConfidenceTotal / resultado.TotalDocumentos)
                : 0,
            TempoMedioProcessamentoMs = resultado.TotalDocumentos > 0
                ? resultado.TempoTotal / resultado.TotalDocumentos
                : 0,
            TotalItensExtraidos = resultado.TotalItens
        };
    }

    /// <inheritdoc />
    public async Task<List<ProcessamentoPorTipo>> ObterPorTipoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct)
    {
        var filtro = Builders<LogProcessamentoLens>.Filter.And(
            Builders<LogProcessamentoLens>.Filter.Eq(x => x.EmpresaId, empresaId),
            Builders<LogProcessamentoLens>.Filter.Gte(x => x.CriadoEm, de),
            Builders<LogProcessamentoLens>.Filter.Lte(x => x.CriadoEm, ate));

        var pipeline = _colecao.Aggregate()
            .Match(filtro)
            .Group(x => x.TipoDocumento, g => new ProcessamentoPorTipo
            {
                TipoDocumento = g.Key,
                Total = g.Count(),
                Sucesso = g.Sum(x => x.Status == "Concluido" ? 1 : 0),
                Erro = g.Sum(x => x.Status == "Erro" ? 1 : 0),
                ConfidenceMedio = (float)g.Average(x => x.ConfidenceScore)
            });

        return await pipeline.ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<TendenciaConfidence>> ObterTendenciaConfidenceAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct)
    {
        var filtro = Builders<LogProcessamentoLens>.Filter.And(
            Builders<LogProcessamentoLens>.Filter.Eq(x => x.EmpresaId, empresaId),
            Builders<LogProcessamentoLens>.Filter.Gte(x => x.CriadoEm, de),
            Builders<LogProcessamentoLens>.Filter.Lte(x => x.CriadoEm, ate),
            Builders<LogProcessamentoLens>.Filter.Ne(x => x.Status, "Erro"));

        var pipeline = _colecao.Aggregate()
            .Match(filtro)
            .Group(x => x.CriadoEm.Date, g => new TendenciaConfidence
            {
                Data = g.Key,
                ConfidenceMedio = (float)g.Average(x => x.ConfidenceScore),
                TotalDocumentos = g.Count()
            })
            .SortBy(x => x.Data);

        return await pipeline.ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<List<WarningFrequente>> ObterWarningsFrequentesAsync(Guid empresaId, int limite, CancellationToken ct)
    {
        var filtro = Builders<LogProcessamentoLens>.Filter.Eq(x => x.EmpresaId, empresaId);

        var pipeline = _colecao.Aggregate()
            .Match(filtro)
            .Unwind<LogProcessamentoLens, LogWarningUnwind>(x => x.Warnings)
            .Group(x => x.Warnings, g => new WarningFrequente
            {
                Warning = g.Key,
                Frequencia = g.Count()
            })
            .SortByDescending(x => x.Frequencia)
            .Limit(limite);

        return await pipeline.ToListAsync(ct);
    }

    private class LogWarningUnwind
    {
        public string Warnings { get; set; } = string.Empty;
    }
}
