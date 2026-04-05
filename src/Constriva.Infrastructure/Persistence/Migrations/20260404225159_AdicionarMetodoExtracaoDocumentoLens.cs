using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMetodoExtracaoDocumentoLens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetodoExtracao",
                table: "DocumentosLens",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetodoExtracao",
                table: "DocumentosLens");
        }
    }
}
