using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDadosBancariosAndRefactorFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BancoAgencia",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "BancoConta",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "BancoNome",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "PixChave",
                table: "Funcionarios");

            migrationBuilder.AddColumn<Guid>(
                name: "DadosBancariosId",
                table: "Funcionarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DadosBancarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoBanco = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BancoNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Agencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Conta = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PixChave = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_DadosBancarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_DadosBancariosId",
                table: "Funcionarios",
                column: "DadosBancariosId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_ObraAtualId",
                table: "Funcionarios",
                column: "ObraAtualId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_GestorId",
                table: "Departamentos",
                column: "GestorId");

            migrationBuilder.CreateIndex(
                name: "IX_CronogramasObra_ObraId",
                table: "CronogramasObra",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosBancarios_Id",
                table: "DadosBancarios",
                column: "Id",
                unique: true);

            // Limpar FKs órfãs antes de criar constraints
            migrationBuilder.Sql(
                """DELETE FROM "CronogramasObra" WHERE "ObraId" NOT IN (SELECT "Id" FROM "Obras")""");
            migrationBuilder.Sql(
                """UPDATE "Departamentos" SET "GestorId" = NULL WHERE "GestorId" IS NOT NULL AND "GestorId" NOT IN (SELECT "Id" FROM "Funcionarios")""");
            migrationBuilder.Sql(
                """UPDATE "Funcionarios" SET "ObraAtualId" = NULL WHERE "ObraAtualId" IS NOT NULL AND "ObraAtualId" NOT IN (SELECT "Id" FROM "Obras")""");

            migrationBuilder.AddForeignKey(
                name: "FK_CronogramasObra_Obras_ObraId",
                table: "CronogramasObra",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_Funcionarios_GestorId",
                table: "Departamentos",
                column: "GestorId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_DadosBancarios_DadosBancariosId",
                table: "Funcionarios",
                column: "DadosBancariosId",
                principalTable: "DadosBancarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Obras_ObraAtualId",
                table: "Funcionarios",
                column: "ObraAtualId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CronogramasObra_Obras_ObraId",
                table: "CronogramasObra");

            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_Funcionarios_GestorId",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_DadosBancarios_DadosBancariosId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Obras_ObraAtualId",
                table: "Funcionarios");

            migrationBuilder.DropTable(
                name: "DadosBancarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_DadosBancariosId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_ObraAtualId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Departamentos_GestorId",
                table: "Departamentos");

            migrationBuilder.DropIndex(
                name: "IX_CronogramasObra_ObraId",
                table: "CronogramasObra");

            migrationBuilder.DropColumn(
                name: "DadosBancariosId",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "BancoAgencia",
                table: "Funcionarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BancoConta",
                table: "Funcionarios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BancoNome",
                table: "Funcionarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PixChave",
                table: "Funcionarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
