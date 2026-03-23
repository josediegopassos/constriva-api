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
            "buscar" => await BuscarAsync(argsJson, empresaId, ct),
            "salvar" => await SalvarAsync(argsJson, empresaId, ct),
            "excluir" => await ExcluirAsync(argsJson, empresaId, ct),
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
            "contratos" => await ConsultarContratosAsync(empresaId, filtros, limite, ct),
            "compras" => await ConsultarComprasAsync(empresaId, filtros, limite, ct),
            "financeiro" => await ConsultarFinanceiroAsync(empresaId, filtros, limite, ct),
            "rh" => await ConsultarRHAsync(empresaId, filtros, limite, ct),
            "cronograma" => await ConsultarCronogramaAsync(empresaId, filtros, limite, ct),
            "orcamento" => await ConsultarOrcamentoAsync(empresaId, filtros, limite, ct),
            "qualidade" => await ConsultarQualidadeAsync(empresaId, filtros, limite, ct),
            "sst" => await ConsultarSSTAsync(empresaId, filtros, limite, ct),
            "ged" => await ConsultarGEDAsync(empresaId, filtros, limite, ct),
            "usuarios" => await ConsultarUsuariosAsync(empresaId, filtros, limite, ct),
            "empresas" => await ConsultarEmpresasAsync(empresaId, filtros, limite, ct),
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

    // ── Buscar por ID ────────────────────────────────────────────────────
    private async Task<string> BuscarAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;
        var idStr = root.GetProperty("id").GetString()!;
        if (!Guid.TryParse(idStr, out var id))
            return $"ID inválido: '{idStr}'.";

        object? resultado = modulo switch
        {
            "obras" => await _ctx.Obras.Include(o => o.Endereco).AsNoTracking()
                .Where(o => o.Id == id && o.EmpresaId == empresaId && !o.IsDeleted)
                .Select(o => new { o.Id, o.Codigo, o.Nome, o.Descricao, Tipo = o.Tipo.ToString(), Status = o.Status.ToString(), o.ValorContrato, o.ValorOrcado, o.DataInicioPrevista, o.DataFimPrevista, o.DataInicioReal, o.DataFimReal, o.PercentualConcluido, Logradouro = o.Endereco != null ? o.Endereco.Logradouro : null, Cidade = o.Endereco != null ? o.Endereco.Cidade : null, Estado = o.Endereco != null ? o.Endereco.Estado : null, o.Observacoes, o.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "clientes" => await _ctx.Clientes.Include(c => c.Endereco).AsNoTracking()
                .Where(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted)
                .Select(c => new { c.Id, c.Codigo, c.Nome, c.NomeFantasia, c.Documento, c.Email, c.Telefone, c.Celular, Status = c.Status.ToString(), Logradouro = c.Endereco != null ? c.Endereco.Logradouro : null, Cidade = c.Endereco != null ? c.Endereco.Cidade : null, Estado = c.Endereco != null ? c.Endereco.Estado : null, c.Observacoes, c.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "fornecedores" => await _ctx.Fornecedores.Include(f => f.Endereco).AsNoTracking()
                .Where(f => f.Id == id && f.EmpresaId == empresaId && !f.IsDeleted)
                .Select(f => new { f.Id, f.Codigo, f.RazaoSocial, f.NomeFantasia, f.Documento, f.Email, f.Telefone, Tipo = f.Tipo.ToString(), f.Ativo, f.Homologado, f.Classificacao, Logradouro = f.Endereco != null ? f.Endereco.Logradouro : null, Cidade = f.Endereco != null ? f.Endereco.Cidade : null, Estado = f.Endereco != null ? f.Endereco.Estado : null, f.Observacoes, f.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "contratos" => await _ctx.Contratos.Include(c => c.Fornecedor).AsNoTracking()
                .Where(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted)
                .Select(c => new { c.Id, c.Numero, c.Objeto, c.Descricao, Tipo = c.Tipo.ToString(), Status = c.Status.ToString(), c.FornecedorId, Fornecedor = c.Fornecedor.RazaoSocial, c.ValorGlobal, c.ValorAditivos, c.ValorMedidoAcumulado, c.ValorPagoAcumulado, c.PercentualRetencao, c.DataAssinatura, c.DataVigenciaInicio, c.DataVigenciaFim, c.DataEncerramento, c.CondicoesPagamento, c.Observacoes, c.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "compras" => await _ctx.PedidosCompra.Include(p => p.Fornecedor).AsNoTracking()
                .Where(p => p.Id == id && p.EmpresaId == empresaId && !p.IsDeleted)
                .Select(p => new { p.Id, p.Numero, Status = p.Status.ToString(), p.FornecedorId, Fornecedor = p.Fornecedor.RazaoSocial, p.ValorTotal, p.ValorFrete, p.ValorDesconto, p.DataPedido, p.DataEntregaPrevista, p.DataEntregaReal, p.CondicoesPagamento, p.Observacoes, p.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "estoque" => await _ctx.Materiais.AsNoTracking()
                .Where(m => m.Id == id && m.EmpresaId == empresaId && !m.IsDeleted)
                .Select(m => new { m.Id, m.Codigo, m.Nome, m.UnidadeMedida, Tipo = m.Tipo.ToString(), m.Marca, m.Fabricante, m.EstoqueMinimo, m.EstoqueMaximo, m.PrecoCustoMedio, m.PrecoUltimaCompra, m.Ativo, m.ControlaLote, m.ControlaValidade, m.Observacoes, m.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "financeiro" => await _ctx.LancamentosFinanceiros.AsNoTracking()
                .Where(l => l.Id == id && l.EmpresaId == empresaId && !l.IsDeleted)
                .Select(l => new { l.Id, Tipo = l.Tipo.ToString(), Status = l.Status.ToString(), l.Descricao, l.Valor, l.DataVencimento, l.DataPagamento, l.Observacoes, l.CreatedAt })
                .FirstOrDefaultAsync(ct),
            "rh" => await _ctx.Funcionarios.AsNoTracking()
                .Where(f => f.Id == id && f.EmpresaId == empresaId && !f.IsDeleted)
                .Select(f => new { f.Id, f.Matricula, f.Nome, f.Cpf, f.Email, f.Telefone, Status = f.Status.ToString(), f.SalarioBase, f.DataAdmissao, f.DataDemissao, f.CreatedAt })
                .FirstOrDefaultAsync(ct),
            _ => (object?)$"Busca por ID não implementada para o módulo '{modulo}'."
        };

        if (resultado == null) return $"Registro {id} não encontrado no módulo '{modulo}'.";
        if (resultado is string s) return s;
        return JsonSerializer.Serialize(resultado, _jsonOpts);
    }

    // ── Excluir (soft delete) ────────────────────────────────────────────
    private async Task<string> ExcluirAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;
        var idStr = root.GetProperty("id").GetString()!;
        if (!Guid.TryParse(idStr, out var id))
            return $"ID inválido: '{idStr}'.";

        try
        {
            Domain.Entities.Common.TenantEntity? entity = modulo switch
            {
                "obras" => await _ctx.Obras.FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId && !o.IsDeleted, ct),
                "clientes" => await _ctx.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct),
                "fornecedores" => await _ctx.Fornecedores.FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId && !f.IsDeleted, ct),
                "contratos" => await _ctx.Contratos.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct),
                "compras" => await _ctx.PedidosCompra.FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId && !p.IsDeleted, ct),
                "estoque" => await _ctx.Materiais.FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId && !m.IsDeleted, ct),
                "financeiro" => await _ctx.LancamentosFinanceiros.FirstOrDefaultAsync(l => l.Id == id && l.EmpresaId == empresaId && !l.IsDeleted, ct),
                "rh" => await _ctx.Funcionarios.FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId && !f.IsDeleted, ct),
                _ => null
            };

            if (entity == null)
                return modulo is "empresas" or "usuarios"
                    ? $"Exclusão via agente não é permitida para o módulo '{modulo}'."
                    : $"Registro {id} não encontrado no módulo '{modulo}'.";

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await _uow.SaveChangesAsync(ct);
            return $"Registro {id} excluído com sucesso do módulo '{modulo}'.";
        }
        catch (Exception ex)
        {
            return $"Erro ao excluir registro no módulo '{modulo}': {ex.Message}";
        }
    }

    private async Task<string> SalvarAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;
        var operacao = root.TryGetProperty("operacao", out var opProp) ? opProp.GetString()! : "criar";

        // dados pode vir como objeto JSON ou como string JSON escapada
        JsonElement dados;
        if (root.TryGetProperty("dados", out var dadosProp))
        {
            if (dadosProp.ValueKind == JsonValueKind.String)
            {
                // OpenAI às vezes envia dados como string escapada
                using var dadosDoc = JsonDocument.Parse(dadosProp.GetString()!);
                dados = dadosDoc.RootElement.Clone();
            }
            else
            {
                dados = dadosProp;
            }
        }
        else
        {
            return "Campo 'dados' é obrigatório para a operação de salvamento.";
        }

        var id = root.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.String
            ? Guid.TryParse(idProp.GetString(), out var parsedId) ? parsedId : (Guid?)null
            : null;

        try
        {
            return modulo switch
            {
                "obras" => await SalvarObraAsync(empresaId, operacao, id, dados, ct),
                "clientes" => await SalvarClienteAsync(empresaId, operacao, id, dados, ct),
                "fornecedores" => await SalvarFornecedorAsync(empresaId, operacao, id, dados, ct),
                "contratos" => await SalvarContratoAsync(empresaId, operacao, id, dados, ct),
                "compras" => await SalvarCompraAsync(empresaId, operacao, id, dados, ct),
                "estoque" => await SalvarEstoqueAsync(empresaId, operacao, id, dados, ct),
                "financeiro" => await SalvarFinanceiroAsync(empresaId, operacao, id, dados, ct),
                "rh" => await SalvarRHAsync(empresaId, operacao, id, dados, ct),
                "cronograma" => await SalvarCronogramaAsync(empresaId, operacao, id, dados, ct),
                "orcamento" => await SalvarOrcamentoAsync(empresaId, operacao, id, dados, ct),
                "qualidade" => await SalvarQualidadeAsync(empresaId, operacao, id, dados, ct),
                "sst" => await SalvarSSTAsync(empresaId, operacao, id, dados, ct),
                "ged" => await SalvarGEDAsync(empresaId, operacao, id, dados, ct),
                "usuarios" => await SalvarUsuarioAsync(empresaId, operacao, id, dados, ct),
                "empresas" => "Salvamento de empresas não é permitido via agente.",
                _ => $"Salvamento via agente ainda não implementado para o módulo '{modulo}'."
            };
        }
        catch (Exception ex)
        {
            return $"Erro ao salvar no módulo '{modulo}': {ex.Message}";
        }
    }

    private async Task<string> SalvarObraAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Obras.Obra>(
                "OBR-", empresaId, o => o.EmpresaId == empresaId, c => o => o.EmpresaId == empresaId && o.Codigo == c, ct);
            var obra = new Domain.Entities.Obras.Obra
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Nova Obra",
                Tipo = dados.TryGetProperty("tipo", out var t) && Enum.TryParse<Domain.Enums.TipoObraEnum>(t.GetString(), true, out var tp) ? tp : Domain.Enums.TipoObraEnum.Residencial,
                TipoContrato = dados.TryGetProperty("tipo_contrato", out var tc) && Enum.TryParse<Domain.Enums.TipoContratoObraEnum>(tc.GetString(), true, out var tco) ? tco : Domain.Enums.TipoContratoObraEnum.Empreitada,
                DataInicioPrevista = dados.TryGetProperty("data_inicio", out var di) ? DateTime.Parse(di.GetString()!) : DateTime.UtcNow,
                DataFimPrevista = dados.TryGetProperty("data_fim", out var df) ? DateTime.Parse(df.GetString()!) : DateTime.UtcNow.AddMonths(12),
                ValorContrato = dados.TryGetProperty("valor_contrato", out var vc) ? vc.GetDecimal() : 0,
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
                Endereco = new Domain.Entities.Common.Endereco
                {
                    EmpresaId = empresaId,
                    Logradouro = dados.TryGetProperty("logradouro", out var lg) ? lg.GetString() : null,
                    Numero = dados.TryGetProperty("numero", out var nu) ? nu.GetString() : null,
                    Bairro = dados.TryGetProperty("bairro", out var ba) ? ba.GetString() : null,
                    Cidade = dados.TryGetProperty("cidade", out var ci) ? ci.GetString() : null,
                    Estado = dados.TryGetProperty("estado", out var es) ? es.GetString() : null,
                    Cep = dados.TryGetProperty("cep", out var ce) ? ce.GetString() : null,
                }
            };
            await _ctx.Obras.AddAsync(obra, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Obra '{obra.Nome}' criada com sucesso. Código: {obra.Codigo}, Id: {obra.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var obra = await _ctx.Obras.FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId, ct);
            if (obra == null) return $"Obra {id} não encontrada.";
            if (dados.TryGetProperty("nome", out var n)) obra.Nome = n.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) obra.Descricao = d.GetString();
            if (dados.TryGetProperty("observacoes", out var o2)) obra.Observacoes = o2.GetString();
            if (dados.TryGetProperty("valor_contrato", out var vc)) obra.ValorContrato = vc.GetDecimal();
            await _uow.SaveChangesAsync(ct);
            return $"Obra '{obra.Nome}' atualizada com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarClienteAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Clientes.Cliente>(
                "CLI-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Codigo == c, ct);
            var cliente = new Domain.Entities.Clientes.Cliente
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Cliente",
                TipoPessoa = dados.TryGetProperty("tipo_pessoa", out var tp) && Enum.TryParse<Domain.Enums.TipoPessoaEnum>(tp.GetString(), true, out var tpe) ? tpe : Domain.Enums.TipoPessoaEnum.PessoaJuridica,
                Documento = dados.TryGetProperty("documento", out var doc) ? doc.GetString() : null,
                Email = dados.TryGetProperty("email", out var em) ? em.GetString() : null,
                Telefone = dados.TryGetProperty("telefone", out var tel) ? tel.GetString() : null,
            };
            await _ctx.Clientes.AddAsync(cliente, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Cliente '{cliente.Nome}' criado com sucesso. Código: {cliente.Codigo}, Id: {cliente.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var cliente = await _ctx.Clientes.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId, ct);
            if (cliente == null) return $"Cliente {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) cliente.Nome = n.GetString()!;
            if (dados.TryGetProperty("email", out var e)) cliente.Email = e.GetString();
            if (dados.TryGetProperty("telefone", out var t)) cliente.Telefone = t.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Cliente '{cliente.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarFornecedorAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Fornecedores.Fornecedor>(
                "FOR-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Codigo == c, ct);
            var fornecedor = new Domain.Entities.Fornecedores.Fornecedor
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                RazaoSocial = dados.TryGetProperty("razao_social", out var rs) ? rs.GetString()! : "Novo Fornecedor",
                Documento = dados.TryGetProperty("documento", out var doc) ? doc.GetString()! : "",
                Email = dados.TryGetProperty("email", out var em) ? em.GetString()! : "",
                TipoPessoaEnum = dados.TryGetProperty("tipo_pessoa", out var tp) && Enum.TryParse<Domain.Enums.TipoPessoaEnum>(tp.GetString(), true, out var tpe) ? tpe : Domain.Enums.TipoPessoaEnum.PessoaJuridica,
                Tipo = dados.TryGetProperty("tipo", out var ti) && Enum.TryParse<Domain.Enums.TipoFornecedorEnum>(ti.GetString(), true, out var tfi) ? tfi : Domain.Enums.TipoFornecedorEnum.Material,
            };
            await _ctx.Fornecedores.AddAsync(fornecedor, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Fornecedor '{fornecedor.RazaoSocial}' criado com sucesso. Código: {fornecedor.Codigo}, Id: {fornecedor.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var fornecedor = await _ctx.Fornecedores.FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId, ct);
            if (fornecedor == null) return $"Fornecedor {id} não encontrado.";
            if (dados.TryGetProperty("razao_social", out var rs)) fornecedor.RazaoSocial = rs.GetString()!;
            if (dados.TryGetProperty("email", out var e)) fornecedor.Email = e.GetString()!;
            if (dados.TryGetProperty("telefone", out var t)) fornecedor.Telefone = t.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Fornecedor '{fornecedor.RazaoSocial}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarContratoAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Contratos.Contrato>(
                "CTR-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Numero == c, ct);
            var contrato = new Domain.Entities.Contratos.Contrato
            {
                EmpresaId = empresaId,
                Numero = codigo,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                FornecedorId = dados.TryGetProperty("fornecedor_id", out var fi) && Guid.TryParse(fi.GetString(), out var fornId) ? fornId : Guid.Empty,
                Objeto = dados.TryGetProperty("objeto", out var obj) ? obj.GetString()! : "Novo Contrato",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Tipo = dados.TryGetProperty("tipo", out var tp) && Enum.TryParse<TipoContratoFornecedorEnum>(tp.GetString(), true, out var tpe) ? tpe : TipoContratoFornecedorEnum.Global,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusContratoEnum>(st.GetString(), true, out var ste) ? ste : StatusContratoEnum.Rascunho,
                DataAssinatura = dados.TryGetProperty("data_assinatura", out var da) && DateTime.TryParse(da.GetString(), out var daVal) ? daVal : DateTime.UtcNow,
                DataVigenciaInicio = dados.TryGetProperty("data_vigencia_inicio", out var dvi) && DateTime.TryParse(dvi.GetString(), out var dviVal) ? dviVal : DateTime.UtcNow,
                DataVigenciaFim = dados.TryGetProperty("data_vigencia_fim", out var dvf) && DateTime.TryParse(dvf.GetString(), out var dvfVal) ? dvfVal : DateTime.UtcNow.AddMonths(12),
                ValorGlobal = dados.TryGetProperty("valor_global", out var vg) ? vg.GetDecimal() : 0,
                CondicoesPagamento = dados.TryGetProperty("condicoes_pagamento", out var cp) ? cp.GetString() : null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
            };
            await _ctx.Contratos.AddAsync(contrato, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Contrato '{contrato.Objeto}' criado com sucesso. Número: {contrato.Numero}, Id: {contrato.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var contrato = await _ctx.Contratos.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId, ct);
            if (contrato == null) return $"Contrato {id} não encontrado.";
            if (dados.TryGetProperty("objeto", out var obj)) contrato.Objeto = obj.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) contrato.Descricao = d.GetString();
            if (dados.TryGetProperty("valor_global", out var vg)) contrato.ValorGlobal = vg.GetDecimal();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusContratoEnum>(st.GetString(), true, out var ste)) contrato.Status = ste;
            if (dados.TryGetProperty("observacoes", out var obs)) contrato.Observacoes = obs.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Contrato '{contrato.Objeto}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarCompraAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Compras.PedidoCompra>(
                "PED-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Numero == c, ct);
            var pedido = new Domain.Entities.Compras.PedidoCompra
            {
                EmpresaId = empresaId,
                Numero = codigo,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                FornecedorId = dados.TryGetProperty("fornecedor_id", out var fi) && Guid.TryParse(fi.GetString(), out var fornId) ? fornId : Guid.Empty,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusPedidoCompraEnum>(st.GetString(), true, out var ste) ? ste : StatusPedidoCompraEnum.Rascunho,
                DataPedido = dados.TryGetProperty("data_pedido", out var dp) && DateTime.TryParse(dp.GetString(), out var dpVal) ? dpVal : DateTime.UtcNow,
                DataEntregaPrevista = dados.TryGetProperty("data_entrega_prevista", out var dep) && DateTime.TryParse(dep.GetString(), out var depVal) ? depVal : (DateTime?)null,
                CondicoesPagamento = dados.TryGetProperty("condicoes_pagamento", out var cp) ? cp.GetString() : null,
                LocalEntrega = dados.TryGetProperty("local_entrega", out var le) ? le.GetString() : null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
                ValorFrete = dados.TryGetProperty("valor_frete", out var vf) ? vf.GetDecimal() : 0,
                ValorDesconto = dados.TryGetProperty("valor_desconto", out var vd) ? vd.GetDecimal() : 0,
                ValorTotal = dados.TryGetProperty("valor_total", out var vt) ? vt.GetDecimal() : 0,
                CriadoPor = Guid.Empty,
            };
            await _ctx.PedidosCompra.AddAsync(pedido, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Pedido de compra criado com sucesso. Número: {pedido.Numero}, Id: {pedido.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var pedido = await _ctx.PedidosCompra.FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId, ct);
            if (pedido == null) return $"Pedido de compra {id} não encontrado.";
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusPedidoCompraEnum>(st.GetString(), true, out var ste)) pedido.Status = ste;
            if (dados.TryGetProperty("observacoes", out var obs)) pedido.Observacoes = obs.GetString();
            if (dados.TryGetProperty("valor_total", out var vt)) pedido.ValorTotal = vt.GetDecimal();
            if (dados.TryGetProperty("local_entrega", out var le)) pedido.LocalEntrega = le.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Pedido de compra '{pedido.Numero}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarEstoqueAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Estoque.Material>(
                "MAT-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Codigo == c, ct);
            var material = new Domain.Entities.Estoque.Material
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Material",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Especificacao = dados.TryGetProperty("especificacao", out var esp) ? esp.GetString() : null,
                UnidadeMedida = dados.TryGetProperty("unidade_medida", out var um) ? um.GetString()! : "UN",
                Tipo = dados.TryGetProperty("tipo", out var tp) && Enum.TryParse<TipoInsumoEnum>(tp.GetString(), true, out var tpe) ? tpe : TipoInsumoEnum.Material,
                EstoqueMinimo = dados.TryGetProperty("estoque_minimo", out var emin) ? emin.GetDecimal() : 0,
                EstoqueMaximo = dados.TryGetProperty("estoque_maximo", out var emax) ? emax.GetDecimal() : 0,
                Marca = dados.TryGetProperty("marca", out var marca) ? marca.GetString() : null,
                Fabricante = dados.TryGetProperty("fabricante", out var fab) ? fab.GetString() : null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
            };
            await _ctx.Materiais.AddAsync(material, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Material '{material.Nome}' criado com sucesso. Código: {material.Codigo}, Id: {material.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var material = await _ctx.Materiais.FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId, ct);
            if (material == null) return $"Material {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) material.Nome = n.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) material.Descricao = d.GetString();
            if (dados.TryGetProperty("unidade_medida", out var um)) material.UnidadeMedida = um.GetString()!;
            if (dados.TryGetProperty("estoque_minimo", out var emin)) material.EstoqueMinimo = emin.GetDecimal();
            if (dados.TryGetProperty("estoque_maximo", out var emax)) material.EstoqueMaximo = emax.GetDecimal();
            if (dados.TryGetProperty("observacoes", out var obs)) material.Observacoes = obs.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Material '{material.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarFinanceiroAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var lancamento = new Domain.Entities.Financeiro.LancamentoFinanceiro
            {
                EmpresaId = empresaId,
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString()! : "Novo Lançamento",
                Tipo = dados.TryGetProperty("tipo", out var tp) && Enum.TryParse<TipoLancamentoEnum>(tp.GetString(), true, out var tpe) ? tpe : TipoLancamentoEnum.Despesa,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusLancamentoEnum>(st.GetString(), true, out var ste) ? ste : StatusLancamentoEnum.Previsto,
                Valor = dados.TryGetProperty("valor", out var val) ? val.GetDecimal() : 0,
                DataVencimento = dados.TryGetProperty("data_vencimento", out var dv) && DateTime.TryParse(dv.GetString(), out var dvVal) ? dvVal : DateTime.UtcNow,
                DataCompetencia = dados.TryGetProperty("data_competencia", out var dc) && DateTime.TryParse(dc.GetString(), out var dcVal) ? dcVal : (DateTime?)null,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : (Guid?)null,
                FornecedorId = dados.TryGetProperty("fornecedor_id", out var fi) && Guid.TryParse(fi.GetString(), out var fornId) ? fornId : (Guid?)null,
                ContratoId = dados.TryGetProperty("contrato_id", out var ci) && Guid.TryParse(ci.GetString(), out var ctrId) ? ctrId : (Guid?)null,
                NumeroDocumento = dados.TryGetProperty("numero_documento", out var nd) ? nd.GetString() : null,
                NumeroNF = dados.TryGetProperty("numero_nf", out var nf) ? nf.GetString() : null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
                CriadoPor = Guid.Empty,
            };
            await _ctx.LancamentosFinanceiros.AddAsync(lancamento, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Lançamento financeiro '{lancamento.Descricao}' criado com sucesso. Id: {lancamento.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var lancamento = await _ctx.LancamentosFinanceiros.FirstOrDefaultAsync(l => l.Id == id && l.EmpresaId == empresaId, ct);
            if (lancamento == null) return $"Lançamento financeiro {id} não encontrado.";
            if (dados.TryGetProperty("descricao", out var d)) lancamento.Descricao = d.GetString()!;
            if (dados.TryGetProperty("valor", out var v)) lancamento.Valor = v.GetDecimal();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusLancamentoEnum>(st.GetString(), true, out var ste)) lancamento.Status = ste;
            if (dados.TryGetProperty("observacoes", out var obs)) lancamento.Observacoes = obs.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Lançamento financeiro '{lancamento.Descricao}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarRHAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.RH.Funcionario>(
                "FUN-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Matricula == c, ct);
            var funcionario = new Domain.Entities.RH.Funcionario
            {
                EmpresaId = empresaId,
                Matricula = codigo,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Funcionário",
                Cpf = dados.TryGetProperty("cpf", out var cpf) ? cpf.GetString()! : "",
                Email = dados.TryGetProperty("email", out var em) ? em.GetString()! : "",
                Telefone = dados.TryGetProperty("telefone", out var tel) ? tel.GetString() : null,
                Celular = dados.TryGetProperty("celular", out var cel) ? cel.GetString() : null,
                DataNascimento = dados.TryGetProperty("data_nascimento", out var dn) && DateTime.TryParse(dn.GetString(), out var dnVal) ? dnVal : DateTime.UtcNow,
                DataAdmissao = dados.TryGetProperty("data_admissao", out var da) && DateTime.TryParse(da.GetString(), out var daVal) ? daVal : DateTime.UtcNow,
                TipoContratacaoEnum = dados.TryGetProperty("tipo_contratacao", out var tc) && Enum.TryParse<TipoContratacaoEnum>(tc.GetString(), true, out var tce) ? tce : TipoContratacaoEnum.CLT,
                SalarioBase = dados.TryGetProperty("salario_base", out var sb) ? sb.GetDecimal() : 0,
                CargoId = dados.TryGetProperty("cargo_id", out var ci) && Guid.TryParse(ci.GetString(), out var cargoId) ? cargoId : (Guid?)null,
                DepartamentoId = dados.TryGetProperty("departamento_id", out var di) && Guid.TryParse(di.GetString(), out var depId) ? depId : (Guid?)null,
                ObraAtualId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : (Guid?)null,
            };
            await _ctx.Funcionarios.AddAsync(funcionario, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Funcionário '{funcionario.Nome}' criado com sucesso. Matrícula: {funcionario.Matricula}, Id: {funcionario.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var funcionario = await _ctx.Funcionarios.FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId, ct);
            if (funcionario == null) return $"Funcionário {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) funcionario.Nome = n.GetString()!;
            if (dados.TryGetProperty("email", out var e)) funcionario.Email = e.GetString()!;
            if (dados.TryGetProperty("telefone", out var t)) funcionario.Telefone = t.GetString();
            if (dados.TryGetProperty("salario_base", out var sb)) funcionario.SalarioBase = sb.GetDecimal();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusFuncionarioEnum>(st.GetString(), true, out var ste)) funcionario.Status = ste;
            await _uow.SaveChangesAsync(ct);
            return $"Funcionário '{funcionario.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarCronogramaAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var cronograma = new Domain.Entities.Cronograma.CronogramaObra
            {
                EmpresaId = empresaId,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Cronograma",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                DataInicio = dados.TryGetProperty("data_inicio", out var di) && DateTime.TryParse(di.GetString(), out var diVal) ? diVal : DateTime.UtcNow,
                DataFim = dados.TryGetProperty("data_fim", out var df) && DateTime.TryParse(df.GetString(), out var dfVal) ? dfVal : DateTime.UtcNow.AddMonths(12),
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
            };
            await _ctx.Cronogramas.AddAsync(cronograma, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Cronograma '{cronograma.Nome}' criado com sucesso. Id: {cronograma.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var cronograma = await _ctx.Cronogramas.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId, ct);
            if (cronograma == null) return $"Cronograma {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) cronograma.Nome = n.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) cronograma.Descricao = d.GetString();
            if (dados.TryGetProperty("data_inicio", out var di) && DateTime.TryParse(di.GetString(), out var diVal)) cronograma.DataInicio = diVal;
            if (dados.TryGetProperty("data_fim", out var df) && DateTime.TryParse(df.GetString(), out var dfVal)) cronograma.DataFim = dfVal;
            if (dados.TryGetProperty("observacoes", out var obs)) cronograma.Observacoes = obs.GetString();
            if (dados.TryGetProperty("ativo", out var at)) cronograma.Ativo = at.GetBoolean();
            await _uow.SaveChangesAsync(ct);
            return $"Cronograma '{cronograma.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarOrcamentoAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Orcamento.Orcamento>(
                "ORC-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Codigo == c, ct);
            var orcamento = new Domain.Entities.Orcamento.Orcamento
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Orçamento",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusOrcamentoEnum>(st.GetString(), true, out var ste) ? ste : StatusOrcamentoEnum.Rascunho,
                DataReferencia = dados.TryGetProperty("data_referencia", out var dr) && DateTime.TryParse(dr.GetString(), out var drVal) ? drVal : DateTime.UtcNow,
                BaseOrcamentaria = dados.TryGetProperty("base_orcamentaria", out var bo) ? bo.GetString() : null,
                Localidade = dados.TryGetProperty("localidade", out var loc) ? loc.GetString() : null,
                BDI = dados.TryGetProperty("bdi", out var bdi) ? bdi.GetDecimal() : 0,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
            };
            await _ctx.Orcamentos.AddAsync(orcamento, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Orçamento '{orcamento.Nome}' criado com sucesso. Código: {orcamento.Codigo}, Id: {orcamento.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var orcamento = await _ctx.Orcamentos.FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId, ct);
            if (orcamento == null) return $"Orçamento {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) orcamento.Nome = n.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) orcamento.Descricao = d.GetString();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusOrcamentoEnum>(st.GetString(), true, out var ste)) orcamento.Status = ste;
            if (dados.TryGetProperty("bdi", out var bdi)) orcamento.BDI = bdi.GetDecimal();
            if (dados.TryGetProperty("observacoes", out var obs)) orcamento.Observacoes = obs.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Orçamento '{orcamento.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarQualidadeAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.Qualidade.Inspecao>(
                "INS-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Numero == c, ct);
            var inspecao = new Domain.Entities.Qualidade.Inspecao
            {
                EmpresaId = empresaId,
                Numero = codigo,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                Titulo = dados.TryGetProperty("titulo", out var t) ? t.GetString()! : "Nova Inspeção",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusInspecaoEnum>(st.GetString(), true, out var ste) ? ste : StatusInspecaoEnum.Pendente,
                DataProgramada = dados.TryGetProperty("data_programada", out var dp) && DateTime.TryParse(dp.GetString(), out var dpVal) ? dpVal : DateTime.UtcNow,
                Localicacao = dados.TryGetProperty("localizacao", out var loc) ? loc.GetString() : null,
                InspetorId = dados.TryGetProperty("inspetor_id", out var ii) && Guid.TryParse(ii.GetString(), out var inspId) ? inspId : (Guid?)null,
                Observacoes = dados.TryGetProperty("observacoes", out var obs) ? obs.GetString() : null,
            };
            await _ctx.Inspecoes.AddAsync(inspecao, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Inspeção '{inspecao.Titulo}' criada com sucesso. Número: {inspecao.Numero}, Id: {inspecao.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var inspecao = await _ctx.Inspecoes.FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId, ct);
            if (inspecao == null) return $"Inspeção {id} não encontrada.";
            if (dados.TryGetProperty("titulo", out var t)) inspecao.Titulo = t.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) inspecao.Descricao = d.GetString();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusInspecaoEnum>(st.GetString(), true, out var ste)) inspecao.Status = ste;
            if (dados.TryGetProperty("resultado", out var r)) inspecao.Resultado = r.GetString();
            if (dados.TryGetProperty("observacoes", out var obs)) inspecao.Observacoes = obs.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Inspeção '{inspecao.Titulo}' atualizada com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarSSTAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.SST.DDS>(
                "DDS-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Numero == c, ct);
            var dds = new Domain.Entities.SST.DDS
            {
                EmpresaId = empresaId,
                Numero = codigo,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : Guid.Empty,
                Tema = dados.TryGetProperty("tema", out var t) ? t.GetString()! : "Novo DDS",
                Conteudo = dados.TryGetProperty("conteudo", out var c) ? c.GetString() : null,
                Ministrador = dados.TryGetProperty("ministrador", out var m) ? m.GetString() : null,
                DataRealizacao = dados.TryGetProperty("data_realizacao", out var dr) && DateTime.TryParse(dr.GetString(), out var drVal) ? drVal : DateTime.UtcNow,
                DuracaoMinutos = dados.TryGetProperty("duracao_minutos", out var dm) ? dm.GetDecimal() : 15,
                Local = dados.TryGetProperty("local", out var l) ? l.GetString() : null,
                NumeroParticipantes = dados.TryGetProperty("numero_participantes", out var np) ? np.GetInt32() : 0,
                RegistradoPor = Guid.Empty,
            };
            await _ctx.DDSs.AddAsync(dds, ct);
            await _uow.SaveChangesAsync(ct);
            return $"DDS '{dds.Tema}' criado com sucesso. Número: {dds.Numero}, Id: {dds.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var dds = await _ctx.DDSs.FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId, ct);
            if (dds == null) return $"DDS {id} não encontrado.";
            if (dados.TryGetProperty("tema", out var t)) dds.Tema = t.GetString()!;
            if (dados.TryGetProperty("conteudo", out var c)) dds.Conteudo = c.GetString();
            if (dados.TryGetProperty("ministrador", out var m)) dds.Ministrador = m.GetString();
            if (dados.TryGetProperty("numero_participantes", out var np)) dds.NumeroParticipantes = np.GetInt32();
            await _uow.SaveChangesAsync(ct);
            return $"DDS '{dds.Tema}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarGEDAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var codigo = await GerarCodigoUnicoAsync<Domain.Entities.GED.DocumentoGED>(
                "DOC-", empresaId, e => e.EmpresaId == empresaId, c => e => e.EmpresaId == empresaId && e.Codigo == c, ct);
            var documento = new Domain.Entities.GED.DocumentoGED
            {
                EmpresaId = empresaId,
                Codigo = codigo,
                PastaId = dados.TryGetProperty("pasta_id", out var pi) && Guid.TryParse(pi.GetString(), out var pastaId) ? pastaId : Guid.Empty,
                ObraId = dados.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var obraId) ? obraId : (Guid?)null,
                Titulo = dados.TryGetProperty("titulo", out var t) ? t.GetString()! : "Novo Documento",
                Descricao = dados.TryGetProperty("descricao", out var desc) ? desc.GetString() : null,
                Tipo = dados.TryGetProperty("tipo", out var tp) && Enum.TryParse<TipoDocumentoGEDEnum>(tp.GetString(), true, out var tpe) ? tpe : TipoDocumentoGEDEnum.Outros,
                Status = dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusDocumentoGEDEnum>(st.GetString(), true, out var ste) ? ste : StatusDocumentoGEDEnum.Rascunho,
                NormaReferencia = dados.TryGetProperty("norma_referencia", out var nr) ? nr.GetString() : null,
                Palavraschave = dados.TryGetProperty("palavras_chave", out var pc) ? pc.GetString() : null,
                DataVigencia = dados.TryGetProperty("data_vigencia", out var dv) && DateTime.TryParse(dv.GetString(), out var dvVal) ? dvVal : (DateTime?)null,
            };
            await _ctx.DocumentosGED.AddAsync(documento, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Documento GED '{documento.Titulo}' criado com sucesso. Código: {documento.Codigo}, Id: {documento.Id}";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var documento = await _ctx.DocumentosGED.FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId, ct);
            if (documento == null) return $"Documento GED {id} não encontrado.";
            if (dados.TryGetProperty("titulo", out var t)) documento.Titulo = t.GetString()!;
            if (dados.TryGetProperty("descricao", out var d)) documento.Descricao = d.GetString();
            if (dados.TryGetProperty("status", out var st) && Enum.TryParse<StatusDocumentoGEDEnum>(st.GetString(), true, out var ste)) documento.Status = ste;
            if (dados.TryGetProperty("palavras_chave", out var pc)) documento.Palavraschave = pc.GetString();
            await _uow.SaveChangesAsync(ct);
            return $"Documento GED '{documento.Titulo}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> SalvarUsuarioAsync(Guid empresaId, string operacao, Guid? id, JsonElement dados, CancellationToken ct)
    {
        if (operacao == "criar")
        {
            var usuario = new Domain.Entities.Tenant.Usuario
            {
                EmpresaId = empresaId,
                Nome = dados.TryGetProperty("nome", out var n) ? n.GetString()! : "Novo Usuário",
                Email = dados.TryGetProperty("email", out var em) ? em.GetString()! : "",
                PasswordHash = "",  // Deve ser definido via fluxo próprio de criação
                Telefone = dados.TryGetProperty("telefone", out var tel) ? tel.GetString() : null,
                Cargo = dados.TryGetProperty("cargo", out var c) ? c.GetString() : null,
                Perfil = dados.TryGetProperty("perfil", out var p) && Enum.TryParse<PerfilUsuarioEnum>(p.GetString(), true, out var pe) ? pe : PerfilUsuarioEnum.Colaborador,
            };
            await _ctx.Usuarios.AddAsync(usuario, ct);
            await _uow.SaveChangesAsync(ct);
            return $"Usuário '{usuario.Nome}' criado com sucesso. Id: {usuario.Id}. Nota: a senha deve ser definida pelo próprio usuário.";
        }
        else if (operacao == "atualizar" && id.HasValue)
        {
            var usuario = await _ctx.Usuarios.FirstOrDefaultAsync(u => u.Id == id && u.EmpresaId == empresaId, ct);
            if (usuario == null) return $"Usuário {id} não encontrado.";
            if (dados.TryGetProperty("nome", out var n)) usuario.Nome = n.GetString()!;
            if (dados.TryGetProperty("email", out var e)) usuario.Email = e.GetString()!;
            if (dados.TryGetProperty("telefone", out var t)) usuario.Telefone = t.GetString();
            if (dados.TryGetProperty("cargo", out var c)) usuario.Cargo = c.GetString();
            if (dados.TryGetProperty("perfil", out var p) && Enum.TryParse<PerfilUsuarioEnum>(p.GetString(), true, out var pe)) usuario.Perfil = pe;
            if (dados.TryGetProperty("ativo", out var a)) usuario.Ativo = a.GetBoolean();
            await _uow.SaveChangesAsync(ct);
            return $"Usuário '{usuario.Nome}' atualizado com sucesso.";
        }
        return "Operação inválida. Use 'criar' ou 'atualizar'.";
    }

    private async Task<string> GerarRelatorioAsync(string argsJson, Guid empresaId, CancellationToken ct)
    {
        using var doc = JsonDocument.Parse(argsJson);
        var root = doc.RootElement;
        var modulo = root.GetProperty("modulo").GetString()!;
        var obraId = root.TryGetProperty("obra_id", out var oi) && Guid.TryParse(oi.GetString(), out var oid) ? oid : (Guid?)null;
        var periodoInicio = root.TryGetProperty("periodo_inicio", out var pi) && DateTime.TryParse(pi.GetString(), out var piVal) ? piVal : (DateTime?)null;
        var periodoFim = root.TryGetProperty("periodo_fim", out var pf) && DateTime.TryParse(pf.GetString(), out var pfVal) ? pfVal : (DateTime?)null;

        return modulo switch
        {
            "obras" => await GerarRelatorioObrasAsync(empresaId, obraId, ct),
            "estoque" => await GerarRelatorioEstoqueAsync(empresaId, ct),
            "financeiro" => await GerarRelatorioFinanceiroAsync(empresaId, obraId, periodoInicio, periodoFim, ct),
            "contratos" => await GerarRelatorioContratosAsync(empresaId, obraId, ct),
            "compras" => await GerarRelatorioComprasAsync(empresaId, obraId, ct),
            "rh" => await GerarRelatorioRHAsync(empresaId, ct),
            "qualidade" => await GerarRelatorioQualidadeAsync(empresaId, obraId, ct),
            "sst" => await GerarRelatorioSSTAsync(empresaId, obraId, ct),
            "cronograma" => await GerarRelatorioCronogramaAsync(empresaId, obraId, ct),
            "orcamento" => await GerarRelatorioOrcamentoAsync(empresaId, obraId, ct),
            "fornecedores" => await GerarRelatorioFornecedoresAsync(empresaId, ct),
            "clientes" => await GerarRelatorioClientesAsync(empresaId, ct),
            _ => "Relatório em desenvolvimento."
        };
    }

    private async Task<string> GerarRelatorioObrasAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.Obras
            .Where(o => o.EmpresaId == empresaId && !o.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(o => o.Id == obraId.Value);

        var obras = await query.ToListAsync(ct);
        var totalObras = obras.Count;
        var porStatus = obras.GroupBy(o => o.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();
        var totalValorContrato = obras.Sum(o => o.ValorContrato);

        var relatorio = new
        {
            Modulo = "obras",
            TotalObras = totalObras,
            PorStatus = porStatus,
            TotalValorContrato = totalValorContrato
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioEstoqueAsync(Guid empresaId, CancellationToken ct)
    {
        var materiais = await _ctx.Materiais
            .Where(m => m.EmpresaId == empresaId && !m.IsDeleted)
            .AsNoTracking()
            .ToListAsync(ct);

        var saldos = await _ctx.EstoquesSaldos
            .Where(s => s.EmpresaId == empresaId && !s.IsDeleted)
            .AsNoTracking()
            .ToListAsync(ct);

        var totalMateriais = materiais.Count;
        var saldoTotal = saldos.Sum(s => s.SaldoAtual);
        var top10 = saldos
            .OrderByDescending(s => s.SaldoAtual)
            .Take(10)
            .Select(s => new
            {
                s.MaterialId,
                Material = materiais.FirstOrDefault(m => m.Id == s.MaterialId)?.Nome ?? "N/A",
                SaldoAtual = s.SaldoAtual
            })
            .ToList();

        var relatorio = new
        {
            Modulo = "estoque",
            TotalMateriais = totalMateriais,
            SaldoTotal = saldoTotal,
            Top10PorQuantidade = top10
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioFinanceiroAsync(Guid empresaId, Guid? obraId, DateTime? periodoInicio, DateTime? periodoFim, CancellationToken ct)
    {
        var query = _ctx.LancamentosFinanceiros
            .Where(l => l.EmpresaId == empresaId && !l.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(l => l.ObraId == obraId.Value);
        if (periodoInicio.HasValue)
            query = query.Where(l => l.DataVencimento >= periodoInicio.Value);
        if (periodoFim.HasValue)
            query = query.Where(l => l.DataVencimento <= periodoFim.Value);

        var lancamentos = await query.ToListAsync(ct);
        var totalReceitas = lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Receita).Sum(l => l.Valor);
        var totalDespesas = lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Despesa).Sum(l => l.Valor);
        var saldo = totalReceitas - totalDespesas;

        var relatorio = new
        {
            Modulo = "financeiro",
            PeriodoInicio = periodoInicio?.ToString("yyyy-MM-dd"),
            PeriodoFim = periodoFim?.ToString("yyyy-MM-dd"),
            TotalReceitas = totalReceitas,
            TotalDespesas = totalDespesas,
            Saldo = saldo,
            TotalLancamentos = lancamentos.Count
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioContratosAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.Contratos
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(c => c.ObraId == obraId.Value);

        var contratos = await query.ToListAsync(ct);
        var totalContratos = contratos.Count;
        var porStatus = contratos.GroupBy(c => c.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();
        var totalValorGlobal = contratos.Sum(c => c.ValorGlobal);
        var totalMedido = contratos.Sum(c => c.ValorMedidoAcumulado);

        var relatorio = new
        {
            Modulo = "contratos",
            TotalContratos = totalContratos,
            PorStatus = porStatus,
            TotalValorGlobal = totalValorGlobal,
            TotalValorMedido = totalMedido
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioComprasAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.PedidosCompra
            .Where(p => p.EmpresaId == empresaId && !p.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(p => p.ObraId == obraId.Value);

        var pedidos = await query.ToListAsync(ct);
        var totalPedidos = pedidos.Count;
        var porStatus = pedidos.GroupBy(p => p.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();
        var totalValor = pedidos.Sum(p => p.ValorTotal);

        var relatorio = new
        {
            Modulo = "compras",
            TotalPedidos = totalPedidos,
            PorStatus = porStatus,
            TotalValor = totalValor
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioRHAsync(Guid empresaId, CancellationToken ct)
    {
        var funcionarios = await _ctx.Funcionarios
            .Where(f => f.EmpresaId == empresaId && !f.IsDeleted)
            .AsNoTracking()
            .ToListAsync(ct);

        var totalFuncionarios = funcionarios.Count;
        var porStatus = funcionarios.GroupBy(f => f.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();
        var totalSalarioBase = funcionarios.Sum(f => f.SalarioBase);

        var relatorio = new
        {
            Modulo = "rh",
            TotalFuncionarios = totalFuncionarios,
            PorStatus = porStatus,
            TotalSalarioBase = totalSalarioBase
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioQualidadeAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.Inspecoes
            .Where(i => i.EmpresaId == empresaId && !i.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(i => i.ObraId == obraId.Value);

        var inspecoes = await query.ToListAsync(ct);
        var totalInspecoes = inspecoes.Count;
        var porStatus = inspecoes.GroupBy(i => i.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();

        var relatorio = new
        {
            Modulo = "qualidade",
            TotalInspecoes = totalInspecoes,
            PorStatus = porStatus
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioSSTAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var queryDds = _ctx.DDSs
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .AsNoTracking();

        var queryAcidentes = _ctx.RegistrosAcidentes
            .Where(a => a.EmpresaId == empresaId && !a.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
        {
            queryDds = queryDds.Where(d => d.ObraId == obraId.Value);
            queryAcidentes = queryAcidentes.Where(a => a.ObraId == obraId.Value);
        }

        var totalDDS = await queryDds.CountAsync(ct);
        var totalAcidentes = await queryAcidentes.CountAsync(ct);

        var relatorio = new
        {
            Modulo = "sst",
            TotalDDS = totalDDS,
            TotalAcidentes = totalAcidentes
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioCronogramaAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.Cronogramas
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(c => c.ObraId == obraId.Value);

        var cronogramas = await query.ToListAsync(ct);
        var totalCronogramas = cronogramas.Count;
        var porStatus = cronogramas.GroupBy(c => c.Ativo ? "Ativo" : "Inativo")
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();

        var relatorio = new
        {
            Modulo = "cronograma",
            TotalCronogramas = totalCronogramas,
            PorStatus = porStatus
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioOrcamentoAsync(Guid empresaId, Guid? obraId, CancellationToken ct)
    {
        var query = _ctx.Orcamentos
            .Where(o => o.EmpresaId == empresaId && !o.IsDeleted)
            .AsNoTracking();

        if (obraId.HasValue)
            query = query.Where(o => o.ObraId == obraId.Value);

        var orcamentos = await query.ToListAsync(ct);
        var totalOrcamentos = orcamentos.Count;
        var porStatus = orcamentos.GroupBy(o => o.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();
        var totalValor = orcamentos.Sum(o => o.ValorTotal);

        var relatorio = new
        {
            Modulo = "orcamento",
            TotalOrcamentos = totalOrcamentos,
            PorStatus = porStatus,
            TotalValor = totalValor
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioFornecedoresAsync(Guid empresaId, CancellationToken ct)
    {
        var fornecedores = await _ctx.Fornecedores
            .Where(f => f.EmpresaId == empresaId && !f.IsDeleted)
            .AsNoTracking()
            .ToListAsync(ct);

        var totalFornecedores = fornecedores.Count;
        var ativos = fornecedores.Count(f => f.Ativo);
        var inativos = totalFornecedores - ativos;
        var homologados = fornecedores.Count(f => f.Homologado);

        var relatorio = new
        {
            Modulo = "fornecedores",
            TotalFornecedores = totalFornecedores,
            Ativos = ativos,
            Inativos = inativos,
            Homologados = homologados
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
    }

    private async Task<string> GerarRelatorioClientesAsync(Guid empresaId, CancellationToken ct)
    {
        var clientes = await _ctx.Clientes
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted)
            .AsNoTracking()
            .ToListAsync(ct);

        var totalClientes = clientes.Count;
        var porStatus = clientes.GroupBy(c => c.Status.ToString())
            .Select(g => new { Status = g.Key, Quantidade = g.Count() })
            .ToList();

        var relatorio = new
        {
            Modulo = "clientes",
            TotalClientes = totalClientes,
            PorStatus = porStatus
        };

        return JsonSerializer.Serialize(relatorio, _jsonOpts);
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

    private async Task<string> ConsultarContratosAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Contratos
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "numero" => query.Where(c => c.Numero == valor),
                    "objeto" => query.Where(c => EF.Functions.ILike(c.Objeto, $"%{valor}%")),
                    "status" => Enum.TryParse<StatusContratoEnum>(valor, true, out var s)
                        ? query.Where(c => c.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(c => new
            {
                c.Id,
                c.Numero,
                c.Objeto,
                Status = c.Status.ToString(),
                c.FornecedorId,
                c.ValorGlobal,
                c.DataVigenciaInicio,
                c.DataVigenciaFim
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarComprasAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.PedidosCompra
            .Where(p => p.EmpresaId == empresaId && !p.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "numero" => query.Where(p => p.Numero == valor),
                    "status" => Enum.TryParse<StatusPedidoCompraEnum>(valor, true, out var s)
                        ? query.Where(p => p.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(p => new
            {
                p.Id,
                p.Numero,
                Status = p.Status.ToString(),
                p.FornecedorId,
                p.ValorTotal,
                p.DataPedido
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarFinanceiroAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.LancamentosFinanceiros
            .Where(l => l.EmpresaId == empresaId && !l.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "descricao" => query.Where(l => EF.Functions.ILike(l.Descricao, $"%{valor}%")),
                    "tipo" => Enum.TryParse<TipoLancamentoEnum>(valor, true, out var t)
                        ? query.Where(l => l.Tipo == t)
                        : query,
                    "status" => Enum.TryParse<StatusLancamentoEnum>(valor, true, out var s)
                        ? query.Where(l => l.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(l => new
            {
                l.Id,
                Tipo = l.Tipo.ToString(),
                l.Valor,
                l.DataVencimento,
                Status = l.Status.ToString(),
                l.Descricao
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarRHAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Funcionarios
            .Where(f => f.EmpresaId == empresaId && !f.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" => query.Where(f => EF.Functions.ILike(f.Nome, $"%{valor}%")),
                    "matricula" => query.Where(f => f.Matricula == valor),
                    "status" => Enum.TryParse<StatusFuncionarioEnum>(valor, true, out var s)
                        ? query.Where(f => f.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(f => new
            {
                f.Id,
                f.Nome,
                f.Matricula,
                Cargo = f.Cargo != null ? f.Cargo.Nome : null,
                Status = f.Status.ToString()
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarCronogramaAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Cronogramas
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
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(c => new
            {
                c.Id,
                c.ObraId,
                c.Nome,
                c.DataInicio,
                c.DataFim,
                c.Versao,
                c.Ativo
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarOrcamentoAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Orcamentos
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
                    "codigo" => query.Where(o => o.Codigo == valor),
                    "status" => Enum.TryParse<StatusOrcamentoEnum>(valor, true, out var s)
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
                o.ObraId,
                o.Codigo,
                o.Nome,
                Status = o.Status.ToString(),
                o.Versao,
                o.ValorTotal,
                o.DataReferencia
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarQualidadeAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Inspecoes
            .Where(i => i.EmpresaId == empresaId && !i.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "titulo" => query.Where(i => EF.Functions.ILike(i.Titulo, $"%{valor}%")),
                    "numero" => query.Where(i => i.Numero == valor),
                    "status" => Enum.TryParse<StatusInspecaoEnum>(valor, true, out var s)
                        ? query.Where(i => i.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(i => new
            {
                i.Id,
                i.ObraId,
                i.Numero,
                i.Titulo,
                Status = i.Status.ToString(),
                i.DataProgramada,
                i.DataRealizacao,
                i.TemNaoConformidade
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarSSTAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.DDSs
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "tema" => query.Where(d => EF.Functions.ILike(d.Tema, $"%{valor}%")),
                    "numero" => query.Where(d => d.Numero == valor),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(d => new
            {
                d.Id,
                d.ObraId,
                d.Numero,
                d.Tema,
                d.DataRealizacao,
                d.Ministrador,
                d.NumeroParticipantes
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarGEDAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.DocumentosGED
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "titulo" => query.Where(d => EF.Functions.ILike(d.Titulo, $"%{valor}%")),
                    "codigo" => query.Where(d => d.Codigo == valor),
                    "status" => Enum.TryParse<StatusDocumentoGEDEnum>(valor, true, out var s)
                        ? query.Where(d => d.Status == s)
                        : query,
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(d => new
            {
                d.Id,
                d.Codigo,
                d.Titulo,
                Tipo = d.Tipo.ToString(),
                Status = d.Status.ToString(),
                d.Versao,
                d.NumeroRevisao,
                d.DataCadastro
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarUsuariosAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Usuarios
            .Where(u => u.EmpresaId == empresaId && !u.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "nome" => query.Where(u => EF.Functions.ILike(u.Nome, $"%{valor}%")),
                    "email" => query.Where(u => EF.Functions.ILike(u.Email, $"%{valor}%")),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(u => new
            {
                u.Id,
                u.Nome,
                u.Email,
                u.Cargo,
                Perfil = u.Perfil.ToString(),
                u.Ativo,
                u.UltimoAcesso
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> ConsultarEmpresasAsync(Guid empresaId, string? filtros, int limite, CancellationToken ct)
    {
        var query = _ctx.Empresas
            .Where(e => e.Id == empresaId && !e.IsDeleted)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtros))
        {
            var pares = ParseFiltros(filtros);
            foreach (var (campo, valor) in pares)
            {
                query = campo.ToLowerInvariant() switch
                {
                    "razaosocial" or "nome" => query.Where(e => EF.Functions.ILike(e.RazaoSocial, $"%{valor}%")),
                    "cnpj" => query.Where(e => e.Cnpj == valor),
                    _ => query
                };
            }
        }

        var resultados = await query.Take(limite)
            .Select(e => new
            {
                e.Id,
                e.RazaoSocial,
                e.NomeFantasia,
                e.Cnpj,
                Status = e.Status.ToString(),
                Plano = e.Plano.ToString(),
                e.DataVencimento,
                e.MaxUsuarios,
                e.MaxObras
            })
            .ToListAsync(ct);

        return JsonSerializer.Serialize(resultados, _jsonOpts);
    }

    private async Task<string> GerarCodigoUnicoAsync<T>(
        string prefixo, Guid empresaId,
        System.Linq.Expressions.Expression<Func<T, bool>> filtroEmpresa,
        Func<string, System.Linq.Expressions.Expression<Func<T, bool>>> filtroCodigo,
        CancellationToken ct) where T : class
    {
        var count = await _ctx.Set<T>().CountAsync(filtroEmpresa, ct);
        var seq = count + 1;
        string codigo;
        do
        {
            codigo = $"{prefixo}{seq:D4}";
            seq++;
        }
        while (await _ctx.Set<T>().AnyAsync(filtroCodigo(codigo), ct));
        return codigo;
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
                name = "buscar",
                description = "Busca um registro específico por ID em um módulo do Constriva, retornando todos os campos",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        modulo = new { type = "string", @enum = new[] { "clientes", "compras", "contratos", "estoque", "financeiro", "fornecedores", "obras", "rh" }, description = "Módulo do sistema" },
                        id = new { type = "string", description = "ID (GUID) do registro" }
                    },
                    required = new[] { "modulo", "id" }
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
                        ["operacao"] = new
                        {
                            type = "string",
                            @enum = new[] { "criar", "atualizar" },
                            description = "Tipo de operação: criar novo registro ou atualizar existente"
                        },
                        ["dados"] = new
                        {
                            type = "object",
                            description = "Dados do registro com os campos a preencher (ex: {\"nome\": \"...\", \"valor\": 100})"
                        },
                        ["id"] = new
                        {
                            type = "string",
                            description = "ID do registro para atualização (obrigatório quando operacao=atualizar)"
                        }
                    },
                    required = new[] { "modulo", "operacao", "dados" }
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
        },
        new
        {
            type = "function",
            function = new
            {
                name = "excluir",
                description = "Exclui (soft delete) um registro de um módulo do Constriva",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        modulo = new { type = "string", @enum = new[] { "clientes", "compras", "contratos", "estoque", "financeiro", "fornecedores", "obras", "rh" }, description = "Módulo do sistema" },
                        id = new { type = "string", description = "ID (GUID) do registro a excluir" }
                    },
                    required = new[] { "modulo", "id" }
                }
            }
        }
    }.AsReadOnly();

    public static IReadOnlyList<object> Get() => _definitions;
}
