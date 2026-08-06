using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class addUsuarioCreacionTablaTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Usuarios_TecnicoId",
                table: "Trabajos");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioCreacionId",
                table: "Trabajos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_UsuarioCreacionId",
                table: "Trabajos",
                column: "UsuarioCreacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Usuarios_TecnicoId",
                table: "Trabajos",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Usuarios_UsuarioCreacionId",
                table: "Trabajos",
                column: "UsuarioCreacionId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Usuarios_TecnicoId",
                table: "Trabajos");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Usuarios_UsuarioCreacionId",
                table: "Trabajos");

            migrationBuilder.DropIndex(
                name: "IX_Trabajos_UsuarioCreacionId",
                table: "Trabajos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacionId",
                table: "Trabajos");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Usuarios_TecnicoId",
                table: "Trabajos",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
