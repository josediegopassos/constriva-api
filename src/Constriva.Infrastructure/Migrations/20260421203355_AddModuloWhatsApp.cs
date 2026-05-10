using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloWhatsApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CotacoesWhatsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TelefoneEmpresa = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NomeExibicaoEmpresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisparadaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataLimiteResposta = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalFornecedoresConvidados = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalRespostas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalPropostasExtraidas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EncerradaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MensagemPersonalizada = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_CotacoesWhatsApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MensagensWhatsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoWhatsAppId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorCotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TelefoneDestino = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NomeFornecedor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoMensagem = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    WaMessageId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EnviadaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EntregueEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LidaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NumeroTentativa = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    ErroEnvio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PayloadEnviado = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_MensagensWhatsApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensagensWhatsApp_CotacoesWhatsApp_CotacaoWhatsAppId",
                        column: x => x.CotacaoWhatsAppId,
                        principalTable: "CotacoesWhatsApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespostasFornecedorWhatsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CotacaoWhatsAppId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorCotacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropostaCotacaoId = table.Column<Guid>(type: "uuid", nullable: true),
                    WaMessageId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TelefoneOrigem = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RecebidaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TipoConteudo = table.Column<int>(type: "integer", nullable: false),
                    TextoMensagem = table.Column<string>(type: "text", nullable: true),
                    WaMediaId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MediaUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MediaMimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MediaNomeArquivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MediaPathArmazenado = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PayloadWebhookOriginal = table.Column<string>(type: "text", nullable: false),
                    ProcessadoPelaIa = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ProcessadaEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NivelConfianca = table.Column<int>(type: "integer", nullable: true),
                    ExtraidaComSucesso = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MotivoFalha = table.Column<int>(type: "integer", nullable: true),
                    DescricaoFalha = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TentativasProcessamento = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_RespostasFornecedorWhatsApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RespostasFornecedorWhatsApp_CotacoesWhatsApp_CotacaoWhatsAp~",
                        column: x => x.CotacaoWhatsAppId,
                        principalTable: "CotacoesWhatsApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CotacoesWhatsApp_CotacaoId",
                table: "CotacoesWhatsApp",
                column: "CotacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CotacoesWhatsApp_EmpresaId",
                table: "CotacoesWhatsApp",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_CotacoesWhatsApp_EmpresaId_DataLimiteResposta",
                table: "CotacoesWhatsApp",
                columns: new[] { "EmpresaId", "DataLimiteResposta" });

            migrationBuilder.CreateIndex(
                name: "IX_CotacoesWhatsApp_Id",
                table: "CotacoesWhatsApp",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_CotacaoWhatsAppId",
                table: "MensagensWhatsApp",
                column: "CotacaoWhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_EmpresaId",
                table: "MensagensWhatsApp",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_EmpresaId_Status",
                table: "MensagensWhatsApp",
                columns: new[] { "EmpresaId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_FornecedorCotacaoId_TipoMensagem",
                table: "MensagensWhatsApp",
                columns: new[] { "FornecedorCotacaoId", "TipoMensagem" });

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_FornecedorId",
                table: "MensagensWhatsApp",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_Id",
                table: "MensagensWhatsApp",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MensagensWhatsApp_WaMessageId",
                table: "MensagensWhatsApp",
                column: "WaMessageId",
                unique: true,
                filter: "\"WaMessageId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_CotacaoWhatsAppId",
                table: "RespostasFornecedorWhatsApp",
                column: "CotacaoWhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_EmpresaId",
                table: "RespostasFornecedorWhatsApp",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_EmpresaId_ProcessadoPelaIa",
                table: "RespostasFornecedorWhatsApp",
                columns: new[] { "EmpresaId", "ProcessadoPelaIa" });

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_FornecedorCotacaoId",
                table: "RespostasFornecedorWhatsApp",
                column: "FornecedorCotacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_FornecedorId",
                table: "RespostasFornecedorWhatsApp",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_Id",
                table: "RespostasFornecedorWhatsApp",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_TelefoneOrigem_RecebidaEm",
                table: "RespostasFornecedorWhatsApp",
                columns: new[] { "TelefoneOrigem", "RecebidaEm" });

            migrationBuilder.CreateIndex(
                name: "IX_RespostasFornecedorWhatsApp_WaMessageId",
                table: "RespostasFornecedorWhatsApp",
                column: "WaMessageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MensagensWhatsApp");

            migrationBuilder.DropTable(
                name: "RespostasFornecedorWhatsApp");

            migrationBuilder.DropTable(
                name: "CotacoesWhatsApp");
        }
    }
}
