using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloLensToEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ModuloLens",
                table: "Empresas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuloLens",
                table: "Empresas");
        }
    }
}
