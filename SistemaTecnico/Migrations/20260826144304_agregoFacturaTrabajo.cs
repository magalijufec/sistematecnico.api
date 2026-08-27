using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class agregoFacturaTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Factura",
                table: "Trabajos");

            migrationBuilder.CreateTable(
                name: "TrabajoFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrabajoId = table.Column<int>(type: "int", nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPagado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajoFacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajoFacturas_Trabajos_TrabajoId",
                        column: x => x.TrabajoId,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrabajoFacturas_TrabajoId",
                table: "TrabajoFacturas",
                column: "TrabajoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrabajoFacturas");

            migrationBuilder.AddColumn<string>(
                name: "Factura",
                table: "Trabajos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
