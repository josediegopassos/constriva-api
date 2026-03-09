using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;

namespace Constriva.Infrastructure.Persistence;

public static class DbSeeder
{
    // Helper: define Id (protected set) via reflection para uso no seed
    private static T SetId<T>(T entity, Guid id) where T : BaseEntity
    {
        typeof(BaseEntity).GetProperty("Id")!.SetValue(entity, id);
        return entity;
    }

    // ── IDs fixos para idempotência ────────────────────────────────────────────
    private static readonly Guid IdEmpresa      = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid IdSuperAdmin   = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid IdAdminEmpresa = Guid.Parse("20000000-0000-0000-0000-000000000002");

    // Plano de Contas
    private static readonly Guid IdPlanoReceitas         = Guid.Parse("30000000-0000-0000-0000-000000000001");
    private static readonly Guid IdPlanoReceitaObras     = Guid.Parse("30000000-0000-0000-0000-000000000002");
    private static readonly Guid IdPlanoReceitaServicos  = Guid.Parse("30000000-0000-0000-0000-000000000003");
    private static readonly Guid IdPlanoDespesas         = Guid.Parse("30000000-0000-0000-0000-000000000004");
    private static readonly Guid IdPlanoDespMateriais    = Guid.Parse("30000000-0000-0000-0000-000000000005");
    private static readonly Guid IdPlanoDespMaoDeObra    = Guid.Parse("30000000-0000-0000-0000-000000000006");
    private static readonly Guid IdPlanoDespEquipamentos = Guid.Parse("30000000-0000-0000-0000-000000000007");
    private static readonly Guid IdPlanoDespAdmin        = Guid.Parse("30000000-0000-0000-0000-000000000008");
    private static readonly Guid IdPlanoDespImpostos     = Guid.Parse("30000000-0000-0000-0000-000000000009");

    // Centros de Custo
    private static readonly Guid IdCcAdministrativo = Guid.Parse("40000000-0000-0000-0000-000000000001");
    private static readonly Guid IdCcObras          = Guid.Parse("40000000-0000-0000-0000-000000000002");
    private static readonly Guid IdCcCompras        = Guid.Parse("40000000-0000-0000-0000-000000000003");
    private static readonly Guid IdCcFinanceiro     = Guid.Parse("40000000-0000-0000-0000-000000000004");

    // Conta Bancária
    private static readonly Guid IdContaPrincipal = Guid.Parse("50000000-0000-0000-0000-000000000001");

    // Grupos de Materiais
    private static readonly Guid IdGrupoConstrucaoCivil = Guid.Parse("60000000-0000-0000-0000-000000000001");
    private static readonly Guid IdGrupoConcreto        = Guid.Parse("60000000-0000-0000-0000-000000000002");
    private static readonly Guid IdGrupoAco             = Guid.Parse("60000000-0000-0000-0000-000000000003");
    private static readonly Guid IdGrupoAlvenaria       = Guid.Parse("60000000-0000-0000-0000-000000000004");
    private static readonly Guid IdGrupoAcabamentos     = Guid.Parse("60000000-0000-0000-0000-000000000005");
    private static readonly Guid IdGrupoEletrico        = Guid.Parse("60000000-0000-0000-0000-000000000006");
    private static readonly Guid IdGrupoHidraulico      = Guid.Parse("60000000-0000-0000-0000-000000000007");
    private static readonly Guid IdGrupoFerramentas     = Guid.Parse("60000000-0000-0000-0000-000000000008");
    private static readonly Guid IdGrupoEPI             = Guid.Parse("60000000-0000-0000-0000-000000000009");

    // Departamentos
    private static readonly Guid IdDeptEngenharia = Guid.Parse("70000000-0000-0000-0000-000000000001");
    private static readonly Guid IdDeptObras      = Guid.Parse("70000000-0000-0000-0000-000000000002");
    private static readonly Guid IdDeptAdmin      = Guid.Parse("70000000-0000-0000-0000-000000000003");
    private static readonly Guid IdDeptCompras    = Guid.Parse("70000000-0000-0000-0000-000000000004");
    private static readonly Guid IdDeptFinanceiro = Guid.Parse("70000000-0000-0000-0000-000000000005");
    private static readonly Guid IdDeptRH         = Guid.Parse("70000000-0000-0000-0000-000000000006");
    private static readonly Guid IdDeptQualidade  = Guid.Parse("70000000-0000-0000-0000-000000000007");
    private static readonly Guid IdDeptSST        = Guid.Parse("70000000-0000-0000-0000-000000000008");

