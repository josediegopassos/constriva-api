using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloLens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentosLens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObraId = table.Column<Guid>(type: "uuid", nullable: true),
                    CentroCustoId = table.Column<Guid>(type: "uuid", nullable: true),
                    FornecedorId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoDocumento = table.Column<int>(type: "integer", nullable: false),
                    TipoDocumentoDeclarado = table.Column<int>(type: "integer", nullable: false),
                    TiposConferem = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    NomeArquivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ExtensaoArquivo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    ConfidenceScore = table.Column<float>(type: "real", nullable: true),
                    TextoBruto = table.Column<string>(type: "text", nullable: true),
                    Warnings = table.Column<List<string>>(type: "jsonb", nullable: false),
                    MensagemErro = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CodigoErro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PodeReprocessar = table.Column<bool>(type: "boolean", nullable: false),
                    TentativaNumero = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    TempoProcessamentoMs = table.Column<int>(type: "integer", nullable: true),
                    PaginasProcessadas = table.Column<int>(type: "integer", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MotivoRejeicaoOuCancelamento = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    NumeroDocumentoExtraido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataEmissaoExtraida = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ValorTotalExtraido = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CnpjFornecedorExtraido = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    NomeFornecedorExtraido = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CompraId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_DocumentosLens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItensDocumentoLens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentoLensId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrdemItem = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Ncm = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cfop = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Unidade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Quantidade = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    PrecoTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Desconto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AliquotaIcms = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    AliquotaIpi = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MotivoRejeicao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DescricaoOriginalOcr = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    QuantidadeOriginalOcr = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    PrecoUnitarioOriginalOcr = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    PrecoTotalOriginalOcr = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EditadoPorUsuarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    EditadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_ItensDocumentoLens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensDocumentoLens_DocumentosLens_DocumentoLensId",
                        column: x => x.DocumentoLensId,
                        principalTable: "DocumentosLens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLens_CnpjFornecedorExtraido",
                table: "DocumentosLens",
                column: "CnpjFornecedorExtraido");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLens_CreatedAt",
                table: "DocumentosLens",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLens_EmpresaId",
                table: "DocumentosLens",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLens_ObraId",
                table: "DocumentosLens",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosLens_Status",
                table: "DocumentosLens",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ItensDocumentoLens_DocumentoLensId",
                table: "ItensDocumentoLens",
                column: "DocumentoLensId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensDocumentoLens_ProdutoId",
                table: "ItensDocumentoLens",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensDocumentoLens_Status",
                table: "ItensDocumentoLens",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensDocumentoLens");

            migrationBuilder.DropTable(
                name: "DocumentosLens");
        }
    }
}
