using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class CambioCampoFechasTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaTrabajo",
                table: "Trabajos",
                newName: "FechaFinalizado");

            migrationBuilder.RenameColumn(
                name: "FechaAlta",
                table: "Trabajos",
                newName: "FechaInicio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "Trabajos",
                newName: "FechaAlta");

            migrationBuilder.RenameColumn(
                name: "FechaFinalizado",
                table: "Trabajos",
                newName: "FechaTrabajo");
        }
    }
}
