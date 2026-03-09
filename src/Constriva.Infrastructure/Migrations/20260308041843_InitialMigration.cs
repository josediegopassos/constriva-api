using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Almoxarifados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Principal = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almoxarifados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CBO = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SalarioBase = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SalarioMaximo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentrosCusto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    CentroPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosCusto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentrosCusto_CentrosCusto_CentroPaiId",
                        column: x => x.CentroPaiId,
                        principalTable: "CentrosCusto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistModelos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EtapaConstrucao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistModelos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContasBancarias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BancoNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BancoCodigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Agencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Conta = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TipoConta = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoAtual = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    PixChave = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasBancarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cotacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLimiteResposta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CondicoesGerais = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CriadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorVencedorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CronogramasObra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ELinhaDBase = table.Column<bool>(type: "boolean", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Versao = table.Column<int>(type: "integer", nullable: false),
                    VersaoBaseadaEm = table.Column<Guid>(type: "uuid", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CronogramasObra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DDS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataRealizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tema = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Conteudo = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Ministrador = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DuracaoMinutos = table.Column<decimal>(type: "numeric(5,0)", precision: 5, scale: 0, nullable: false),
                    Local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    NumeroParticipantes = table.Column<int>(type: "integer", nullable: false),
                    RegistradoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DDS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    GestorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartamentoPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamentos_Departamentos_DepartamentoPaiId",
                        column: x => x.DepartamentoPaiId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosPCM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NumeroRevisao = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DataElaboracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVigencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Elaborador = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AprovadoPor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ArquivoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosPCM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    InscricaoEstadual = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Site = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Plano = table.Column<int>(type: "integer", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxUsuarios = table.Column<int>(type: "integer", nullable: false),
                    MaxObras = table.Column<int>(type: "integer", nullable: false),
                    MaxStorageMb = table.Column<int>(type: "integer", nullable: false),
                    PrimeiroAcesso = table.Column<bool>(type: "boolean", nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ModuloObras = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloEstoque = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloCronograma = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloOrcamento = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloCompras = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloQualidade = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloContratos = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloRH = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloFinanceiro = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloSST = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloGED = table.Column<bool>(type: "boolean", nullable: false),
                    ModuloRelatorios = table.Column<bool>(type: "boolean", nullable: false),
                    ConfiguracoesJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnsaiosMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataColeta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataEnsaio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Laboratorio = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    NormaReferencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Resultado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Aprovado = table.Column<bool>(type: "boolean", nullable: false),
                    ValorObtido = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    ValorMinimo = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    ValorMaximo = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    Laudo = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LaudoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    LocalColeta = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnsaiosMaterial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EPIs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Fabricante = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NumeroCA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ValidadeCA = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NormaReferencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EstoqueAtual = table.Column<int>(type: "integer", nullable: false),
                    EstoqueMinimo = table.Column<int>(type: "integer", nullable: false),
                    VidaUtilMeses = table.Column<decimal>(type: "numeric(5,1)", precision: 5, scale: 1, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ImagemUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolhasPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Competencia = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ValorTotalBruto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorTotalDescontos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorTotalLiquido = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalFuncionarios = table.Column<int>(type: "integer", nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolhasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoPessoaEnum = table.Column<int>(type: "integer", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Documento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    InscricaoEstadual = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Site = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Contato = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Homologado = table.Column<bool>(type: "boolean", nullable: false),
                    Classificacao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Prazo = table.Column<int>(type: "integer", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    BancoNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BancoAgencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BancoConta = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PixChave = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FVS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Servico = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    EtapaConstrucao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pavimento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Area = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataVerificacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Aprovado = table.Column<bool>(type: "boolean", nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    FiscalId = table.Column<Guid>(type: "uuid", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AssinaturaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FVS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GruposMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    GrupoPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposMaterial_GruposMaterial_GrupoPaiId",
                        column: x => x.GrupoPaiId,
                        principalTable: "GruposMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndicadoresSST",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Competencia = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    TotalFuncionarios = table.Column<int>(type: "integer", nullable: false),
                    TotalHHT = table.Column<int>(type: "integer", nullable: false),
                    AcidentesComAfastamento = table.Column<int>(type: "integer", nullable: false),
                    AcidentesSemAfastamento = table.Column<int>(type: "integer", nullable: false),
                    QuaseAcidentes = table.Column<int>(type: "integer", nullable: false),
                    DiasAfastados = table.Column<int>(type: "integer", nullable: false),
                    NumeroDDS = table.Column<int>(type: "integer", nullable: false),
                    DiasUteis = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresSST", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventariosEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ValorDiferenca = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventariosEstoque", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicoesContrato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Periodo = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ValorMedicao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorAcumulado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualMedicao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PercentualAcumulado = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoesContrato", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TipoContrato = table.Column<int>(type: "integer", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    NomeCliente = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ResponsavelTecnico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreaResponsavel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroART = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroRRT = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroAlvara = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ValidadeAlvara = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AreaTotal = table.Column<double>(type: "double precision", nullable: true),
                    AreaConstruida = table.Column<double>(type: "double precision", nullable: true),
                    NumeroAndares = table.Column<int>(type: "integer", nullable: true),
                    NumeroUnidades = table.Column<int>(type: "integer", nullable: true),
                    DataInicioPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataInicioReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorContrato = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorOrcado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorRealizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualConcluido = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orcamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Versao = table.Column<int>(type: "integer", nullable: false),
                    ELinhaDBase = table.Column<bool>(type: "boolean", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseOrcamentaria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Localidade = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BDI = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Administracao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Seguro = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Risco = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Financeiro = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Lucro = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BDIDetalhado_Tributos = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    EncargosHoristas = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    EncargosMensalistas = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorCustoDirecto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorBDI = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrcamentosDRE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Competencia = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Receitas = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Deducoes = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CustosDiretos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DespesasAdministrativas = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DespesasComerciais = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DespesasFinanceiras = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IRCSLL = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Realizado = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrcamentosDRE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PastasDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    PastaPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Cor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CaminhoCompleto = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AcessoPublico = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastasDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PastasDocumento_PastasDocumento_PastaPaiId",
                        column: x => x.PastaPaiId,
                        principalTable: "PastasDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissoesTrabalho",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoTrabalho = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DescricaoServico = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RiscosIdentificados = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    MedidasControle = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    EPIsNecessarios = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EPCsNecessarios = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ProcedimentosEmergencia = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ExecutanteId = table.Column<Guid>(type: "uuid", nullable: true),
                    NomeExecutante = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EmissorId = table.Column<Guid>(type: "uuid", nullable: true),
                    AprovadorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SupervisorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ObservacoesFechamento = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DataEncerramento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissoesTrabalho", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanoContas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    ContaPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sintetica = table.Column<bool>(type: "boolean", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoContas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoContas_PlanoContas_ContaPaiId",
                        column: x => x.ContaPaiId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosAcidente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    NomeFuncionario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmpresaFuncionario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CargoFuncionario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    DataHoraAcidente = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Local = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DescricaoAcidente = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    PartesCorpoAfetadas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NaturezaLesao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CausaImediata = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CausaBasica = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    MedidasCorretivas = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AfastamentoMedico = table.Column<bool>(type: "boolean", nullable: false),
                    DiasAfastamento = table.Column<int>(type: "integer", nullable: true),
                    NumeroCAT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    DataCAT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TratamentoMedico = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BoPoliciaRegistrado = table.Column<bool>(type: "boolean", nullable: false),
                    NumeroBO = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    InvestigadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataInvestigacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosAcidente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequisicoesMaterial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataRequisicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataNecessidade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SolicitanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AprovadorId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoRejeicao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicoesMaterial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transmittals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Assunto = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DataEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RemetNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DestNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DestEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Respondido = table.Column<bool>(type: "boolean", nullable: false),
                    DataResposta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transmittals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistModeloItens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ModeloId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrupoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Criterio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Obrigatorio = table.Column<bool>(type: "boolean", nullable: false),
                    TipoResposta = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    NormaReferencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistModeloItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistModeloItens_ChecklistModelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "ChecklistModelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspecoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChecklistModeloId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataProgramada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataRealizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Localicacao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ResponsavelInsId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    InspetorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Resultado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TemNaoConformidade = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspecoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspecoes_ChecklistModelos_ChecklistModeloId",
                        column: x => x.ChecklistModeloId,
                        principalTable: "ChecklistModelos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transferencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContaOrigemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContaDestinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DataTransferencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Comprovante = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RealizadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transferencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transferencias_ContasBancarias_ContaDestinoId",
                        column: x => x.ContaDestinoId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transferencias_ContasBancarias_ContaOrigemId",
                        column: x => x.ContaOrigemId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtividadesCronograma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CronogramaId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtividadePaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    EAgrupadoa = table.Column<bool>(type: "boolean", nullable: false),
                    EMarcador = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DuracaoDias = table.Column<int>(type: "integer", nullable: false),
                    PercentualConcluido = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DataInicioPlanejada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFimPlanejada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataInicioReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInicioReprogramada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFimReprogramada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BCWS = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BCWP = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ACWP = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CustoOrcado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CustoRealizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NoCaminhosCritico = table.Column<bool>(type: "boolean", nullable: false),
                    Folga = table.Column<int>(type: "integer", nullable: false),
                    ResponsavelId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Cor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesCronograma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadesCronograma_AtividadesCronograma_AtividadePaiId",
                        column: x => x.AtividadePaiId,
                        principalTable: "AtividadesCronograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AtividadesCronograma_CronogramasObra_CronogramaId",
                        column: x => x.CronogramaId,
                        principalTable: "CronogramasObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurvaSPontos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CronogramaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PercentualPrevisto = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PercentualRealizado = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorPrevisto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorRealizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvaSPontos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurvaSPontos_CronogramasObra_CronogramaId",
                        column: x => x.CronogramaId,
                        principalTable: "CronogramasObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantesDDS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DDSId = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Funcao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Empresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AssinaturaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantesDDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantesDDS_DDS_DDSId",
                        column: x => x.DDSId,
                        principalTable: "DDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Matricula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NomeSocial = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Rg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OrgaoExpedidor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cnh = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CategoriaCnh = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    ValidadeCnh = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ctps = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SeriCtps = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Pis = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EstadoCivil = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Escolaridade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Naturalidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Nacionalidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TipoContratacaoEnum = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CargoId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartamentoId = table.Column<Guid>(type: "uuid", nullable: true),
                    ObraAtualId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAdmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataDemissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoDemissao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SalarioBase = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HoraExtra50 = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    HoraExtra100 = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    JornadaDiaria = table.Column<int>(type: "integer", nullable: false),
                    BancoNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BancoAgencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BancoConta = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PixChave = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoPlanos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanoAnterior = table.Column<int>(type: "integer", nullable: false),
                    PlanoNovo = table.Column<int>(type: "integer", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AlteradoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPlanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPlanos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdminEmpresa = table.Column<bool>(type: "boolean", nullable: false),
                    Perfil = table.Column<int>(type: "integer", nullable: false),
                    UltimoAcesso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmailConfirmado = table.Column<bool>(type: "boolean", nullable: false),
                    TokenConfirmacaoEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TokenRedefinicaoSenha = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TokenRedefinicaoExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TentativasLogin = table.Column<int>(type: "integer", nullable: false),
                    BloqueadoAte = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Idioma = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Objeto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataAssinatura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVigenciaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVigenciaFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataEncerramento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorGlobal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorAditivos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorMedidoAcumulado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorPagoAcumulado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualRetencao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorRetencao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CondicoesPagamento = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DiasParaMedicao = table.Column<int>(type: "integer", nullable: true),
                    DiasParaPagamento = table.Column<int>(type: "integer", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ArquivoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AssinadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    FiscalId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratos_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosCompra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataEntregaPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataEntregaReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CondicoesPagamento = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LocalEntrega = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ValorFrete = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoRejeicao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CriadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropostasCotacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataRecebimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CondicoesPagamento = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PrazoEntrega = table.Column<int>(type: "integer", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Vencedora = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostasCotacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostasCotacao_Cotacoes_CotacaoId",
                        column: x => x.CotacaoId,
                        principalTable: "Cotacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostasCotacao_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensFVS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FVSId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Criterio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Resultado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensFVS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensFVS_FVS_FVSId",
                        column: x => x.FVSId,
                        principalTable: "FVS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materiais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Especificacao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CodigoBarras = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CodigoSINAPI = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fabricante = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    GrupoId = table.Column<Guid>(type: "uuid", nullable: true),
                    EstoqueMinimo = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    EstoqueMaximo = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoCustoMedio = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUltimaCompra = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ImagemUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ControlaLote = table.Column<bool>(type: "boolean", nullable: false),
                    ControlaValidade = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materiais_GruposMaterial_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "GruposMaterial",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItensMedicao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemOrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    QuantidadeContratada = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeAnterior = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeAtual = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensMedicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensMedicao_MedicoesContrato_MedicaoId",
                        column: x => x.MedicaoId,
                        principalTable: "MedicoesContrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FasesObra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    FasePaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataInicioPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFimPrevista = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataInicioReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFimReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PercentualConcluido = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorPrevisto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorRealizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Cor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasesObra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FasesObra_FasesObra_FasePaiId",
                        column: x => x.FasePaiId,
                        principalTable: "FasesObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FasesObra_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObraAnexos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TipoArquivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UsuarioUploadId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObraAnexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObraAnexos_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObraHistoricos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Acao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ValorAnterior = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ValorNovo = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObraHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObraHistoricos_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RDOs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DataRDO = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CondicaoTempo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TempoManha = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TempoTarde = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TempoNoite = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TemperaturaMin = table.Column<decimal>(type: "numeric(5,1)", precision: 5, scale: 1, nullable: false),
                    TemperaturaMax = table.Column<decimal>(type: "numeric(5,1)", precision: 5, scale: 1, nullable: false),
                    AtividadesRealizadas = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    OcorrenciasObservacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PendenciasProblemas = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Fotografias = table.Column<string>(type: "jsonb", nullable: true),
                    ResponsavelNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ResponsavelAssinaturaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AutrId = table.Column<Guid>(type: "uuid", nullable: false),
                    Aprovado = table.Column<bool>(type: "boolean", nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RDOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RDOs_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComposicoesOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fonte = table.Column<int>(type: "integer", nullable: false),
                    CodigoFonte = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CustoTotal = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposicoesOrcamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComposicoesOrcamento_Orcamentos_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GruposOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrupoPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualTotal = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposOrcamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposOrcamento_GruposOrcamento_GrupoPaiId",
                        column: x => x.GrupoPaiId,
                        principalTable: "GruposOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GruposOrcamento_Orcamentos_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrcamentoHistoricos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ValorAnterior = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorNovo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrcamentoHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrcamentoHistoricos_Orcamentos_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosGED",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    PastaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Versao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    NumeroRevisao = table.Column<int>(type: "integer", nullable: false),
                    NormaReferencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Palavraschave = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataVigencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ControleAcesso = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosGED", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosGED_PastasDocumento_PastaId",
                        column: x => x.PastaId,
                        principalTable: "PastasDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensChecklistPT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Item = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Atendido = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensChecklistPT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensChecklistPT_PermissoesTrabalho_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "PermissoesTrabalho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LancamentosFinanceiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    CentroCustoId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContaBancariaId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlanoContaId = table.Column<Guid>(type: "uuid", nullable: true),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: true),
                    PedidoCompraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorRealizado = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCompetencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FormaPagamentoEnum = table.Column<int>(type: "integer", nullable: true),
                    NumeroDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroNF = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Repetir = table.Column<bool>(type: "boolean", nullable: false),
                    Periodicidade = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    QtdParcelas = table.Column<int>(type: "integer", nullable: true),
                    NumeroParcela = table.Column<int>(type: "integer", nullable: true),
                    LancamentoPaiId = table.Column<Guid>(type: "uuid", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ComprovantePagUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StatusAprovacao = table.Column<int>(type: "integer", nullable: false),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentosFinanceiros_CentrosCusto_CentroCustoId",
                        column: x => x.CentroCustoId,
                        principalTable: "CentrosCusto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentosFinanceiros_ContasBancarias_ContaBancariaId",
                        column: x => x.ContaBancariaId,
                        principalTable: "ContasBancarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentosFinanceiros_PlanoContas_PlanoContaId",
                        column: x => x.PlanoContaId,
                        principalTable: "PlanoContas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AcoesCorretivasSST",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcidenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Prazo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Concluida = table.Column<bool>(type: "boolean", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Evidencia = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcoesCorretivasSST", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcoesCorretivasSST_RegistrosAcidente_AcidenteId",
                        column: x => x.AcidenteId,
                        principalTable: "RegistrosAcidente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestemunhasAcidente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcidenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Funcao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Depoimento = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestemunhasAcidente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestemunhasAcidente_RegistrosAcidente_AcidenteId",
                        column: x => x.AcidenteId,
                        principalTable: "RegistrosAcidente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FotosInspecao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InspecaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Legenda = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    DataCaptura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotosInspecao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotosInspecao_Inspecoes_InspecaoId",
                        column: x => x.InspecaoId,
                        principalTable: "Inspecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensInspecao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InspecaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChecklistItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RespostaSimNao = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    RespostaTexto = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RespostaNumero = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    Conforme = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensInspecao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensInspecao_Inspecoes_InspecaoId",
                        column: x => x.InspecaoId,
                        principalTable: "Inspecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaoConformidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspecaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Gravidade = table.Column<int>(type: "integer", nullable: false),
                    LocalizacaoObra = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CausaRaiz = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AcaoCorretiva = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AcaoPreventiva = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DataAbertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrazoConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataEncerramento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    VerificadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    FotoAntesUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FotoDepoisUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CustoNC = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaoConformidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaoConformidades_Inspecoes_InspecaoId",
                        column: x => x.InspecaoId,
                        principalTable: "Inspecoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressosAtividade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtividadeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PercentualAnterior = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PercentualAtual = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RegistradoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    FotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressosAtividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressosAtividade_AtividadesCronograma_AtividadeId",
                        column: x => x.AtividadeId,
                        principalTable: "AtividadesCronograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecursosAtividade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtividadeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoRecurso = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NomeRecurso = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RecursoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustoUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursosAtividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursosAtividade_AtividadesCronograma_AtividadeId",
                        column: x => x.AtividadeId,
                        principalTable: "AtividadesCronograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VinculosAtividade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtividadeOrigemId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtividadeDestinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Lag = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VinculosAtividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VinculosAtividade_AtividadesCronograma_AtividadeDestinoId",
                        column: x => x.AtividadeDestinoId,
                        principalTable: "AtividadesCronograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VinculosAtividade_AtividadesCronograma_AtividadeOrigemId",
                        column: x => x.AtividadeOrigemId,
                        principalTable: "AtividadesCronograma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Afastamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataRetornoPrevisto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    NumeroCAT = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DocumentoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afastamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Afastamentos_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApuracoesPonto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorasNormais = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasExtras50 = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasExtras100 = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasNoturnas = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasFeriado = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasFalta = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    HorasAtraso = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Fechado = table.Column<bool>(type: "boolean", nullable: false),
                    FechadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApuracoesPonto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApuracoesPonto_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosFuncionario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosFuncionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosFuncionario_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EPIFuncionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    EPIId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Quantidade = table.Column<decimal>(type: "numeric(5,0)", precision: 5, scale: 0, nullable: false),
                    NumeroCA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AssinaturaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPIFuncionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EPIFuncionarios_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamesMedicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoExame = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DataExame = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Resultado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Medico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CRM = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DocumentoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamesMedicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamesMedicos_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FolhaFuncionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FolhaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalarioBruto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HorasExtras = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorHorasExtras = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AdicionalNoturno = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Periculosidade = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Insalubridade = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OutrasVerbas = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalProventos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    INSS = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IRRF = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValeTransporte = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValeRefeicao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OutrosDescontos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDescontos = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SalarioLiquido = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FGTS = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolhaFuncionarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolhaFuncionarios_FolhasPagamento_FolhaId",
                        column: x => x.FolhaId,
                        principalTable: "FolhasPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolhaFuncionarios_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosPonto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    HorarioPrevisto = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    HorasExtras = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Latitude = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Longitude = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Dispositivo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    Manual = table.Column<bool>(type: "boolean", nullable: false),
                    Justificativa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPonto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosPonto_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreinamentosFuncionario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeTreinamento = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NormaRelacionada = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataRealizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CargaHoraria = table.Column<decimal>(type: "numeric(5,1)", precision: 5, scale: 1, nullable: false),
                    Instrutor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CertificadoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinamentosFuncionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreinamentosFuncionario_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogAcessos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Acao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Detalhes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Sucesso = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAcessos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogAcessos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioObras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Funcao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioObras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioObras_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPermissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Modulo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PodeVisualizar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeCriar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeEditar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeDeletar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeAprovar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeExportar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeAdministrar = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioPermissoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AditivosContrato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Justificativa = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DataAssinatura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValorAditivo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ProrrogacaoDias = table.Column<int>(type: "integer", nullable: true),
                    NovaDataVigencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArquivoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AditivosContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AditivosContrato_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContratoAnexos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TipoArquivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoAnexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoAnexos_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaturasContrato",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroNF = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorFatura = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorPago = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturasContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturasContrato_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicoesContratuais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Periodo = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ValorMedicao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ValorRetencao = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualMedicao = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PercentualAcumulado = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DataSubmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnalisadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAnalise = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AprovadoPor = table.Column<Guid>(type: "uuid", nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoRejeicao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ArquivoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoesContratuais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicoesContratuais_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecebimentosCompra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataRecebimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroNF = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SerieNF = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DataNF = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorNF = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecebidoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecebimentosCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecebimentosCompra_PedidosCompra_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "PedidosCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstoqueSaldos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaldoAtual = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoReservado = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CustoMedio = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    UltimaMovimentacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueSaldos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueSaldos_Almoxarifados_AlmoxarifadoId",
                        column: x => x.AlmoxarifadoId,
                        principalTable: "Almoxarifados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstoqueSaldos_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensCotacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Especificacao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PrecoReferencia = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCotacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensCotacao_Cotacoes_CotacaoId",
                        column: x => x.CotacaoId,
                        principalTable: "Cotacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensCotacao_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensInventario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaldoSistema = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoContado = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CustoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ajustado = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensInventario_InventariosEstoque_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "InventariosEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensInventario_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensPedidoCompra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    QuantidadePedida = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeRecebida = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPedidoCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPedidoCompra_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensPedidoCompra_PedidosCompra_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "PedidosCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensRequisicao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantidadeSolicitada = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeAtendida = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PrecoReferencia = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensRequisicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensRequisicao_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensRequisicao_RequisicoesMaterial_RequisicaoId",
                        column: x => x.RequisicaoId,
                        principalTable: "RequisicoesMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlmoxarifadoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    FaseObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlmoxarifadoDestinoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoAnterior = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SaldoPosterior = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroNF = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Lote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Validade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequisicaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    PedidoCompraId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Almoxarifados_AlmoxarifadoId",
                        column: x => x.AlmoxarifadoId,
                        principalTable: "Almoxarifados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RDOEquipamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RDOId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    HorasUtilizadas = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RDOEquipamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RDOEquipamentos_RDOs_RDOId",
                        column: x => x.RDOId,
                        principalTable: "RDOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RDOEquipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RDOId = table.Column<Guid>(type: "uuid", nullable: false),
                    Funcao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    HorasTrabalhadas = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Empresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RDOEquipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RDOEquipes_RDOs_RDOId",
                        column: x => x.RDOId,
                        principalTable: "RDOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RDOOcorrencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RDOId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Resolucao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RDOOcorrencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RDOOcorrencias_RDOs_RDOId",
                        column: x => x.RDOId,
                        principalTable: "RDOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsumosComposicao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ComposicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Coeficiente = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FontePrecoEnum = table.Column<int>(type: "integer", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosComposicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumosComposicao_ComposicoesOrcamento_ComposicaoId",
                        column: x => x.ComposicaoId,
                        principalTable: "ComposicoesOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensOrcamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrcamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrupoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComposicaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fonte = table.Column<int>(type: "integer", nullable: false),
                    CodigoFonte = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CustoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CustoComBDI = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BDI = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensOrcamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensOrcamento_ComposicoesOrcamento_ComposicaoId",
                        column: x => x.ComposicaoId,
                        principalTable: "ComposicoesOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensOrcamento_GruposOrcamento_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "GruposOrcamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcessosDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    PodeVisualizar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeEditar = table.Column<bool>(type: "boolean", nullable: false),
                    PodeBaixar = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcessosDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcessosDocumento_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArquivosDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeArquivo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TipoArquivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    NumeroRevisao = table.Column<int>(type: "integer", nullable: false),
                    Atual = table.Column<bool>(type: "boolean", nullable: false),
                    UploadPor = table.Column<Guid>(type: "uuid", nullable: false),
                    HashArquivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosDocumento_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DistribuicoesDoc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinatarioNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DestinatarioEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DestinatarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataDistribuicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Finalidade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    DataConfirmacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistribuicoesDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistribuicoesDoc_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosTransmittal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransmittalId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Finalidade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosTransmittal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentosTransmittal_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentosTransmittal_Transmittals_TransmittalId",
                        column: x => x.TransmittalId,
                        principalTable: "Transmittals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxosAprovacaoDoc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    AprovadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Comentario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxosAprovacaoDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxosAprovacaoDoc_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevisoesDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroRevisao = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RevisadoPor = table.Column<Guid>(type: "uuid", nullable: false),
                    DataRevisao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArquivoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisoesDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisoesDocumento_DocumentosGED_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "DocumentosGED",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnexosLancamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LancamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    TipoArquivo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnexosLancamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnexosLancamento_LancamentosFinanceiros_LancamentoId",
                        column: x => x.LancamentoId,
                        principalTable: "LancamentosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcoesNC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NaoConformidadeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ResponsavelId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrazoConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Evidencia = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Concluida = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcoesNC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcoesNC_NaoConformidades_NaoConformidadeId",
                        column: x => x.NaoConformidadeId,
                        principalTable: "NaoConformidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensMedicaoContratual",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    QuantidadeContratada = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeAnterior = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    QuantidadeAtual = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensMedicaoContratual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensMedicaoContratual_MedicoesContratuais_MedicaoId",
                        column: x => x.MedicaoId,
                        principalTable: "MedicoesContratuais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensPropostaCotacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropostaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemCotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Disponivel = table.Column<bool>(type: "boolean", nullable: false),
                    MenorPreco = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensPropostaCotacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensPropostaCotacao_ItensCotacao_ItemCotacaoId",
                        column: x => x.ItemCotacaoId,
                        principalTable: "ItensCotacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensPropostaCotacao_PropostasCotacao_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "PropostasCotacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensRecebimento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecebimentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemPedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantidadeRecebida = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Lote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Validade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aprovado = table.Column<bool>(type: "boolean", nullable: false),
                    MotivoReprovacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensRecebimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensRecebimento_ItensPedidoCompra_ItemPedidoId",
                        column: x => x.ItemPedidoId,
                        principalTable: "ItensPedidoCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensRecebimento_RecebimentosCompra_RecebimentoId",
                        column: x => x.RecebimentoId,
                        principalTable: "RecebimentosCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcessosDocumento_DocumentoId_UsuarioId",
                table: "AcessosDocumento",
                columns: new[] { "DocumentoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcessosDocumento_Id",
                table: "AcessosDocumento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcoesCorretivasSST_AcidenteId",
                table: "AcoesCorretivasSST",
                column: "AcidenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AcoesCorretivasSST_Id",
                table: "AcoesCorretivasSST",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcoesNC_Id",
                table: "AcoesNC",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcoesNC_NaoConformidadeId",
                table: "AcoesNC",
                column: "NaoConformidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_AditivosContrato_ContratoId",
                table: "AditivosContrato",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_AditivosContrato_Id",
                table: "AditivosContrato",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Afastamentos_FuncionarioId",
                table: "Afastamentos",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Afastamentos_Id",
                table: "Afastamentos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Almoxarifados_Id",
                table: "Almoxarifados",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnexosLancamento_Id",
                table: "AnexosLancamento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnexosLancamento_LancamentoId",
                table: "AnexosLancamento",
                column: "LancamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApuracoesPonto_FuncionarioId_DataReferencia",
                table: "ApuracoesPonto",
                columns: new[] { "FuncionarioId", "DataReferencia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApuracoesPonto_Id",
                table: "ApuracoesPonto",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDocumento_DocumentoId",
                table: "ArquivosDocumento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDocumento_Id",
                table: "ArquivosDocumento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCronograma_AtividadePaiId",
                table: "AtividadesCronograma",
                column: "AtividadePaiId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCronograma_CronogramaId",
                table: "AtividadesCronograma",
                column: "CronogramaId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesCronograma_Id",
                table: "AtividadesCronograma",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_EmpresaId_Codigo",
                table: "Cargos",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_Id",
                table: "Cargos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCusto_CentroPaiId",
                table: "CentrosCusto",
                column: "CentroPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCusto_EmpresaId_Codigo",
                table: "CentrosCusto",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCusto_Id",
                table: "CentrosCusto",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModeloItens_Id",
                table: "ChecklistModeloItens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModeloItens_ModeloId",
                table: "ChecklistModeloItens",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModelos_Id",
                table: "ChecklistModelos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComposicoesOrcamento_Id",
                table: "ComposicoesOrcamento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComposicoesOrcamento_OrcamentoId",
                table: "ComposicoesOrcamento",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_Id",
                table: "ContasBancarias",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAnexos_ContratoId",
                table: "ContratoAnexos",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoAnexos_Id",
                table: "ContratoAnexos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_EmpresaId_Numero",
                table: "Contratos",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_FornecedorId",
                table: "Contratos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_Id",
                table: "Contratos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotacoes_EmpresaId_Numero",
                table: "Cotacoes",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotacoes_Id",
                table: "Cotacoes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CronogramasObra_Id",
                table: "CronogramasObra",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurvaSPontos_CronogramaId",
                table: "CurvaSPontos",
                column: "CronogramaId");

            migrationBuilder.CreateIndex(
                name: "IX_CurvaSPontos_Id",
                table: "CurvaSPontos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DDS_EmpresaId_Numero",
                table: "DDS",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DDS_Id",
                table: "DDS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_DepartamentoPaiId",
                table: "Departamentos",
                column: "DepartamentoPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_Id",
                table: "Departamentos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DistribuicoesDoc_DocumentoId",
                table: "DistribuicoesDoc",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DistribuicoesDoc_Id",
                table: "DistribuicoesDoc",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosFuncionario_FuncionarioId",
                table: "DocumentosFuncionario",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosFuncionario_Id",
                table: "DocumentosFuncionario",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosGED_EmpresaId_Codigo",
                table: "DocumentosGED",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosGED_Id",
                table: "DocumentosGED",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosGED_PastaId",
                table: "DocumentosGED",
                column: "PastaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPCM_Id",
                table: "DocumentosPCM",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosTransmittal_DocumentoId",
                table: "DocumentosTransmittal",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosTransmittal_Id",
                table: "DocumentosTransmittal",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosTransmittal_TransmittalId",
                table: "DocumentosTransmittal",
                column: "TransmittalId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Cnpj",
                table: "Empresas",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Id",
                table: "Empresas",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnsaiosMaterial_Id",
                table: "EnsaiosMaterial",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EPIFuncionarios_FuncionarioId",
                table: "EPIFuncionarios",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EPIFuncionarios_Id",
                table: "EPIFuncionarios",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EPIs_EmpresaId_Codigo",
                table: "EPIs",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EPIs_Id",
                table: "EPIs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueSaldos_AlmoxarifadoId_MaterialId",
                table: "EstoqueSaldos",
                columns: new[] { "AlmoxarifadoId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueSaldos_Id",
                table: "EstoqueSaldos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueSaldos_MaterialId",
                table: "EstoqueSaldos",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamesMedicos_FuncionarioId",
                table: "ExamesMedicos",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamesMedicos_Id",
                table: "ExamesMedicos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FasesObra_FasePaiId",
                table: "FasesObra",
                column: "FasePaiId");

            migrationBuilder.CreateIndex(
                name: "IX_FasesObra_Id",
                table: "FasesObra",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FasesObra_ObraId",
                table: "FasesObra",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturasContrato_ContratoId",
                table: "FaturasContrato",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_FaturasContrato_Id",
                table: "FaturasContrato",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FluxosAprovacaoDoc_DocumentoId",
                table: "FluxosAprovacaoDoc",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxosAprovacaoDoc_Id",
                table: "FluxosAprovacaoDoc",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FolhaFuncionarios_FolhaId",
                table: "FolhaFuncionarios",
                column: "FolhaId");

            migrationBuilder.CreateIndex(
                name: "IX_FolhaFuncionarios_FuncionarioId",
                table: "FolhaFuncionarios",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FolhaFuncionarios_Id",
                table: "FolhaFuncionarios",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FolhasPagamento_Id",
                table: "FolhasPagamento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_EmpresaId_Codigo",
                table: "Fornecedores",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_EmpresaId_Documento",
                table: "Fornecedores",
                columns: new[] { "EmpresaId", "Documento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_Id",
                table: "Fornecedores",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FotosInspecao_Id",
                table: "FotosInspecao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FotosInspecao_InspecaoId",
                table: "FotosInspecao",
                column: "InspecaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_CargoId",
                table: "Funcionarios",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_DepartamentoId",
                table: "Funcionarios",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_EmpresaId_Cpf",
                table: "Funcionarios",
                columns: new[] { "EmpresaId", "Cpf" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_EmpresaId_Matricula",
                table: "Funcionarios",
                columns: new[] { "EmpresaId", "Matricula" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Id",
                table: "Funcionarios",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FVS_EmpresaId_Numero",
                table: "FVS",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FVS_Id",
                table: "FVS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GruposMaterial_GrupoPaiId",
                table: "GruposMaterial",
                column: "GrupoPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_GruposMaterial_Id",
                table: "GruposMaterial",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GruposOrcamento_GrupoPaiId",
                table: "GruposOrcamento",
                column: "GrupoPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_GruposOrcamento_Id",
                table: "GruposOrcamento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GruposOrcamento_OrcamentoId",
                table: "GruposOrcamento",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPlanos_EmpresaId",
                table: "HistoricoPlanos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPlanos_Id",
                table: "HistoricoPlanos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresSST_Id",
                table: "IndicadoresSST",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresSST_ObraId_Competencia",
                table: "IndicadoresSST",
                columns: new[] { "ObraId", "Competencia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspecoes_ChecklistModeloId",
                table: "Inspecoes",
                column: "ChecklistModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspecoes_EmpresaId_Numero",
                table: "Inspecoes",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspecoes_Id",
                table: "Inspecoes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsumosComposicao_ComposicaoId",
                table: "InsumosComposicao",
                column: "ComposicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosComposicao_Id",
                table: "InsumosComposicao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventariosEstoque_Id",
                table: "InventariosEstoque",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensChecklistPT_Id",
                table: "ItensChecklistPT",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensChecklistPT_PermissaoId",
                table: "ItensChecklistPT",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCotacao_CotacaoId",
                table: "ItensCotacao",
                column: "CotacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCotacao_Id",
                table: "ItensCotacao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCotacao_MaterialId",
                table: "ItensCotacao",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensFVS_FVSId",
                table: "ItensFVS",
                column: "FVSId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensFVS_Id",
                table: "ItensFVS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensInspecao_Id",
                table: "ItensInspecao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensInspecao_InspecaoId",
                table: "ItensInspecao",
                column: "InspecaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensInventario_Id",
                table: "ItensInventario",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensInventario_InventarioId",
                table: "ItensInventario",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensInventario_MaterialId",
                table: "ItensInventario",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMedicao_Id",
                table: "ItensMedicao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensMedicao_MedicaoId",
                table: "ItensMedicao",
                column: "MedicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensMedicaoContratual_Id",
                table: "ItensMedicaoContratual",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensMedicaoContratual_MedicaoId",
                table: "ItensMedicaoContratual",
                column: "MedicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensOrcamento_ComposicaoId",
                table: "ItensOrcamento",
                column: "ComposicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensOrcamento_GrupoId",
                table: "ItensOrcamento",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensOrcamento_Id",
                table: "ItensOrcamento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidoCompra_Id",
                table: "ItensPedidoCompra",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidoCompra_MaterialId",
                table: "ItensPedidoCompra",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidoCompra_PedidoId",
                table: "ItensPedidoCompra",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPropostaCotacao_Id",
                table: "ItensPropostaCotacao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensPropostaCotacao_ItemCotacaoId",
                table: "ItensPropostaCotacao",
                column: "ItemCotacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPropostaCotacao_PropostaId",
                table: "ItensPropostaCotacao",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRecebimento_Id",
                table: "ItensRecebimento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensRecebimento_ItemPedidoId",
                table: "ItensRecebimento",
                column: "ItemPedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRecebimento_RecebimentoId",
                table: "ItensRecebimento",
                column: "RecebimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRequisicao_Id",
                table: "ItensRequisicao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensRequisicao_MaterialId",
                table: "ItensRequisicao",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRequisicao_RequisicaoId",
                table: "ItensRequisicao",
                column: "RequisicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_CentroCustoId",
                table: "LancamentosFinanceiros",
                column: "CentroCustoId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_ContaBancariaId",
                table: "LancamentosFinanceiros",
                column: "ContaBancariaId");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_EmpresaId_DataVencimento",
                table: "LancamentosFinanceiros",
                columns: new[] { "EmpresaId", "DataVencimento" });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_EmpresaId_Status",
                table: "LancamentosFinanceiros",
                columns: new[] { "EmpresaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_Id",
                table: "LancamentosFinanceiros",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LancamentosFinanceiros_PlanoContaId",
                table: "LancamentosFinanceiros",
                column: "PlanoContaId");

            migrationBuilder.CreateIndex(
                name: "IX_LogAcessos_Id",
                table: "LogAcessos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogAcessos_UsuarioId",
                table: "LogAcessos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_EmpresaId_Codigo",
                table: "Materiais",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_GrupoId",
                table: "Materiais",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_Id",
                table: "Materiais",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicoesContrato_Id",
                table: "MedicoesContrato",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicoesContratuais_ContratoId",
                table: "MedicoesContratuais",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicoesContratuais_Id",
                table: "MedicoesContratuais",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_AlmoxarifadoId",
                table: "MovimentacoesEstoque",
                column: "AlmoxarifadoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_Id",
                table: "MovimentacoesEstoque",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_MaterialId",
                table: "MovimentacoesEstoque",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_NaoConformidades_EmpresaId_Numero",
                table: "NaoConformidades",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaoConformidades_Id",
                table: "NaoConformidades",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaoConformidades_InspecaoId",
                table: "NaoConformidades",
                column: "InspecaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ObraAnexos_Id",
                table: "ObraAnexos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObraAnexos_ObraId",
                table: "ObraAnexos",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_ObraHistoricos_Id",
                table: "ObraHistoricos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObraHistoricos_ObraId",
                table: "ObraHistoricos",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_EmpresaId_Codigo",
                table: "Obras",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obras_Id",
                table: "Obras",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoHistoricos_Id",
                table: "OrcamentoHistoricos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoHistoricos_OrcamentoId",
                table: "OrcamentoHistoricos",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_EmpresaId_Codigo",
                table: "Orcamentos",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orcamentos_Id",
                table: "Orcamentos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentosDRE_Id",
                table: "OrcamentosDRE",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesDDS_DDSId",
                table: "ParticipantesDDS",
                column: "DDSId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantesDDS_Id",
                table: "ParticipantesDDS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PastasDocumento_Id",
                table: "PastasDocumento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PastasDocumento_PastaPaiId",
                table: "PastasDocumento",
                column: "PastaPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_EmpresaId_Numero",
                table: "PedidosCompra",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_FornecedorId",
                table: "PedidosCompra",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_Id",
                table: "PedidosCompra",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesTrabalho_EmpresaId_Numero",
                table: "PermissoesTrabalho",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissoesTrabalho_Id",
                table: "PermissoesTrabalho",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanoContas_ContaPaiId",
                table: "PlanoContas",
                column: "ContaPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoContas_EmpresaId_Codigo",
                table: "PlanoContas",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanoContas_Id",
                table: "PlanoContas",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgressosAtividade_AtividadeId",
                table: "ProgressosAtividade",
                column: "AtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressosAtividade_Id",
                table: "ProgressosAtividade",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostasCotacao_CotacaoId",
                table: "PropostasCotacao",
                column: "CotacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostasCotacao_FornecedorId",
                table: "PropostasCotacao",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostasCotacao_Id",
                table: "PropostasCotacao",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RDOEquipamentos_Id",
                table: "RDOEquipamentos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RDOEquipamentos_RDOId",
                table: "RDOEquipamentos",
                column: "RDOId");

            migrationBuilder.CreateIndex(
                name: "IX_RDOEquipes_Id",
                table: "RDOEquipes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RDOEquipes_RDOId",
                table: "RDOEquipes",
                column: "RDOId");

            migrationBuilder.CreateIndex(
                name: "IX_RDOOcorrencias_Id",
                table: "RDOOcorrencias",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RDOOcorrencias_RDOId",
                table: "RDOOcorrencias",
                column: "RDOId");

            migrationBuilder.CreateIndex(
                name: "IX_RDOs_Id",
                table: "RDOs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RDOs_ObraId_Numero",
                table: "RDOs",
                columns: new[] { "ObraId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecebimentosCompra_Id",
                table: "RecebimentosCompra",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecebimentosCompra_PedidoId",
                table: "RecebimentosCompra",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAtividade_AtividadeId",
                table: "RecursosAtividade",
                column: "AtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursosAtividade_Id",
                table: "RecursosAtividade",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAcidente_Id",
                table: "RegistrosAcidente",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPonto_FuncionarioId",
                table: "RegistrosPonto",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPonto_Id",
                table: "RegistrosPonto",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicoesMaterial_EmpresaId_Numero",
                table: "RequisicoesMaterial",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicoesMaterial_Id",
                table: "RequisicoesMaterial",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RevisoesDocumento_DocumentoId",
                table: "RevisoesDocumento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_RevisoesDocumento_Id",
                table: "RevisoesDocumento",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestemunhasAcidente_AcidenteId",
                table: "TestemunhasAcidente",
                column: "AcidenteId");

            migrationBuilder.CreateIndex(
                name: "IX_TestemunhasAcidente_Id",
                table: "TestemunhasAcidente",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transferencias_ContaDestinoId",
                table: "Transferencias",
                column: "ContaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencias_ContaOrigemId",
                table: "Transferencias",
                column: "ContaOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencias_Id",
                table: "Transferencias",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transmittals_EmpresaId_Numero",
                table: "Transmittals",
                columns: new[] { "EmpresaId", "Numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transmittals_Id",
                table: "Transmittals",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreinamentosFuncionario_FuncionarioId",
                table: "TreinamentosFuncionario",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TreinamentosFuncionario_Id",
                table: "TreinamentosFuncionario",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioObras_Id",
                table: "UsuarioObras",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioObras_UsuarioId_ObraId",
                table: "UsuarioObras",
                columns: new[] { "UsuarioId", "ObraId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermissoes_Id",
                table: "UsuarioPermissoes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermissoes_UsuarioId_Modulo_EmpresaId",
                table: "UsuarioPermissoes",
                columns: new[] { "UsuarioId", "Modulo", "EmpresaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email_EmpresaId",
                table: "Usuarios",
                columns: new[] { "Email", "EmpresaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id",
                table: "Usuarios",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VinculosAtividade_AtividadeDestinoId",
                table: "VinculosAtividade",
                column: "AtividadeDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_VinculosAtividade_AtividadeOrigemId",
                table: "VinculosAtividade",
                column: "AtividadeOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_VinculosAtividade_Id",
                table: "VinculosAtividade",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcessosDocumento");

            migrationBuilder.DropTable(
                name: "AcoesCorretivasSST");

            migrationBuilder.DropTable(
                name: "AcoesNC");

            migrationBuilder.DropTable(
                name: "AditivosContrato");

            migrationBuilder.DropTable(
                name: "Afastamentos");

            migrationBuilder.DropTable(
                name: "AnexosLancamento");

            migrationBuilder.DropTable(
                name: "ApuracoesPonto");

            migrationBuilder.DropTable(
                name: "ArquivosDocumento");

            migrationBuilder.DropTable(
                name: "ChecklistModeloItens");

            migrationBuilder.DropTable(
                name: "ContratoAnexos");

            migrationBuilder.DropTable(
                name: "CurvaSPontos");

            migrationBuilder.DropTable(
                name: "DistribuicoesDoc");

            migrationBuilder.DropTable(
                name: "DocumentosFuncionario");

            migrationBuilder.DropTable(
                name: "DocumentosPCM");

            migrationBuilder.DropTable(
                name: "DocumentosTransmittal");

            migrationBuilder.DropTable(
                name: "EnsaiosMaterial");

            migrationBuilder.DropTable(
                name: "EPIFuncionarios");

            migrationBuilder.DropTable(
                name: "EPIs");

            migrationBuilder.DropTable(
                name: "EstoqueSaldos");

            migrationBuilder.DropTable(
                name: "ExamesMedicos");

            migrationBuilder.DropTable(
                name: "FasesObra");

            migrationBuilder.DropTable(
                name: "FaturasContrato");

            migrationBuilder.DropTable(
                name: "FluxosAprovacaoDoc");

            migrationBuilder.DropTable(
                name: "FolhaFuncionarios");

            migrationBuilder.DropTable(
                name: "FotosInspecao");

            migrationBuilder.DropTable(
                name: "HistoricoPlanos");

            migrationBuilder.DropTable(
                name: "IndicadoresSST");

            migrationBuilder.DropTable(
                name: "InsumosComposicao");

            migrationBuilder.DropTable(
                name: "ItensChecklistPT");

            migrationBuilder.DropTable(
                name: "ItensFVS");

            migrationBuilder.DropTable(
                name: "ItensInspecao");

            migrationBuilder.DropTable(
                name: "ItensInventario");

            migrationBuilder.DropTable(
                name: "ItensMedicao");

            migrationBuilder.DropTable(
                name: "ItensMedicaoContratual");

            migrationBuilder.DropTable(
                name: "ItensOrcamento");

            migrationBuilder.DropTable(
                name: "ItensPropostaCotacao");

            migrationBuilder.DropTable(
                name: "ItensRecebimento");

            migrationBuilder.DropTable(
                name: "ItensRequisicao");

            migrationBuilder.DropTable(
                name: "LogAcessos");

            migrationBuilder.DropTable(
                name: "MovimentacoesEstoque");

            migrationBuilder.DropTable(
                name: "ObraAnexos");

            migrationBuilder.DropTable(
                name: "ObraHistoricos");

            migrationBuilder.DropTable(
                name: "OrcamentoHistoricos");

            migrationBuilder.DropTable(
                name: "OrcamentosDRE");

            migrationBuilder.DropTable(
                name: "ParticipantesDDS");

            migrationBuilder.DropTable(
                name: "ProgressosAtividade");

            migrationBuilder.DropTable(
                name: "RDOEquipamentos");

            migrationBuilder.DropTable(
                name: "RDOEquipes");

            migrationBuilder.DropTable(
                name: "RDOOcorrencias");

            migrationBuilder.DropTable(
                name: "RecursosAtividade");

            migrationBuilder.DropTable(
                name: "RegistrosPonto");

            migrationBuilder.DropTable(
                name: "RevisoesDocumento");

            migrationBuilder.DropTable(
                name: "TestemunhasAcidente");

            migrationBuilder.DropTable(
                name: "Transferencias");

            migrationBuilder.DropTable(
                name: "TreinamentosFuncionario");

            migrationBuilder.DropTable(
                name: "UsuarioObras");

            migrationBuilder.DropTable(
                name: "UsuarioPermissoes");

            migrationBuilder.DropTable(
                name: "VinculosAtividade");

            migrationBuilder.DropTable(
                name: "NaoConformidades");

            migrationBuilder.DropTable(
                name: "LancamentosFinanceiros");

            migrationBuilder.DropTable(
                name: "Transmittals");

            migrationBuilder.DropTable(
                name: "FolhasPagamento");

            migrationBuilder.DropTable(
                name: "PermissoesTrabalho");

            migrationBuilder.DropTable(
                name: "FVS");

            migrationBuilder.DropTable(
                name: "InventariosEstoque");

            migrationBuilder.DropTable(
                name: "MedicoesContrato");

            migrationBuilder.DropTable(
                name: "MedicoesContratuais");

            migrationBuilder.DropTable(
                name: "ComposicoesOrcamento");

            migrationBuilder.DropTable(
                name: "GruposOrcamento");

            migrationBuilder.DropTable(
                name: "ItensCotacao");

            migrationBuilder.DropTable(
                name: "PropostasCotacao");

            migrationBuilder.DropTable(
                name: "ItensPedidoCompra");

            migrationBuilder.DropTable(
                name: "RecebimentosCompra");

            migrationBuilder.DropTable(
                name: "RequisicoesMaterial");

            migrationBuilder.DropTable(
                name: "Almoxarifados");

            migrationBuilder.DropTable(
                name: "DDS");

            migrationBuilder.DropTable(
                name: "RDOs");

            migrationBuilder.DropTable(
                name: "DocumentosGED");

            migrationBuilder.DropTable(
                name: "RegistrosAcidente");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "AtividadesCronograma");

            migrationBuilder.DropTable(
                name: "Inspecoes");

            migrationBuilder.DropTable(
                name: "CentrosCusto");

            migrationBuilder.DropTable(
                name: "ContasBancarias");

            migrationBuilder.DropTable(
                name: "PlanoContas");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Orcamentos");

            migrationBuilder.DropTable(
                name: "Cotacoes");

            migrationBuilder.DropTable(
                name: "Materiais");

            migrationBuilder.DropTable(
                name: "PedidosCompra");

            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropTable(
                name: "PastasDocumento");

            migrationBuilder.DropTable(
                name: "Cargos");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "CronogramasObra");

            migrationBuilder.DropTable(
                name: "ChecklistModelos");

            migrationBuilder.DropTable(
                name: "GruposMaterial");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
