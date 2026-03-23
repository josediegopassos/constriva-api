using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constriva.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnderecoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Almoxarifados");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Almoxarifados");

            migrationBuilder.AddColumn<Guid>(
                name: "EnderecoId",
                table: "Obras",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnderecoId",
                table: "Funcionarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnderecoId",
                table: "Fornecedores",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnderecoId",
                table: "Clientes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnderecoId",
                table: "Almoxarifados",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", precision: 10, scale: 7, nullable: true),
                    Longitude = table.Column<double>(type: "double precision", precision: 10, scale: 7, nullable: true),
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
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Obras_EnderecoId",
                table: "Obras",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_EnderecoId",
                table: "Funcionarios",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_EnderecoId",
                table: "Fornecedores",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EnderecoId",
                table: "Clientes",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Almoxarifados_EnderecoId",
                table: "Almoxarifados",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_Id",
                table: "Enderecos",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Almoxarifados_Enderecos_EnderecoId",
                table: "Almoxarifados",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Enderecos_EnderecoId",
                table: "Clientes",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedores_Enderecos_EnderecoId",
                table: "Fornecedores",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Enderecos_EnderecoId",
                table: "Funcionarios",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Obras_Enderecos_EnderecoId",
                table: "Obras",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Almoxarifados_Enderecos_EnderecoId",
                table: "Almoxarifados");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Enderecos_EnderecoId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedores_Enderecos_EnderecoId",
                table: "Fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Enderecos_EnderecoId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Obras_Enderecos_EnderecoId",
                table: "Obras");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Obras_EnderecoId",
                table: "Obras");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_EnderecoId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedores_EnderecoId",
                table: "Fornecedores");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_EnderecoId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Almoxarifados_EnderecoId",
                table: "Almoxarifados");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Obras");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Almoxarifados");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Obras",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Obras",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Obras",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Obras",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Obras",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Obras",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Obras",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Obras",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Obras",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Funcionarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Funcionarios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Funcionarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Funcionarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Funcionarios",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Funcionarios",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Funcionarios",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Fornecedores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Fornecedores",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Fornecedores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Fornecedores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Fornecedores",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Fornecedores",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Fornecedores",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Clientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Clientes",
                type: "character varying(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Clientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Clientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Clientes",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Clientes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Clientes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Almoxarifados",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Almoxarifados",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);
        }
    }
}
