using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloFornecedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ModuloFornecedores",
                table: "Empresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuloFornecedores",
                table: "Empresas");
        }
    }
}
