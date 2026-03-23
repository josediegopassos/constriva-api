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

    public AgenteTokenService(
        IAgenteRepository repo,
        IUnitOfWork uow,
        IEmpresaRepository empresaRepo,
        IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _uow = uow;
        _empresaRepo = empresaRepo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task ValidarCotaAsync(Guid empresaId, CancellationToken ct)
    {
        var config = await _repo.GetEmpresaConfigAsync(empresaId, ct)
            ?? throw new KeyNotFoundException($"Configuração do agente não encontrada para a empresa {empresaId}.");

        if (!config.Ativo)
            throw new KeyNotFoundException("O módulo Agente IA não está ativo para esta empresa.");

        var tier = config.Tier;

        // -1 = unlimited
        if (tier.TokensMensais == -1)
            return;

        var agora = DateTime.UtcNow;
        var consumo = await GetOrCreateConsumoMensalAsync(empresaId, agora.Year, agora.Month, tier.TokensMensais, ct);

        var tokensUsados = consumo.TokensUtilizados;

        // Check avulsa quotas
        var cotasAvulsas = await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct);
        var tokensAvulsosDisponiveis = cotasAvulsas.Sum(c => c.TokensRestantes);

        var totalDisponivel = (consumo.TokensLimite - tokensUsados) + tokensAvulsosDisponiveis;

        if (totalDisponivel <= 0)
            throw new CotaExcedidaException("Cota mensal esgotada. Acesse o painel para fazer upgrade do plano.");
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

        // Get active avulsa quotas
        var cotasAvulsas = (await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct)).ToList();

        var tokensRestantes = totalTokens;

        // Consume from avulsa quotas first
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

        // Remainder goes to mensal
        if (tokensRestantes > 0)
        {
            consumo.TokensUtilizados += tokensRestantes;
        }

        consumo.AtualizadoEm = agora;

        // Update/create consumo diario
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

        // Update/create consumo usuario
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

        // Check 80% threshold
        if (tier != null && tier.TokensMensais != -1)
        {
            var totalUsado = consumo.TokensUtilizados + consumo.TokensAvulsosUtilizados;
            var limite80 = (long)(0.8m * consumo.TokensLimite);

            if (totalUsado >= limite80 && !consumo.Alerta80Enviado)
            {
                consumo.Alerta80Enviado = true;
                var notificacao80 = new Notificacao
                {
                    EmpresaId = empresaId,
                    ModuloOrigem = "agente",
                    Tipo = TipoNotificacaoEnum.Aviso,
                    Mensagem = $"Sua cota de tokens do Agente IA atingiu 80%. Utilizados: {totalUsado:N0} de {consumo.TokensLimite:N0}.",
                    Lida = false
                };
                await _repo.AddNotificacaoAsync(notificacao80, ct);
            }

            // Check 100% - no avulsa left
            var cotasAvulsasRestantes = cotasAvulsas.Sum(c => c.TokensRestantes);
            if (consumo.TokensUtilizados >= consumo.TokensLimite && cotasAvulsasRestantes <= 0)
            {
                var notificacao100 = new Notificacao
                {
                    EmpresaId = empresaId,
                    ModuloOrigem = "agente",
                    Tipo = TipoNotificacaoEnum.Critico,
                    Mensagem = "Sua cota mensal de tokens do Agente IA foi totalmente consumida. O agente ficará indisponível até o próximo mês ou até a aquisição de cotas avulsas.",
                    Lida = false
                };
                await _repo.AddNotificacaoAsync(notificacao100, ct);
            }
        }

        await _uow.SaveChangesAsync(ct);
    }

    public async Task<DashboardConsumoDto> ObterDashboardConsumoAsync(Guid empresaId, CancellationToken ct)
    {
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

        var consumoResumo = new ConsumoResumoDto(
            totalUsado,
            consumo.TokensLimite,
            percentual,
            tokensRestantes,
            consumo.Alerta80Enviado);

        var tierDto = new TierDto(tier.Id, tier.Nome, tier.TokensMensais, tier.Descricao);

        // Cotas avulsas disponíveis
        var cotasAvulsas = await _repo.GetCotasAvulsasAtivasAsync(empresaId, ct);
        var cotaAvulsaDisponivel = cotasAvulsas.Sum(c => c.TokensRestantes);

        // Histórico diário últimos 30 dias
        var historicoDiario = (await _repo.GetConsumoDiarioUltimos30DiasAsync(empresaId, ct))
            .Select(d => new ConsumoDiarioDto(d.Data, d.TokensInput + d.TokensOutput, d.TotalRequisicoes));

        // Top 5 usuários
        var topUsuarios = (await _repo.GetTopUsuariosAsync(empresaId, agora.Year, agora.Month, 5, ct)).ToList();
        var topUsuarioDtos = new List<ConsumoUsuarioDto>();

        foreach (var u in topUsuarios)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(u.UsuarioId, ct);
            var nome = usuario?.Nome ?? "Usuário desconhecido";
            topUsuarioDtos.Add(new ConsumoUsuarioDto(u.UsuarioId, nome, u.TokensUtilizados, u.TotalRequisicoes));
        }

        // Dias restantes no mês
        var ultimoDiaMes = new DateTime(agora.Year, agora.Month, DateTime.DaysInMonth(agora.Year, agora.Month));
        var diasRestantes = (ultimoDiaMes - agora.Date).Days;

        return new DashboardConsumoDto(
            consumoResumo,
            tierDto,
            cotaAvulsaDisponivel,
            historicoDiario,
            topUsuarioDtos,
            diasRestantes);
    }

    public async Task<IEnumerable<AdminRelatorioItemDto>> ObterRelatorioMensalAsync(int ano, int mes, CancellationToken ct)
    {
        var consumos = (await _repo.GetRelatorioMensalAsync(ano, mes, ct)).ToList();
        var configs = (await _repo.GetTodasEmpresasAtivasAsync(ct)).ToList();

        var resultado = new List<AdminRelatorioItemDto>();

        foreach (var consumo in consumos)
        {
            var config = configs.FirstOrDefault(c => c.EmpresaId == consumo.EmpresaId);
            var tierNome = config?.Tier?.Nome ?? "Desconhecido";

            var empresa = await _empresaRepo.GetByIdAsync(consumo.EmpresaId, ct);
            var empresaNome = empresa?.NomeFantasia ?? empresa?.RazaoSocial ?? "Empresa desconhecida";

            var totalUsado = consumo.TokensUtilizados + consumo.TokensAvulsosUtilizados;
            var percentualUso = consumo.TokensLimite > 0
                ? Math.Round((decimal)totalUsado / consumo.TokensLimite * 100, 2)
                : 0m;

            // Estimativa de custo: GPT-4o-mini pricing ~$0.15/1M input, $0.60/1M output
            // Simplificação: usar média de $0.30/1M tokens
            var custoEstimadoUsd = Math.Round(totalUsado * 0.30m / 1_000_000m, 4);

            // Get total requisicoes from diarios (sum for the month)
            var totalRequisicoes = 0;
            var primeiroDia = new DateTime(ano, mes, 1);
            var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
            for (var d = primeiroDia; d <= ultimoDia; d = d.AddDays(1))
            {
                var diario = await _repo.GetConsumoDiarioAsync(consumo.EmpresaId, d, ct);
                if (diario != null)
                    totalRequisicoes += diario.TotalRequisicoes;
            }

            resultado.Add(new AdminRelatorioItemDto(
                consumo.EmpresaId,
                empresaNome,
                tierNome,
                consumo.TokensLimite,
                totalUsado,
                percentualUso,
                consumo.TokensAvulsosUtilizados,
                totalRequisicoes,
                custoEstimadoUsd));
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
