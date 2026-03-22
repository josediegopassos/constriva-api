using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMaterialUniqueIndexPartial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materiais_EmpresaId_Codigo",
                table: "Materiais");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_EmpresaId_Codigo",
                table: "Materiais",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materiais_EmpresaId_Codigo",
                table: "Materiais");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_EmpresaId_Codigo",
                table: "Materiais",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);
        }
    }
}
