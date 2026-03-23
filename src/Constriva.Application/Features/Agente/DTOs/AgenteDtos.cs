namespace Constriva.Application.Features.Agente.DTOs;

public record ChatRequestDto(string Mensagem, Guid? SessaoId = null);

public record ChatResponseDto(
    Guid SessaoId, string Resposta, int TokensConsumidos,
    long TokensRestantes, decimal PercentualUsado);

public record ConsumoResumoDto(
    long TokensUtilizados, long TokensLimite, decimal PercentualUso,
    long TokensRestantes, bool Alerta80Enviado);

public record DashboardConsumoDto(
    ConsumoResumoDto ConsumoMesAtual,
    TierDto TierContratado,
    long CotaAvulsaDisponivel,
    IEnumerable<ConsumoDiarioDto> HistoricoDiario,
    IEnumerable<ConsumoUsuarioDto> TopUsuarios,
    int DiasRestantesNoMes);

public record ConsumoDiarioDto(DateTime Data, long TotalTokens, int TotalRequisicoes);

public record ConsumoUsuarioDto(Guid UsuarioId, string NomeUsuario, long TokensUtilizados, int TotalRequisicoes);

public record SessaoResumoDto(Guid Id, DateTime AtualizadaEm, bool Ativa, int TotalMensagens);

public record SessaoDetalheDto(Guid Id, DateTime AtualizadaEm, IEnumerable<MensagemDto> Mensagens);

public record MensagemDto(string Role, string Conteudo, int TokensInput, int TokensOutput, DateTime CreatedAt);

public record NotificacaoDto(Guid Id, string ModuloOrigem, string Tipo, string Mensagem, bool Lida, DateTime? Prazo, DateTime CreatedAt);

public record TierDto(Guid Id, string Nome, long TokensMensais, string? Descricao);

public record AdminRelatorioItemDto(
    Guid EmpresaId, string EmpresaNome, string TierNome,
    long TokensLimite, long TokensUtilizados, decimal PercentualUso,
    long TokensAvulsosUtilizados, int TotalRequisicoes, decimal CustoEstimadoUsd);

public record AdminEmpresaAgenteDto(
    Guid EmpresaId, string EmpresaNome, string TierNome,
    long TokensUtilizados, long TokensLimite, bool Ativo);

public record CriarCotaAvulsaDto(Guid EmpresaId, long Tokens, string Motivo, DateTime? Expiracao = null);

public record AlterarTierDto(Guid TierId);
