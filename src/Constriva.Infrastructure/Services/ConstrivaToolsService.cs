using System.Text.Json;
using Constriva.Application.Features.Agente.Services;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Constriva.Infrastructure.Services;

public class ConstrivaToolsService : IConstrivaToolsService
{
    private readonly AppDbContext _ctx;
    private readonly IAgenteRepository _repo;
    private readonly IUnitOfWork _uow;

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public ConstrivaToolsService(AppDbContext ctx, IAgenteRepository repo, IUnitOfWork uow)
    {
        _ctx = ctx;
        _repo = repo;
        _uow = uow;
    }

    public async Task<string> ExecuteToolAsync(string toolName, string argsJson, Guid empresaId, Guid usuarioId, CancellationToken ct)
    {
        return toolName switch
        {
            "consultar" => await ConsultarAsync(argsJson, empresaId, ct),
            "salvar" => await SalvarAsync(argsJson, empresaId, ct),
            "gerar_relatorio" => await GerarRelatorioAsync(argsJson, empresaId, ct),
            "disparar_alerta" => await DispararAlertaAsync(argsJson, empresaId, usuarioId, ct),
            _ => throw new InvalidOperationException($"Tool '{toolName}' não reconhecida.")
        };
    }

    public IReadOnlyList<object> GetToolDefinitions()
    {
        return ToolDefinitions.Get();
    }

    private async Task<string> ConsultarAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;

        var modulo = root.GetProperty("modulo").GetString()!;
        var filtros = root.TryGetProperty("filtros", out var f) ? f.GetString() : null;
        var campos = root.TryGetProperty("campos", out var c) ? c.GetString() : null;
        var limite = root.TryGetProperty("limite", out var l) ? l.GetInt32() : 10;

