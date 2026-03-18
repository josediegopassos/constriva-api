using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Entities.Obras.Diario;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Entities.Orcamento;
using OrcamentoEntity = Constriva.Domain.Entities.Orcamento.Orcamento;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Entities.GED;
using Constriva.Application.Common.Interfaces;
using MediatR;

namespace Constriva.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ICurrentUser? _currentUser;
    private readonly IMediator? _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUser? currentUser = null, IMediator? mediator = null)
        : base(options)
    {
        _currentUser = currentUser;
        _mediator = mediator;
    }

    // ── Tenant ────────────────────────────────────────────────────────────
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<HistoricoPlano> HistoricoPlanos => Set<HistoricoPlano>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioPermissao> UsuariosPermissoes => Set<UsuarioPermissao>();
    public DbSet<UsuarioObra> UsuariosObras => Set<UsuarioObra>();
    public DbSet<LogAcesso> LogsAcesso => Set<LogAcesso>();

    // ── Clientes ─────────────────────────────────────────────────────────
    public DbSet<Cliente> Clientes => Set<Cliente>();

    // ── Obras ────────────────────────────────────────────────────────────
    public DbSet<Obra> Obras => Set<Obra>();
    public DbSet<FaseObra> FasesObra => Set<FaseObra>();
    public DbSet<ObraAnexo> ObrasAnexos => Set<ObraAnexo>();
    public DbSet<ObraHistorico> ObrasHistoricos => Set<ObraHistorico>();
    public DbSet<RDO> RDOs => Set<RDO>();
    public DbSet<RDOEquipe> RDOsEquipes => Set<RDOEquipe>();
    public DbSet<RDOEquipamento> RDOsEquipamentos => Set<RDOEquipamento>();
    public DbSet<RDOOcorrencia> RDOsOcorrencias => Set<RDOOcorrencia>();

    // ── Estoque ───────────────────────────────────────────────────────────
    public DbSet<GrupoMaterial> GruposMateriais => Set<GrupoMaterial>();
    public DbSet<Material> Materiais => Set<Material>();
    public DbSet<Almoxarifado> Almoxarifados => Set<Almoxarifado>();
    public DbSet<EstoqueSaldo> EstoquesSaldos => Set<EstoqueSaldo>();
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();
    public DbSet<RequisicaoMaterial> RequisicoesMateriis => Set<RequisicaoMaterial>();
    public DbSet<ItemRequisicao> ItensRequisicao => Set<ItemRequisicao>();
    public DbSet<InventarioEstoque> InventariosEstoque => Set<InventarioEstoque>();
    public DbSet<ItemInventario> ItensInventario => Set<ItemInventario>();

    // ── Cronograma ────────────────────────────────────────────────────────
    public DbSet<CronogramaObra> Cronogramas => Set<CronogramaObra>();
    public DbSet<AtividadeCronograma> AtividadesCronograma => Set<AtividadeCronograma>();
    public DbSet<VinculoAtividade> VinculosAtividades => Set<VinculoAtividade>();
    public DbSet<RecursoAtividade> RecursosAtividades => Set<RecursoAtividade>();
    public DbSet<CurvaSPonto> CurvaSPontos => Set<CurvaSPonto>();
    public DbSet<ProgressoAtividade> ProgressoAtividades => Set<ProgressoAtividade>();

    // ── Orçamento ─────────────────────────────────────────────────────────
    public DbSet<OrcamentoEntity> Orcamentos => Set<OrcamentoEntity>();
    public DbSet<GrupoOrcamento> GruposOrcamento => Set<GrupoOrcamento>();
    public DbSet<ItemOrcamento> ItensOrcamento => Set<ItemOrcamento>();
    public DbSet<ComposicaoOrcamento> ComposicoesOrcamento => Set<ComposicaoOrcamento>();
    public DbSet<InsumoComposicao> InsumosComposicao => Set<InsumoComposicao>();
    public DbSet<OrcamentoHistorico> OrcamentosHistoricos => Set<OrcamentoHistorico>();
    public DbSet<MedicaoContrato> MedicoesContrato => Set<MedicaoContrato>();
    public DbSet<ItemMedicao> ItensMedicao => Set<ItemMedicao>();

    // ── Compras ───────────────────────────────────────────────────────────
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Cotacao> Cotacoes => Set<Cotacao>();
    public DbSet<ItemCotacao> ItensCotacao => Set<ItemCotacao>();
    public DbSet<PropostaCotacao> PropostasCotacao => Set<PropostaCotacao>();
    public DbSet<ItemPropostaCotacao> ItensPropostaCotacao => Set<ItemPropostaCotacao>();
    public DbSet<PedidoCompra> PedidosCompra => Set<PedidoCompra>();
    public DbSet<ItemPedidoCompra> ItensPedidoCompra => Set<ItemPedidoCompra>();
    public DbSet<RecebimentoCompra> RecebimentosCompra => Set<RecebimentoCompra>();
    public DbSet<ItemRecebimento> ItensRecebimento => Set<ItemRecebimento>();

    // ── Contratos ─────────────────────────────────────────────────────────
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<AditvoContrato> AditivosContrato => Set<AditvoContrato>();
    public DbSet<MedicaoContratual> MedicoesContratuais => Set<MedicaoContratual>();
    public DbSet<ItemMedicaoContratual> ItensMedicaoContratual => Set<ItemMedicaoContratual>();
    public DbSet<FaturaContrato> FaturasContrato => Set<FaturaContrato>();
    public DbSet<ContratoAnexo> ContratosAnexos => Set<ContratoAnexo>();

    // ── RH ────────────────────────────────────────────────────────────────
    public DbSet<Cargo> Cargos => Set<Cargo>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<RegistroPonto> RegistrosPonto => Set<RegistroPonto>();
    public DbSet<ApuracaoPonto> ApuracoesPonto => Set<ApuracaoPonto>();
    public DbSet<FolhaPagamento> FolhasPagamento => Set<FolhaPagamento>();
    public DbSet<FolhaFuncionario> FolhasFuncionarios => Set<FolhaFuncionario>();
    public DbSet<Afastamento> Afastamentos => Set<Afastamento>();
    public DbSet<ExameMedico> ExamesMedicos => Set<ExameMedico>();
    public DbSet<TreinamentoFuncionario> TreinamentosFuncionarios => Set<TreinamentoFuncionario>();
    public DbSet<EPIFuncionario> EPIsFuncionarios => Set<EPIFuncionario>();
    public DbSet<DocumentoFuncionario> DocumentosFuncionarios => Set<DocumentoFuncionario>();

    // ── Financeiro ────────────────────────────────────────────────────────
    public DbSet<CentroCusto> CentrosCusto => Set<CentroCusto>();
    public DbSet<PlanoContas> PlanosContas => Set<PlanoContas>();
    public DbSet<ContaBancaria> ContasBancarias => Set<ContaBancaria>();
    public DbSet<LancamentoFinanceiro> LancamentosFinanceiros => Set<LancamentoFinanceiro>();
    public DbSet<AnexoLancamento> AnexosLancamentos => Set<AnexoLancamento>();
    public DbSet<Transferencia> Transferencias => Set<Transferencia>();
    public DbSet<OrcamentoDRE> OrcamentosDRE => Set<OrcamentoDRE>();

    // ── Qualidade ─────────────────────────────────────────────────────────
    public DbSet<ChecklistModelo> ChecklistsModelos => Set<ChecklistModelo>();
    public DbSet<ChecklistModeloItem> ChecklistsModelosItens => Set<ChecklistModeloItem>();
    public DbSet<Inspecao> Inspecoes => Set<Inspecao>();
    public DbSet<ItemInspecao> ItensInspecao => Set<ItemInspecao>();
    public DbSet<FotoInspecao> FotosInspecao => Set<FotoInspecao>();
    public DbSet<NaoConformidade> NaoConformidades => Set<NaoConformidade>();
    public DbSet<AcaoNC> AcoesNC => Set<AcaoNC>();
    public DbSet<EnsaioMaterial> EnsaiosMateriais => Set<EnsaioMaterial>();
    public DbSet<FVS> FVSs => Set<FVS>();
    public DbSet<ItemFVS> ItensFVS => Set<ItemFVS>();

    // ── SST ───────────────────────────────────────────────────────────────
    public DbSet<DDS> DDSs => Set<DDS>();
    public DbSet<ParticipateDDS> ParticipantesDDS => Set<ParticipateDDS>();
    public DbSet<PermissaoTrabalho> PermissoesTrabalho => Set<PermissaoTrabalho>();
    public DbSet<ItemCheclistPT> ItensChecklistPT => Set<ItemCheclistPT>();
    public DbSet<RegistroAcidente> RegistrosAcidentes => Set<RegistroAcidente>();
    public DbSet<TestemunhaAcidente> TestemunhasAcidente => Set<TestemunhaAcidente>();
    public DbSet<AcaoCorretivaSst> AcoesCorretivasSst => Set<AcaoCorretivaSst>();
    public DbSet<EPI> EPIs => Set<EPI>();
    public DbSet<DocumentoPCM> DocumentosPCM => Set<DocumentoPCM>();
    public DbSet<IndicadorSST> IndicadoresSST => Set<IndicadorSST>();

    // ── GED ───────────────────────────────────────────────────────────────
    public DbSet<PastaDocumento> PastasDocumentos => Set<PastaDocumento>();
    public DbSet<DocumentoGED> DocumentosGED => Set<DocumentoGED>();
    public DbSet<ArquivoDocumento> ArquivosDocumentos => Set<ArquivoDocumento>();
    public DbSet<RevisaoDocumento> RevisoesDocumentos => Set<RevisaoDocumento>();
    public DbSet<FluxoAprovacaoDoc> FluxosAprovacaoDoc => Set<FluxoAprovacaoDoc>();
    public DbSet<DistribuicaoDoc> DistribuicoesDoc => Set<DistribuicaoDoc>();
    public DbSet<AcessoDocumento> AcessosDocumentos => Set<AcessoDocumento>();
    public DbSet<Transmittal> Transmittals => Set<Transmittal>();
    public DbSet<DocumentoTransmittal> DocumentosTransmittals => Set<DocumentoTransmittal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global query filters for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext).GetMethod(nameof(SetSoftDeleteFilter),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    private static void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : BaseEntity
        => modelBuilder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var userId = _currentUser?.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }

        var result = await base.SaveChangesAsync(ct);

        // Dispatch domain events
        if (_mediator != null)
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity).ToList();

            foreach (var entity in entities)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                foreach (var domainEvent in events)
                    await _mediator.Publish(domainEvent, ct);
            }
        }

        return result;
    }
}
