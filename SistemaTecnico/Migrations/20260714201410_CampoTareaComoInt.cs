using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class CampoTareaComoInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarea",
                table: "Trabajos");

            migrationBuilder.AddColumn<int>(
                name: "TareaId",
                table: "Trabajos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_TareaId",
                table: "Trabajos",
                column: "TareaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Tareas_TareaId",
                table: "Trabajos",
                column: "TareaId",
                principalTable: "Tareas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Tareas_TareaId",
                table: "Trabajos");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Trabajos_TareaId",
                table: "Trabajos");

            migrationBuilder.DropColumn(
                name: "TareaId",
                table: "Trabajos");

            migrationBuilder.AddColumn<string>(
                name: "Tarea",
                table: "Trabajos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
