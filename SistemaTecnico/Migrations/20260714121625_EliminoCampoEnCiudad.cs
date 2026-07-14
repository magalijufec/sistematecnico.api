using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class EliminoCampoEnCiudad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdProvincia",
                table: "Ciudades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProvincia",
                table: "Ciudades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