    // Cargos
    private static readonly Guid IdCargoDiretor    = Guid.Parse("80000000-0000-0000-0000-000000000001");
    private static readonly Guid IdCargoGerente    = Guid.Parse("80000000-0000-0000-0000-000000000002");
    private static readonly Guid IdCargoEngenheiro = Guid.Parse("80000000-0000-0000-0000-000000000003");
    private static readonly Guid IdCargoTecnico    = Guid.Parse("80000000-0000-0000-0000-000000000004");
    private static readonly Guid IdCargoMestre     = Guid.Parse("80000000-0000-0000-0000-000000000005");
    private static readonly Guid IdCargoPedreiro   = Guid.Parse("80000000-0000-0000-0000-000000000006");
    private static readonly Guid IdCargoServente   = Guid.Parse("80000000-0000-0000-0000-000000000007");
    private static readonly Guid IdCargoArmador    = Guid.Parse("80000000-0000-0000-0000-000000000008");
    private static readonly Guid IdCargoEletricista= Guid.Parse("80000000-0000-0000-0000-000000000009");
    private static readonly Guid IdCargoEncanador  = Guid.Parse("80000000-0000-0000-0000-000000000010");
    private static readonly Guid IdCargoAlmoxarife = Guid.Parse("80000000-0000-0000-0000-000000000011");
    private static readonly Guid IdCargoSST        = Guid.Parse("80000000-0000-0000-0000-000000000012");

    // EPIs
    private static readonly Guid IdEpiCapacete    = Guid.Parse("90000000-0000-0000-0000-000000000001");
    private static readonly Guid IdEpiOculos      = Guid.Parse("90000000-0000-0000-0000-000000000002");
    private static readonly Guid IdEpiProtetorAud = Guid.Parse("90000000-0000-0000-0000-000000000003");
    private static readonly Guid IdEpiLuvas       = Guid.Parse("90000000-0000-0000-0000-000000000004");
    private static readonly Guid IdEpiBota        = Guid.Parse("90000000-0000-0000-0000-000000000005");
    private static readonly Guid IdEpiCinto       = Guid.Parse("90000000-0000-0000-0000-000000000006");
    private static readonly Guid IdEpiColete      = Guid.Parse("90000000-0000-0000-0000-000000000007");
    private static readonly Guid IdEpiMascara     = Guid.Parse("90000000-0000-0000-0000-000000000008");

    // ── Entry point ───────────────────────────────────────────────────────────
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var ctx    = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            await ctx.Database.MigrateAsync();

            if (await ctx.Usuarios.AnyAsync(u => u.IsSuperAdmin))
            {
                logger.LogInformation("Banco já possui dados. Seed ignorado.");
                return;
            }

            logger.LogInformation("Iniciando seed do banco de dados...");

            await SeedSuperAdmin(ctx);
            await SeedEmpresa(ctx);
            await SeedAdminEmpresa(ctx);
            await SeedPlanoContas(ctx);
            await SeedCentrosCusto(ctx);
            await SeedContaBancaria(ctx);
            await SeedGruposMateriais(ctx);
            await SeedDepartamentos(ctx);
            await SeedCargos(ctx);
            await SeedEPIs(ctx);

