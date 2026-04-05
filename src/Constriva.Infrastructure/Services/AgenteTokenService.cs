using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Exceptions;
using Constriva.Application.Features.Agente.Services;
using Constriva.Domain.Entities.Agente;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Services;

public class AgenteTokenService : IAgenteTokenService
{
    private readonly IAgenteRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ICacheService _cache;

    public AgenteTokenService(
        IAgenteRepository repo,
        IUnitOfWork uow,
        IEmpresaRepository empresaRepo,
        IUsuarioRepository usuarioRepo,
        ICacheService cache)
    {
        _repo = repo;
        _uow = uow;
        _empresaRepo = empresaRepo;
        _usuarioRepo = usuarioRepo;
        _cache = cache;
    }

    public async Task ValidarCotaAsync(Guid empresaId, CancellationToken ct)
    {
        // Cache de validação de cota por 30 segundos para evitar 3 queries/request
        var cacheKey = $"agent:quota:{empresaId}";
        var cached = await _cache.GetAsync<string>(cacheKey, ct);
        if (cached == "ok") return;
        if (cached == "exceeded")
            throw new CotaExcedidaException("Cota mensal esgotada. Acesse o painel para fazer upgrade do plano.");

        var config = await _repo.GetEmpresaConfigAsync(empresaId, ct)
            ?? throw new KeyNotFoundException($"Configuração do agente não encontrada para a empresa {empresaId}.");

        if (!config.Ativo)
            throw new KeyNotFoundException("O módulo Agente IA não está ativo para esta empresa.");

        var tier = config.Tier;

        if (tier.TokensMensais == -1)
        {
            await _cache.SetAsync(cacheKey, "ok", TimeSpan.FromSeconds(60), ct);
            return;
        }

        var agora = DateTime.UtcNow;
        var consumo = await GetOrCreateConsumoMensalAsync(empresaId, agora.Year, agora.Month, tier.TokensMensais, ct);

        var tokensUsados = consumo.TokensUtilizados;
        var cotasAvulsas = await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct);
        var tokensAvulsosDisponiveis = cotasAvulsas.Sum(c => c.TokensRestantes);

        var totalDisponivel = (consumo.TokensLimite - tokensUsados) + tokensAvulsosDisponiveis;

        if (totalDisponivel <= 0)
        {
            await _cache.SetAsync(cacheKey, "exceeded", TimeSpan.FromSeconds(30), ct);
            throw new CotaExcedidaException("Cota mensal esgotada. Acesse o painel para fazer upgrade do plano.");
        }

