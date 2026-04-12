using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAprovacaoPontoAndReprovadoPor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReprovadoPor",
                table: "RegistrosPonto",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusAprovacao",
                table: "RegistrosPonto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cotacoes_FornecedorVencedorId",
                table: "Cotacoes",
                column: "FornecedorVencedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotacoes_ObraId",
                table: "Cotacoes",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cotacoes_Fornecedores_FornecedorVencedorId",
                table: "Cotacoes",
                column: "FornecedorVencedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cotacoes_Obras_ObraId",
                table: "Cotacoes",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cotacoes_Fornecedores_FornecedorVencedorId",
                table: "Cotacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Cotacoes_Obras_ObraId",
                table: "Cotacoes");

            migrationBuilder.DropIndex(
                name: "IX_Cotacoes_FornecedorVencedorId",
                table: "Cotacoes");

            migrationBuilder.DropIndex(
                name: "IX_Cotacoes_ObraId",
                table: "Cotacoes");

            migrationBuilder.DropColumn(
                name: "ReprovadoPor",
                table: "RegistrosPonto");

            migrationBuilder.DropColumn(
                name: "StatusAprovacao",
                table: "RegistrosPonto");
        }
    }
}