            await ctx.SaveChangesAsync();
            logger.LogInformation("Seed concluído com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante o seed do banco de dados.");
            throw;
        }
    }

    // ── SuperAdmin da plataforma ───────────────────────────────────────────────
    private static async Task SeedSuperAdmin(AppDbContext ctx)
    {
        if (await ctx.Usuarios.AnyAsync(u => u.Id == IdSuperAdmin)) return;

        ctx.Usuarios.Add(SetId(new Usuario
        {
            EmpresaId    = null,
            Nome         = "Super Administrador",
            Email        = "superadmin@constriva.com.br",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Constriva@2025!"),
            Perfil       = PerfilUsuarioEnum.SuperAdmin,
            IsSuperAdmin = true,
            Ativo        = true,
            CreatedAt    = DateTime.UtcNow
        }, IdSuperAdmin));
    }

    // ── Empresa padrão ─────────────────────────────────────────────────────────
    private static async Task SeedEmpresa(AppDbContext ctx)
    {
        if (await ctx.Empresas.AnyAsync(e => e.Id == IdEmpresa)) return;

        ctx.Empresas.Add(SetId(new Empresa
        {
            RazaoSocial      = "Constriva Tecnologia Ltda",
            NomeFantasia     = "Constriva",
            Cnpj             = "00000000000191",
            Email            = "contato@constriva.com.br",
            Telefone         = "(11) 3000-0000",
            Logradouro       = "Av. Paulista",
            Numero           = "1000",
            Bairro           = "Bela Vista",
            Cidade           = "São Paulo",
            Estado           = "SP",
            Cep              = "01310-100",
            Status           = StatusEmpresaEnum.Ativa,
            Plano            = PlanoEmpresaEnum.Enterprise,
            MaxUsuarios      = 999,
            MaxObras         = 999,
            MaxStorageMb     = 102400,
            PrimeiroAcesso   = false,
            ModuloObras      = true,
            ModuloEstoque    = true,
            ModuloCronograma = true,
            ModuloOrcamento  = true,
            ModuloCompras    = true,
            ModuloQualidade  = true,
            ModuloContratos  = true,
            ModuloRH         = true,
            ModuloFinanceiro = true,
            ModuloSST        = true,
            ModuloGED        = true,
            ModuloRelatorios = true,
            CreatedAt        = DateTime.UtcNow
        }, IdEmpresa));
    }

    // ── Admin da empresa ───────────────────────────────────────────────────────
    private static async Task SeedAdminEmpresa(AppDbContext ctx)
    {
        if (await ctx.Usuarios.AnyAsync(u => u.Id == IdAdminEmpresa)) return;

        ctx.Usuarios.Add(SetId(new Usuario
        {
            EmpresaId      = IdEmpresa,
            Nome           = "Administrador",
            Email          = "admin@constriva.com.br",
            PasswordHash   = BCrypt.Net.BCrypt.HashPassword("Constriva@2025!"),
            Perfil         = PerfilUsuarioEnum.AdminEmpresa,
            IsAdminEmpresa = true,
            Ativo          = true,
            CreatedAt      = DateTime.UtcNow
        }, IdAdminEmpresa));
    }

    // ── Plano de Contas ────────────────────────────────────────────────────────
    private static async Task SeedPlanoContas(AppDbContext ctx)
    {
        if (await ctx.PlanosContas.AnyAsync(p => p.EmpresaId == IdEmpresa)) return;

        await ctx.PlanosContas.AddRangeAsync(new List<PlanoContas>
        {
            // Receitas (sintéticas)
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "1",   Nome = "RECEITAS",                   Tipo = TipoLancamentoEnum.Receita, Sintetica = true,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdPlanoReceitas),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "1.1", Nome = "Receita de Obras",           Tipo = TipoLancamentoEnum.Receita, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoReceitas, CreatedAt = DateTime.UtcNow }, IdPlanoReceitaObras),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "1.2", Nome = "Receita de Serviços",        Tipo = TipoLancamentoEnum.Receita, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoReceitas, CreatedAt = DateTime.UtcNow }, IdPlanoReceitaServicos),

            // Despesas (sintéticas)
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2",   Nome = "DESPESAS",                   Tipo = TipoLancamentoEnum.Despesa, Sintetica = true,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdPlanoDespesas),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2.1", Nome = "Materiais de Construção",    Tipo = TipoLancamentoEnum.Despesa, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoDespesas, CreatedAt = DateTime.UtcNow }, IdPlanoDespMateriais),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2.2", Nome = "Mão de Obra",                Tipo = TipoLancamentoEnum.Despesa, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoDespesas, CreatedAt = DateTime.UtcNow }, IdPlanoDespMaoDeObra),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2.3", Nome = "Equipamentos e Ferramentas", Tipo = TipoLancamentoEnum.Despesa, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoDespesas, CreatedAt = DateTime.UtcNow }, IdPlanoDespEquipamentos),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2.4", Nome = "Despesas Administrativas",   Tipo = TipoLancamentoEnum.Despesa, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoDespesas, CreatedAt = DateTime.UtcNow }, IdPlanoDespAdmin),
            SetId(new PlanoContas { EmpresaId = IdEmpresa, Codigo = "2.5", Nome = "Impostos e Taxas",           Tipo = TipoLancamentoEnum.Despesa, Sintetica = false, Ativo = true, ContaPaiId = IdPlanoDespesas, CreatedAt = DateTime.UtcNow }, IdPlanoDespImpostos),
        });
    }

    // ── Centros de Custo ───────────────────────────────────────────────────────
    private static async Task SeedCentrosCusto(AppDbContext ctx)
    {
        if (await ctx.CentrosCusto.AnyAsync(c => c.EmpresaId == IdEmpresa)) return;

        await ctx.CentrosCusto.AddRangeAsync(new List<CentroCusto>
        {
            SetId(new CentroCusto { EmpresaId = IdEmpresa, Codigo = "ADM", Nome = "Administrativo",        Tipo = TipoCentroCustoEnum.Administrativo, Ativo = true, CreatedAt = DateTime.UtcNow }, IdCcAdministrativo),
            SetId(new CentroCusto { EmpresaId = IdEmpresa, Codigo = "OBR", Nome = "Obras",                 Tipo = TipoCentroCustoEnum.Obra,            Ativo = true, CreatedAt = DateTime.UtcNow }, IdCcObras),
            SetId(new CentroCusto { EmpresaId = IdEmpresa, Codigo = "COM", Nome = "Compras e Suprimentos", Tipo = TipoCentroCustoEnum.Comercial,       Ativo = true, CreatedAt = DateTime.UtcNow }, IdCcCompras),
            SetId(new CentroCusto { EmpresaId = IdEmpresa, Codigo = "FIN", Nome = "Financeiro",            Tipo = TipoCentroCustoEnum.Financeiro,      Ativo = true, CreatedAt = DateTime.UtcNow }, IdCcFinanceiro),
        });
    }

    // ── Conta Bancária ─────────────────────────────────────────────────────────
    private static async Task SeedContaBancaria(AppDbContext ctx)
    {
        if (await ctx.ContasBancarias.AnyAsync(c => c.EmpresaId == IdEmpresa)) return;

        ctx.ContasBancarias.Add(SetId(new ContaBancaria
        {
            EmpresaId   = IdEmpresa,
            Nome        = "Conta Principal",
            BancoNome   = "Banco do Brasil",
            BancoCodigo = "001",
            Agencia     = "0001",
            Conta       = "000001-0",
            TipoConta   = "Corrente",
            SaldoInicial= 0,
            SaldoAtual  = 0,
            Ativo       = true,
            CreatedAt   = DateTime.UtcNow
        }, IdContaPrincipal));
    }

    // ── Grupos de Materiais ────────────────────────────────────────────────────
    private static async Task SeedGruposMateriais(AppDbContext ctx)
    {
        if (await ctx.GruposMateriais.AnyAsync(g => g.EmpresaId == IdEmpresa)) return;

        await ctx.GruposMateriais.AddRangeAsync(new List<GrupoMaterial>
        {
            // Raízes
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Construção Civil",          CreatedAt = DateTime.UtcNow }, IdGrupoConstrucaoCivil),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Material Elétrico",          CreatedAt = DateTime.UtcNow }, IdGrupoEletrico),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Material Hidráulico",        CreatedAt = DateTime.UtcNow }, IdGrupoHidraulico),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Ferramentas e Equipamentos", CreatedAt = DateTime.UtcNow }, IdGrupoFerramentas),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "EPIs",                       CreatedAt = DateTime.UtcNow }, IdGrupoEPI),

            // Subgrupos de Construção Civil
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Concreto e Argamassa",     GrupoPaiId = IdGrupoConstrucaoCivil, CreatedAt = DateTime.UtcNow }, IdGrupoConcreto),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Aço e Ferragem",           GrupoPaiId = IdGrupoConstrucaoCivil, CreatedAt = DateTime.UtcNow }, IdGrupoAco),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Alvenaria e Revestimento", GrupoPaiId = IdGrupoConstrucaoCivil, CreatedAt = DateTime.UtcNow }, IdGrupoAlvenaria),
            SetId(new GrupoMaterial { EmpresaId = IdEmpresa, Nome = "Acabamentos",              GrupoPaiId = IdGrupoConstrucaoCivil, CreatedAt = DateTime.UtcNow }, IdGrupoAcabamentos),
        });
    }

    // ── Departamentos ──────────────────────────────────────────────────────────
    private static async Task SeedDepartamentos(AppDbContext ctx)
    {
        if (await ctx.Departamentos.AnyAsync(d => d.EmpresaId == IdEmpresa)) return;

        await ctx.Departamentos.AddRangeAsync(new List<Departamento>
        {
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Engenharia",            Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptEngenharia),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Obras",                 Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptObras),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Administrativo",        Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptAdmin),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Compras e Suprimentos", Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptCompras),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Financeiro",            Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptFinanceiro),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Recursos Humanos",      Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptRH),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Qualidade",             Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptQualidade),
            SetId(new Departamento { EmpresaId = IdEmpresa, Nome = "Segurança do Trabalho", Ativo = true, CreatedAt = DateTime.UtcNow }, IdDeptSST),
        });
    }

    // ── Cargos ─────────────────────────────────────────────────────────────────
    private static async Task SeedCargos(AppDbContext ctx)
    {
        if (await ctx.Cargos.AnyAsync(c => c.EmpresaId == IdEmpresa)) return;

        await ctx.Cargos.AddRangeAsync(new List<Cargo>
        {
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "DIR", Nome = "Diretor Técnico",                  Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoDiretor),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "GER", Nome = "Gerente de Obras",                 Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoGerente),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "ENG", Nome = "Engenheiro Civil",                 Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoEngenheiro),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "TEC", Nome = "Técnico em Edificações",           Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoTecnico),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "MES", Nome = "Mestre de Obras",                  Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoMestre),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "PED", Nome = "Pedreiro",                         Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoPedreiro),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "SER", Nome = "Servente de Obras",                Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoServente),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "ARM", Nome = "Armador",                          Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoArmador),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "ELE", Nome = "Eletricista",                      Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoEletricista),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "ENC", Nome = "Encanador",                        Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoEncanador),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "ALM", Nome = "Almoxarife",                       Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoAlmoxarife),
            SetId(new Cargo { EmpresaId = IdEmpresa, Codigo = "SST", Nome = "Técnico de Segurança do Trabalho", Ativo = true, CreatedAt = DateTime.UtcNow }, IdCargoSST),
        });
    }

    // ── EPIs ───────────────────────────────────────────────────────────────────
    private static async Task SeedEPIs(AppDbContext ctx)
    {
        if (await ctx.EPIs.AnyAsync(e => e.EmpresaId == IdEmpresa)) return;

        await ctx.EPIs.AddRangeAsync(new List<EPI>
        {
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-001", Nome = "Capacete de Segurança", Tipo = TipoEPIEnum.Cabeca,       Descricao = "Capacete classe A/B",              VidaUtilMeses = 60,  EstoqueMinimo = 5,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiCapacete),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-002", Nome = "Óculos de Proteção",    Tipo = TipoEPIEnum.Olhos,        Descricao = "Óculos ampla visão",               VidaUtilMeses = 12,  EstoqueMinimo = 5,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiOculos),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-003", Nome = "Protetor Auditivo",     Tipo = TipoEPIEnum.Audicao,      Descricao = "Protetor tipo concha ou inserção", VidaUtilMeses = 6,   EstoqueMinimo = 5,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiProtetorAud),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-004", Nome = "Luvas de Proteção",     Tipo = TipoEPIEnum.Maos,         Descricao = "Luvas de raspa/nitrílica",         VidaUtilMeses = 3,   EstoqueMinimo = 10, Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiLuvas),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-005", Nome = "Bota de Segurança",     Tipo = TipoEPIEnum.Pes,          Descricao = "Bota com biqueira de aço",         VidaUtilMeses = 12,  EstoqueMinimo = 3,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiBota),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-006", Nome = "Cinto de Segurança",    Tipo = TipoEPIEnum.QuedaAltura,  Descricao = "Cinto paraquedista com talabarte", VidaUtilMeses = 60,  EstoqueMinimo = 2,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiCinto),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-007", Nome = "Colete Refletivo",      Tipo = TipoEPIEnum.Corpo,        Descricao = "Colete de alta visibilidade",      VidaUtilMeses = 12,  EstoqueMinimo = 5,  Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiColete),
            SetId(new EPI { EmpresaId = IdEmpresa, Codigo = "EPI-008", Nome = "Máscara de Proteção",   Tipo = TipoEPIEnum.Respiratorio, Descricao = "Respirador semifacial PFF2/N95",   VidaUtilMeses = 1,   EstoqueMinimo = 20, Ativo = true, CreatedAt = DateTime.UtcNow }, IdEpiMascara),
        });
    }
}
