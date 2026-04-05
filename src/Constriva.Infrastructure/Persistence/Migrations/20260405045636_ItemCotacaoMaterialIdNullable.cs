using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ItemCotacaoMaterialIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "ItensCotacao",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_ObraId",
                table: "PedidosCompra",
                column: "ObraId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompra_Obras_ObraId",
                table: "PedidosCompra",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompra_Obras_ObraId",
                table: "PedidosCompra");

            migrationBuilder.DropIndex(
                name: "IX_PedidosCompra_ObraId",
                table: "PedidosCompra");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "ItensCotacao",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
