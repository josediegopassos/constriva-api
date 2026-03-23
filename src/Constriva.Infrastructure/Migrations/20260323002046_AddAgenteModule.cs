using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAgenteModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ModuloAgente",
                table: "Empresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AgenteConsumoDiario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TokensInput = table.Column<long>(type: "bigint", nullable: false),
                    TokensOutput = table.Column<long>(type: "bigint", nullable: false),
                    TotalRequisicoes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteConsumoDiario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteConsumoMensal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    TokensUtilizados = table.Column<long>(type: "bigint", nullable: false),
                    TokensLimite = table.Column<long>(type: "bigint", nullable: false),
                    TokensAvulsosUtilizados = table.Column<long>(type: "bigint", nullable: false),
                    Alerta80Enviado = table.Column<bool>(type: "boolean", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteConsumoMensal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteConsumoUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    TokensUtilizados = table.Column<long>(type: "bigint", nullable: false),
                    TotalRequisicoes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteConsumoUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteCotasAvulsas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TokensConcedidos = table.Column<long>(type: "bigint", nullable: false),
                    TokensUtilizados = table.Column<long>(type: "bigint", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ConcedidoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpiraEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ConcedidoPorUsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteCotasAvulsas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteSessoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtualizadaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Ativa = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteSessoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TokensMensais = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuloOrigem = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ReferenciaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DestinatariosJson = table.Column<string>(type: "text", nullable: true),
                    Prazo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Lida = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgenteHistorico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Conteudo = table.Column<string>(type: "text", nullable: false),
                    ToolCallsJson = table.Column<string>(type: "text", nullable: true),
                    TokensInput = table.Column<int>(type: "integer", nullable: false),
                    TokensOutput = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgenteHistorico_AgenteSessoes_SessaoId",
                        column: x => x.SessaoId,
                        principalTable: "AgenteSessoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgenteEmpresaConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgenteTierId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataAtivacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataDesativacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteEmpresaConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgenteEmpresaConfigs_AgenteTiers_AgenteTierId",
                        column: x => x.AgenteTierId,
                        principalTable: "AgenteTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoDiario_EmpresaId_Data",
                table: "AgenteConsumoDiario",
                columns: new[] { "EmpresaId", "Data" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoDiario_Id",
                table: "AgenteConsumoDiario",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoMensal_EmpresaId_Ano_Mes",
                table: "AgenteConsumoMensal",
                columns: new[] { "EmpresaId", "Ano", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoMensal_Id",
                table: "AgenteConsumoMensal",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoUsuario_EmpresaId_UsuarioId_Ano_Mes",
                table: "AgenteConsumoUsuario",
                columns: new[] { "EmpresaId", "UsuarioId", "Ano", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteConsumoUsuario_Id",
                table: "AgenteConsumoUsuario",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteCotasAvulsas_Id",
                table: "AgenteCotasAvulsas",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteEmpresaConfigs_AgenteTierId",
                table: "AgenteEmpresaConfigs",
                column: "AgenteTierId");

            migrationBuilder.CreateIndex(
                name: "IX_AgenteEmpresaConfigs_EmpresaId",
                table: "AgenteEmpresaConfigs",
                column: "EmpresaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteEmpresaConfigs_Id",
                table: "AgenteEmpresaConfigs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteHistorico_Id",
                table: "AgenteHistorico",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteHistorico_SessaoId",
                table: "AgenteHistorico",
                column: "SessaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AgenteSessoes_Id",
                table: "AgenteSessoes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgenteTiers_Id",
                table: "AgenteTiers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Id",
                table: "Notificacoes",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgenteConsumoDiario");

            migrationBuilder.DropTable(
                name: "AgenteConsumoMensal");

            migrationBuilder.DropTable(
                name: "AgenteConsumoUsuario");

            migrationBuilder.DropTable(
                name: "AgenteCotasAvulsas");

            migrationBuilder.DropTable(
                name: "AgenteEmpresaConfigs");

            migrationBuilder.DropTable(
                name: "AgenteHistorico");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "AgenteTiers");

            migrationBuilder.DropTable(
                name: "AgenteSessoes");

            migrationBuilder.DropColumn(
                name: "ModuloAgente",
                table: "Empresas");
        }
    }
}