        return modulo switch
        {
            "obras" => await ConsultarObrasAsync(empresaId, filtros, limite, ct),
            "clientes" => await ConsultarClientesAsync(empresaId, filtros, limite, ct),
            "fornecedores" => await ConsultarFornecedoresAsync(empresaId, filtros, limite, ct),
            "estoque" => await ConsultarEstoqueAsync(empresaId, filtros, limite, ct),
            _ => $"Módulo '{modulo}' ainda não implementado para consulta."
        };
    }

    private async Task<string> ConsultarObrasAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Obras
            .Where(o => o.EmpresaId == empresaId && !o.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" => query.Where(o => EF.Functions.ILike(o.Nome, $"%{valor}%")),
                    "status" => Enum.TryParse<StatusObraEnum>(valor, true, out var s)
                        ? query.Where(o => o.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(o => new
            {
                o.Id,
                o.Codigo,
                o.Nome,
                Status = o.Status.ToString(),
                o.DataInicioPrevista,
                o.DataFimPrevista,
                o.DataInicioReal,
                o.PercentualConcluido,
                o.ValorContrato
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarClientesAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Clientes
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" => query.Where(c => EF.Functions.ILike(c.Nome, $"%{valor}%")),
                    "documento" or "cpfcnpj" => query.Where(c => c.Documento == valor),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(c => new
            {
                c.Id,
                c.Codigo,
                c.Nome,
                c.NomeFantasia,
                c.Documento,
                c.Email,
                c.Telefone,
                Status = c.Status.ToString()
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarFornecedoresAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Fornecedores
            .Where(f => f.EmpresaId == empresaId && !f.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" or "razaosocial" => query.Where(f => EF.Functions.ILike(f.RazaoSocial, $"%{valor}%")),
                    "documento" or "cnpj" or "cpfcnpj" => query.Where(f => f.Documento == valor),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(f => new
            {
                f.Id,
                f.Codigo,
                f.RazaoSocial,
                f.NomeFantasia,
                f.Documento,
                f.Email,
                f.Telefone,
                Tipo = f.Tipo.ToString()
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarEstoqueAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Materiais
            .Where(m => m.EmpresaId == empresaId && !m.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" => query.Where(m => EF.Functions.ILike(m.Nome, $"%{valor}%")),
                    "descricao" => query.Where(m => m.Descricao != null && EF.Functions.ILike(m.Descricao, $"%{valor}%")),
                    "codigo" => query.Where(m => m.Codigo == valor),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(m => new
            {
                m.Id,
                m.Codigo,
                m.Nome,
                m.Descricao,
                m.UnidadeMedida,
                m.EstoqueMinimo,
                m.PrecoCustoMedio,
                m.Ativo
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private static Task<string> SalvarAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;

        return Task.FromResult($"Operação de salvamento via agente ainda não implementada para o módulo '{modulo}'.");
    }

    private static Task<string> GerarRelatorioAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;

        return Task.FromResult($"Relatório do módulo '{modulo}' ainda não implementado.");
    }

    private async Task<string> DispararAlertaAsync(string argsJson, Guid empresaId, Guid usuarioId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;

        var modulo = root.GetProperty("modulo").GetString()!;
        var tipo = root.TryGetProperty("tipo", out var t) ? t.GetString() : "Info";
        var mensagem = root.GetProperty("mensagem").GetString()!;
        var destinatarios = root.TryGetProperty("destinatarios", out var d) ? d.GetString() : null;
        var referenciaId = root.TryGetProperty("referencia_id", out var r) && r.TryGetGuid(out var rid) ? rid : (Guid?)null;
        var prazo = root.TryGetProperty("prazo", out var p) && DateTime.TryParse(p.GetString(), out var pdt) ? pdt : (DateTime?)null;

        var tipoEnum = tipo?.ToLowerInvariant() switch
        {
            "aviso" => TipoNotificacaoEnum.Aviso,
            "critico" => TipoNotificacaoEnum.Critico,
            "tarefa" => TipoNotificacaoEnum.Tarefa,
            _ => TipoNotificacaoEnum.Info
        };

        var notificacao = new Notificacao
        {
            EmpresaId = empresaId,
            ModuloOrigem = modulo,
            Tipo = tipoEnum,
            Mensagem = mensagem,
            DestinatariosJson = destinatarios,
            ReferenciaId = referenciaId,
            Prazo = prazo,
            Lida = false
        };

        await _repo.AddNotificacaoAsync(notificacao, ct);
        await _uow.SaveChangesAsync(ct);

        return "Alerta disparado com sucesso.";
    }

    private static List<(string Campo, string Valor)> ParseFiltros(string filtros)
    {
        var resultado = new List<(string, string)>();
        if (string.IsNullOrWhiteSpace(filtros)) return resultado;

        var partes = filtros.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var parte in partes)
        {
            var idx = parte.IndexOf('=');
            if (idx > 0 && idx < parte.Length - 1)
            {
                var campo = parte[..idx].Trim();
                var valor = parte[(idx + 1)..].Trim();
                resultado.Add((campo, valor));
            }
        }

        return resultado;
    }
}

public static class ToolDefinitions
{
    private static readonly IReadOnlyList<object> _definitions = new List<object>
    {
        new
        {
            type = "function",
            function = new
            {
                name = "consultar",
                description = "Consulta registros de um módulo do Constriva",
                parameters = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["modulo"] = new
                        {
                            type = "string",
                            @enum = new[]
                            {
                                "clientes", "compras", "contratos", "cronograma", "empresas",
                                "estoque", "financeiro", "fornecedores", "ged", "obras",
                                "orcamento", "qualidade", "relatorios", "rh", "sst", "usuarios"
                            }
                        },
                        ["filtros"] = new
                        {
                            type = "string",
                            description = "Filtros em formato 'campo=valor'"
                        },
                        ["campos"] = new
                        {
                            type = "string",
                            description = "Campos a retornar, separados por vírgula"
                        },
                        ["limite"] = new
                        {
                            type = "integer",
                            description = "Máximo de registros (padrão 10)"
                        }
                    },
                    required = new[] { "modulo" }
                }
            }
        },
        new
        {
            type = "function",
            function = new
            {
                name = "salvar",
                description = "Cria ou atualiza um registro em um módulo do Constriva",
                parameters = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["modulo"] = new
                        {
                            type = "string",
                            @enum = new[]
                            {
                                "clientes", "compras", "contratos", "cronograma", "empresas",
                                "estoque", "financeiro", "fornecedores", "ged", "obras",
                                "orcamento", "qualidade", "relatorios", "rh", "sst", "usuarios"
                            }
                        },
                        ["dados"] = new
                        {
                            type = "string",
                            description = "Dados do registro em formato JSON"
                        },
                        ["id"] = new
                        {
                            type = "string",
                            description = "ID do registro para atualização (omitir para criação)"
                        }
                    },
                    required = new[] { "modulo", "dados" }
                }
            }
        },
        new
        {
            type = "function",
            function = new
            {
                name = "gerar_relatorio",
                description = "Gera um relatório consolidado de um módulo do Constriva",
                parameters = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["modulo"] = new
                        {
                            type = "string",
                            @enum = new[]
                            {
                                "clientes", "compras", "contratos", "cronograma", "empresas",
                                "estoque", "financeiro", "fornecedores", "ged", "obras",
                                "orcamento", "qualidade", "relatorios", "rh", "sst", "usuarios"
                            }
                        },
                        ["tipo"] = new
                        {
                            type = "string",
                            description = "Tipo de relatório (resumo, detalhado, comparativo)"
                        },
                        ["periodo"] = new
                        {
                            type = "string",
                            description = "Período do relatório (ex: '2026-01 a 2026-03')"
                        }
                    },
                    required = new[] { "modulo" }
                }
            }
        },
        new
        {
            type = "function",
            function = new
            {
                name = "disparar_alerta",
                description = "Dispara um alerta ou notificação para usuários do sistema",
                parameters = new
                {
                    type = "object",
                    properties = new Dictionary<string, object>
                    {
                        ["modulo"] = new
                        {
                            type = "string",
                            description = "Módulo de origem do alerta"
                        },
                        ["tipo"] = new
                        {
                            type = "string",
                            @enum = new[] { "info", "aviso", "critico", "tarefa" },
                            description = "Nível de severidade do alerta"
                        },
                        ["mensagem"] = new
                        {
                            type = "string",
                            description = "Conteúdo do alerta"
                        },
                        ["destinatarios"] = new
                        {
                            type = "string",
                            description = "IDs dos usuários destinatários, separados por vírgula"
                        },
                        ["referencia_id"] = new
                        {
                            type = "string",
                            description = "ID da entidade relacionada ao alerta"
                        },
                        ["prazo"] = new
                        {
                            type = "string",
                            description = "Data limite no formato ISO 8601"
                        }
                    },
                    required = new[] { "modulo", "mensagem" }
                }
            }
        }
    }.AsReadOnly();

    public static IReadOnlyList<object> Get() => _definitions;
}
