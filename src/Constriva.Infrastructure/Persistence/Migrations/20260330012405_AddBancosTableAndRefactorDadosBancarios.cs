using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Constriva.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBancosTableAndRefactorDadosBancarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BancoNome",
                table: "DadosBancarios");

            migrationBuilder.DropColumn(
                name: "CodigoBanco",
                table: "DadosBancarios");

            migrationBuilder.AddColumn<int>(
                name: "BancoId",
                table: "DadosBancarios",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bancos",
                columns: new[] { "Id", "Codigo", "Nome" },
                values: new object[,]
                {
                    { 1, "001", "Banco do Brasil" },
                    { 3, "003", "Banco da Amazônia" },
                    { 4, "004", "Banco do Nordeste do Brasil" },
                    { 10, "010", "Credicoamo" },
                    { 12, "012", "Banco Inbursa" },
                    { 21, "021", "Banestes" },
                    { 24, "024", "Banco Bandepe" },
                    { 25, "025", "Banco Alfa" },
                    { 29, "029", "Banco Itaú Consignado" },
                    { 33, "033", "Banco Santander" },
                    { 36, "036", "Banco Bradesco BBI" },
                    { 37, "037", "Banco do Estado do Pará" },
                    { 40, "040", "Banco Cargill" },
                    { 41, "041", "Banco do Estado do Rio Grande do Sul (Banrisul)" },
                    { 47, "047", "Banco do Estado de Sergipe (Banese)" },
                    { 62, "062", "Hipercard Banco Múltiplo" },
                    { 63, "063", "Banco Bradescard" },
                    { 65, "065", "Banco Lemon" },
                    { 66, "066", "Banco Morgan Stanley" },
                    { 69, "069", "Banco Crefisa" },
                    { 70, "070", "BRB - Banco de Brasília" },
                    { 74, "074", "Banco J. Safra" },
                    { 75, "075", "Banco ABN Amro" },
                    { 76, "076", "Banco KDB do Brasil" },
                    { 77, "077", "Banco Inter" },
                    { 78, "078", "Haitong Banco de Investimento" },
                    { 79, "079", "Banco Original do Agronegócio" },
                    { 80, "080", "B&T Corretora de Câmbio" },
                    { 81, "081", "Banco Seguro (BBN)" },
                    { 82, "082", "Banco Topázio" },
                    { 83, "083", "Banco da China Brasil" },
                    { 84, "084", "Uniprime Norte do Paraná" },
                    { 85, "085", "Cooperativa Central de Crédito - Ailos" },
                    { 88, "088", "Banco Randon" },
                    { 89, "089", "Cooperativa de Crédito Rural da Região da Mogiana" },
                    { 91, "091", "Unicred Central RS" },
                    { 92, "092", "BRK Financeira" },
                    { 93, "093", "Pólocred" },
                    { 94, "094", "Banco Finaxis" },
                    { 95, "095", "Travelex Banco de Câmbio" },
                    { 96, "096", "Banco B3" },
                    { 97, "097", "Cooperativa Central de Crédito Noroeste Brasileiro (CentralCredi)" },
                    { 98, "098", "Credialiança Cooperativa de Crédito Rural" },
                    { 99, "099", "Uniprime Central" },
                    { 100, "100", "Planner Corretora de Valores" },
                    { 104, "104", "Caixa Econômica Federal" },
                    { 107, "107", "Banco Bocom BBM" },
                    { 119, "119", "Banco Western Union" },
                    { 120, "120", "Banco Rodobens" },
                    { 121, "121", "Banco Agibank" },
                    { 125, "125", "Banco Genial" },
                    { 128, "128", "MS Bank Banco de Câmbio" },
                    { 136, "136", "Unicred do Brasil" },
                    { 144, "144", "Bexs Banco de Câmbio" },
                    { 169, "169", "Banco Olé Consignado" },
                    { 184, "184", "Banco Itaú BBA" },
                    { 197, "197", "Stone Pagamentos" },
                    { 208, "208", "Banco BTG Pactual" },
                    { 212, "212", "Banco Original" },
                    { 213, "213", "Banco Arbi" },
                    { 217, "217", "Banco John Deere" },
                    { 218, "218", "Banco BS2" },
                    { 222, "222", "Banco Credit Agricole Brasil" },
                    { 224, "224", "Banco Fibra" },
                    { 233, "233", "Banco Cifra" },
                    { 237, "237", "Banco Bradesco" },
                    { 241, "241", "Banco Clássico" },
                    { 243, "243", "Banco Máxima" },
                    { 246, "246", "Banco ABC Brasil" },
                    { 249, "249", "Banco Investcred Unibanco" },
                    { 250, "250", "BCV - Banco de Crédito e Varejo" },
                    { 254, "254", "Paraná Banco" },
                    { 260, "260", "Nu Pagamentos (Nubank)" },
                    { 265, "265", "Banco Fator" },
                    { 266, "266", "Banco Cédula" },
                    { 269, "269", "HSBC Brasil Banco Múltiplo" },
                    { 270, "270", "Sagitur Corretora de Câmbio" },
                    { 271, "271", "IB Corretora de Câmbio" },
                    { 272, "272", "AGK Corretora de Câmbio" },
                    { 274, "274", "Money Plus" },
                    { 276, "276", "Banco Senff" },
                    { 278, "278", "Genial Investimentos" },
                    { 279, "279", "Cooperativa de Crédito Rural de Primavera do Leste" },
                    { 280, "280", "Avista Financeira" },
                    { 281, "281", "Cooperativa de Crédito Rural Coopavel" },
                    { 283, "283", "RB Investimentos" },
                    { 285, "285", "Frente Corretora de Câmbio" },
                    { 286, "286", "Cooperativa de Crédito Rural de Ouro Sulcredi" },
                    { 288, "288", "Carol Distribuidora" },
                    { 289, "289", "Decyseo Corretora de Câmbio" },
                    { 290, "290", "PagSeguro Internet" },
                    { 292, "292", "BS2 Distribuidora de Títulos e Valores Mobiliários" },
                    { 293, "293", "Lastro RDV" },
                    { 296, "296", "Vision" },
                    { 299, "299", "Banco Sorocred" },
                    { 300, "300", "Banco de la Nacion Argentina" },
                    { 301, "301", "BPP Instituição de Pagamento" },
                    { 310, "310", "VORTX" },
                    { 318, "318", "Banco BMG" },
                    { 320, "320", "Banco CCB Brasil" },
                    { 322, "322", "Cooperativa de Crédito de Livre Admissão (Rede)" },
                    { 323, "323", "Mercado Pago" },
                    { 329, "329", "QI Sociedade de Crédito Direto" },
                    { 330, "330", "Banco Bari de Investimentos e Financiamentos" },
                    { 331, "331", "Fram Capital" },
                    { 332, "332", "Acesso Soluções de Pagamento" },
                    { 335, "335", "Banco Digio" },
                    { 336, "336", "Banco C6" },
                    { 340, "340", "Super Pagamentos" },
                    { 341, "341", "Itaú Unibanco" },
                    { 342, "342", "Creditas" },
                    { 343, "343", "FFA" },
                    { 348, "348", "Banco XP" },
                    { 349, "349", "Amaggi" },
                    { 352, "352", "Toro Corretora de Títulos e Valores Mobiliários" },
                    { 356, "356", "Banco ABN Amro Real (encerrado, mas código histórico)" },
                    { 359, "359", "Zema Crédito Financiamento e Investimento" },
                    { 360, "360", "Trinus Capital" },
                    { 362, "362", "Cielo" },
                    { 363, "363", "Socopa" },
                    { 364, "364", "Gerencianet (Efí)" },
                    { 365, "365", "Solidus" },
                    { 366, "366", "Banco Société Générale Brasil" },
                    { 368, "368", "Banco CSF (Carrefour)" },
                    { 370, "370", "Banco Mizuho do Brasil" },
                    { 376, "376", "Banco J.P. Morgan" },
                    { 380, "380", "PicPay Servicos" },
                    { 381, "381", "Banco Mercedes-Benz" },
                    { 382, "382", "Fiducia" },
                    { 383, "383", "Juno" },
                    { 384, "384", "Global SCM" },
                    { 386, "386", "Nu Financeira" },
                    { 389, "389", "Banco Mercantil do Brasil" },
                    { 394, "394", "Banco Bradesco Financiamentos" },
                    { 396, "396", "HUB Pagamentos" },
                    { 399, "399", "Kirton Bank" },
                    { 403, "403", "Cora Sociedade de Crédito Direto" },
                    { 404, "404", "Sumup Sociedade de Crédito Direto" },
                    { 406, "406", "Accredito" },
                    { 412, "412", "Banco Capital" },
                    { 422, "422", "Banco Safra" },
                    { 456, "456", "Banco MUFG Brasil" },
                    { 464, "464", "Banco Sumitomo Mitsui Brasileiro" },
                    { 473, "473", "Banco Caixa Geral Brasil" },
                    { 477, "477", "Citibank" },
                    { 487, "487", "Deutsche Bank Brasil" },
                    { 488, "488", "JPMorgan Chase Bank (filial)" },
                    { 492, "492", "ING Bank" },
                    { 505, "505", "Banco Credit Suisse" },
                    { 600, "600", "Banco Luso Brasileiro" },
                    { 604, "604", "Banco Industrial do Brasil" },
                    { 610, "610", "Banco VR" },
                    { 611, "611", "Banco Paulista" },
                    { 612, "612", "Banco Guanabara" },
                    { 613, "613", "Banco Pecúnia" },
                    { 623, "623", "Banco Pan" },
                    { 626, "626", "Banco C6 Consignado" },
                    { 630, "630", "Banco Smartbank" },
                    { 633, "633", "Banco Rendimento" },
                    { 634, "634", "Banco Triângulo" },
                    { 637, "637", "Banco Sofisa" },
                    { 643, "643", "Banco Pine" },
                    { 652, "652", "Itaú Unibanco Holding" },
                    { 653, "653", "Banco Indusval" },
                    { 654, "654", "Banco A.J. Renner" },
                    { 655, "655", "Banco Votorantim" },
                    { 707, "707", "Banco Daycoval" },
                    { 712, "712", "Banco Ourinvest" },
                    { 739, "739", "Banco Cetelem" },
                    { 741, "741", "Banco Ribeirão Preto" },
                    { 743, "743", "Banco Semear" },
                    { 745, "745", "Banco Citibank" },
                    { 746, "746", "Banco Modal" },
                    { 747, "747", "Banco Rabobank Internacional Brasil" },
                    { 748, "748", "Banco Cooperativo Sicredi" },
                    { 751, "751", "Scotiabank Brasil Banco Múltiplo" },
                    { 752, "752", "Banco BNP Paribas Brasil" },
                    { 753, "753", "Banco Comercial Uruguai (NBC Bank Brasil)" },
                    { 754, "754", "Banco Sistema" },
                    { 755, "755", "Bank of America Merrill Lynch Banco Múltiplo" },
                    { 756, "756", "Banco Cooperativo do Brasil (Bancoob/Sicoob)" },
                    { 757, "757", "Banco KEB Hana do Brasil" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DadosBancarios_BancoId",
                table: "DadosBancarios",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Bancos_Codigo",
                table: "Bancos",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DadosBancarios_Bancos_BancoId",
                table: "DadosBancarios",
                column: "BancoId",
                principalTable: "Bancos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DadosBancarios_Bancos_BancoId",
                table: "DadosBancarios");

            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropIndex(
                name: "IX_DadosBancarios_BancoId",
                table: "DadosBancarios");

            migrationBuilder.DropColumn(
                name: "BancoId",
                table: "DadosBancarios");

            migrationBuilder.AddColumn<string>(
                name: "BancoNome",
                table: "DadosBancarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoBanco",
                table: "DadosBancarios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