        await _cache.SetAsync(cacheKey, "ok", TimeSpan.FromSeconds(30), ct);
    }

    public async Task RegistrarConsumoAsync(Guid empresaId, Guid usuarioId, int tokensInput, int tokensOutput, CancellationToken ct)
    {
        var totalTokens = (long)tokensInput + tokensOutput;

        var config = await _repo.GetEmpresaConfigAsync(empresaId, ct);
        var tier = config?.Tier;

        var agora = DateTime.UtcNow;
        var consumo = await GetOrCreateConsumoMensalAsync(
            empresaId, agora.Year, agora.Month,
            tier?.TokensMensais ?? 0, ct);

        var cotasAvulsas = (await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct)).ToList();
        var tokensRestantes = totalTokens;

        // Consumir cotas avulsas primeiro
        foreach (var cota in cotasAvulsas)
        {
            if (tokensRestantes <= 0) break;
            var disponivelNaCota = cota.TokensRestantes;
            if (disponivelNaCota <= 0) continue;
            var consumirDaCota = Math.Min(tokensRestantes, disponivelNaCota);
            cota.TokensUtilizados += consumirDaCota;
            consumo.TokensAvulsosUtilizados += consumirDaCota;
            tokensRestantes -= consumirDaCota;
        }

        if (tokensRestantes > 0)
            consumo.TokensUtilizados += tokensRestantes;

        consumo.AtualizadoEm = agora;

        // Consumo diário
        var hoje = agora.Date;
        var diario = await _repo.GetConsumoDiarioAsync(empresaId, hoje, ct);
        if (diario == null)
        {
            diario = new AgenteConsumoDiario
            {
                EmpresaId = empresaId,
                Data = hoje,
                TokensInput = tokensInput,
                TokensOutput = tokensOutput,
                TotalRequisicoes = 1
            };
            await _repo.AddConsumoDiarioAsync(diario, ct);
        }
        else
        {
            diario.TokensInput += tokensInput;
            diario.TokensOutput += tokensOutput;
            diario.TotalRequisicoes++;
        }

        // Consumo por usuário
        var consumoUsuario = await _repo.GetConsumoUsuarioAsync(empresaId, usuarioId, agora.Year, agora.Month, ct);
        if (consumoUsuario == null)
        {
            consumoUsuario = new AgenteConsumoUsuario
            {
                EmpresaId = empresaId,
                UsuarioId = usuarioId,
                Ano = agora.Year,
                Mes = agora.Month,
                TokensUtilizados = totalTokens,
                TotalRequisicoes = 1
            };
            await _repo.AddConsumoUsuarioAsync(consumoUsuario, ct);
        }
        else
        {
            consumoUsuario.TokensUtilizados += totalTokens;
            consumoUsuario.TotalRequisicoes++;
        }

        // Alertas de 80% e 100%
        if (tier != null && tier.TokensMensais != -1)
        {
            var totalUsado = consumo.TokensUtilizados + consumo.TokensAvulsosUtilizados;
            var limite80 = (long)(0.8m * consumo.TokensLimite);

            if (totalUsado >= limite80 && !consumo.Alerta80Enviado)
            {
                consumo.Alerta80Enviado = true;
                await _repo.AddNotificacaoAsync(new Notificacao
                {
                    EmpresaId = empresaId,
                    ModuloOrigem = "agente",
                    Tipo = TipoNotificacaoEnum.Aviso,
                    Mensagem = $"Sua cota de tokens do Agente IA atingiu 80%. Utilizados: {totalUsado:N0} de {consumo.TokensLimite:N0}.",
                    Lida = false
                }, ct);
            }

            var cotasAvulsasRestantes = cotasAvulsas.Sum(c => c.TokensRestantes);
            if (consumo.TokensUtilizados >= consumo.TokensLimite && cotasAvulsasRestantes <= 0)
            {
                await _repo.AddNotificacaoAsync(new Notificacao
                {
                    EmpresaId = empresaId,
                    ModuloOrigem = "agente",
                    Tipo = TipoNotificacaoEnum.Critico,
                    Mensagem = "Sua cota mensal de tokens do Agente IA foi totalmente consumida.",
                    Lida = false
                }, ct);
            }
        }

        await _uow.SaveChangesAsync(ct);

        // Invalidar cache de quota após registrar consumo
        await _cache.RemoveAsync($"agent:quota:{empresaId}", ct);
        await _cache.RemoveAsync($"agent:dashboard:{empresaId}", ct);
    }

    public async Task<DashboardConsumoDto> ObterDashboardConsumoAsync(Guid empresaId, CancellationToken ct)
    {
        // Cache do dashboard por 30 segundos
        var cacheKey = $"agent:dashboard:{empresaId}";
        var cached = await _cache.GetAsync<DashboardConsumoDto>(cacheKey, ct);
        if (cached is not null) return cached;

        var config = await _repo.GetEmpresaConfigAsync(empresaId, ct)
            ?? throw new KeyNotFoundException($"Configuração do agente não encontrada para a empresa {empresaId}.");

        var tier = config.Tier;
        var agora = DateTime.UtcNow;

        var consumo = await GetOrCreateConsumoMensalAsync(empresaId, agora.Year, agora.Month, tier.TokensMensais, ct);
        await _uow.SaveChangesAsync(ct);

        var totalUsado = consumo.TokensUtilizados + consumo.TokensAvulsosUtilizados;
        var percentual = consumo.TokensLimite > 0
            ? Math.Round((decimal)totalUsado / consumo.TokensLimite * 100, 2)
            : (tier.TokensMensais == -1 ? 0m : 100m);
        var tokensRestantes = tier.TokensMensais == -1
            ? -1
            : Math.Max(0, consumo.TokensLimite - consumo.TokensUtilizados);

        var consumoResumo = new ConsumoResumoDto(totalUsado, consumo.TokensLimite, percentual, tokensRestantes, consumo.Alerta80Enviado);
        var tierDto = new TierDto(tier.Id, tier.Nome, tier.TokensMensais, tier.Descricao);

        var cotasAvulsas = await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct);
        var cotaAvulsaDisponivel = cotasAvulsas.Sum(c => c.TokensRestantes);

        var historicoDiario = (await _repo.GetConsumoDiarioUltimos30DiasAsync(empresaId, ct))
            .Select(d => new ConsumoDiarioDto(d.Data, d.TokensInput + d.TokensOutput, d.TotalRequisicoes));

        var topUsuarios = (await _repo.GetTopUsuariosAsync(empresaId, agora.Year, agora.Month, 5, ct)).ToList();
        var topUsuarioDtos = new List<ConsumoUsuarioDto>();
        foreach (var u in topUsuarios)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(u.UsuarioId, ct);
            topUsuarioDtos.Add(new ConsumoUsuarioDto(u.UsuarioId, usuario?.Nome ?? "Desconhecido", u.TokensUtilizados, u.TotalRequisicoes));
        }

        var ultimoDiaMes = new DateTime(agora.Year, agora.Month, DateTime.DaysInMonth(agora.Year, agora.Month));
        var diasRestantes = (ultimoDiaMes - agora.Date).Days;

        var dashboard = new DashboardConsumoDto(consumoResumo, tierDto, cotaAvulsaDisponivel, historicoDiario, topUsuarioDtos, diasRestantes);

        // Cachear resultado
        await _cache.SetAsync(cacheKey, dashboard, TimeSpan.FromSeconds(30), ct);

        return dashboard;
    }

    public async Task<IEnumerable<AdminRelatorioItemDto>> ObterRelatorioMensalAsync(int ano, int mes, CancellationToken ct)
    {
        var consumos = (await _repo.GetRelatorioMensalAsync(ano, mes, ct)).ToList();
        var configs = (await _repo.GetTodasEmpresasAtivasAsync(ct)).ToList();

        // Buscar todos os diários do mês de uma vez (evita N+1 queries)
        var primeiroDia = new DateTime(ano, mes, 1);
        var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);

        var resultado = new List<AdminRelatorioItemDto>();

        foreach (var consumo in consumos)
        {
            var config = configs.FirstOrDefault(c => c.EmpresaId == consumo.EmpresaId);
            var tierNome = config?.Tier?.Nome ?? "Desconhecido";

            var empresa = await _empresaRepo.GetByIdAsync(consumo.EmpresaId, ct);
            var empresaNome = empresa?.NomeFantasia ?? empresa?.RazaoSocial ?? "Empresa desconhecida";

            var totalUsado = consumo.TokensUtilizados + consumo.TokensAvulsosUtilizados;
            var percentualUso = consumo.TokensLimite > 0
                ? Math.Round((decimal)totalUsado / consumo.TokensLimite * 100, 2) : 0m;

            // Custo estimado: GPT-4o-mini ~$0.15/1M input, $0.60/1M output → média $0.30/1M
            var custoEstimadoUsd = Math.Round(totalUsado * 0.30m / 1_000_000m, 4);

            // Buscar total de requisições do consumo diário acumulado
            var diarios = await _repo.GetConsumoDiarioPorMesAsync(consumo.EmpresaId, ano, mes, ct);
            var totalRequisicoes = diarios.Sum(d => d.TotalRequisicoes);

            resultado.Add(new AdminRelatorioItemDto(
                consumo.EmpresaId, empresaNome, tierNome,
                consumo.TokensLimite, totalUsado, percentualUso,
                consumo.TokensAvulsosUtilizados, totalRequisicoes, custoEstimadoUsd));
        }

        return resultado;
    }

    public async Task AdicionarCotaAvulsaAsync(Guid empresaId, long tokens, string motivo, DateTime? expiracao, Guid adminUserId, CancellationToken ct)
    {
        var cota = new AgenteCotaAvulsa
        {
            EmpresaId = empresaId,
            TokensConcedidos = tokens,
            TokensUtilizados = 0,
            Motivo = motivo,
            ConcedidoEm = DateTime.UtcNow,
            ExpiraEm = expiracao,
            ConcedidoPorUsuarioId = adminUserId
        };

        await _repo.AddCotaAvulsaAsync(cota, ct);
        await _uow.SaveChangesAsync(ct);

        // Invalidar cache de quota
        await _cache.RemoveAsync($"agent:quota:{empresaId}", ct);
    }

    private async Task<AgenteConsumoMensal> GetOrCreateConsumoMensalAsync(
        Guid empresaId, int ano, int mes, long tokensLimite, CancellationToken ct)
    {
        var consumo = await _repo.GetConsumoMensalAsync(empresaId, ano, mes, ct);
        if (consumo == null)
        {
            consumo = new AgenteConsumoMensal
            {
                EmpresaId = empresaId,
                Ano = ano,
                Mes = mes,
                TokensUtilizados = 0,
                TokensLimite = tokensLimite,
                TokensAvulsosUtilizados = 0,
                Alerta80Enviado = false,
                AtualizadoEm = DateTime.UtcNow
            };
            await _repo.AddConsumoMensalAsync(consumo, ct);
        }

        return consumo;
    }
}
