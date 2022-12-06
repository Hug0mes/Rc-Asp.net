using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ef_2.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artigos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<int>(type: "int", nullable: false),
                    QtaStock = table.Column<short>(type: "smallint", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artigos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateTable(
              name: "Fabricantes",
              columns: table => new
              {
                  Id = table.Column<int>(type: "int", nullable: false)
                      .Annotation("SqlServer:Identity", "1, 1"),
                  Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                  site_web = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                  Imagem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Categorias", x => x.Id);
              });


            migrationBuilder.CreateIndex(
                name: "IX_Artigos_CategoriaId",
                table: "Artigos",
                column: "CategoriaId");
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artigos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
              name: "Fabricantes");

        }
    }
}
