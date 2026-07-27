using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class cambioModeloImagenComparacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsAntes",
                table: "Imagenes");

            migrationBuilder.CreateTable(
                name: "TrabajoImagenComparaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrabajoId = table.Column<int>(type: "int", nullable: false),
                    ImagenAntesId = table.Column<int>(type: "int", nullable: true),
                    ImagenDespuesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajoImagenComparaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajoImagenComparaciones_Imagenes_ImagenAntesId",
                        column: x => x.ImagenAntesId,
                        principalTable: "Imagenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrabajoImagenComparaciones_Imagenes_ImagenDespuesId",
                        column: x => x.ImagenDespuesId,
                        principalTable: "Imagenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrabajoImagenComparaciones_Trabajos_TrabajoId",
                        column: x => x.TrabajoId,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrabajoImagenComparaciones_ImagenAntesId",
                table: "TrabajoImagenComparaciones",
                column: "ImagenAntesId");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajoImagenComparaciones_ImagenDespuesId",
                table: "TrabajoImagenComparaciones",
                column: "ImagenDespuesId");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajoImagenComparaciones_TrabajoId",
                table: "TrabajoImagenComparaciones",
                column: "TrabajoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrabajoImagenComparaciones");

            migrationBuilder.AddColumn<bool>(
                name: "EsAntes",
                table: "Imagenes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
