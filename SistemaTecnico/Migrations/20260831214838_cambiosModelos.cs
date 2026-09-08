using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class cambiosModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TecnicoId",
                table: "Trabajos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Materiales",
                table: "Trabajos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Trabajos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Tareas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EstadoPresupuesto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoPresupuesto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Presupuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioDecisionId = table.Column<int>(type: "int", nullable: true),
                    FechaDecision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    MotivoRechazo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrabajoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presupuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presupuestos_EstadoPresupuesto_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadoPresupuesto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Trabajos_TrabajoId",
                        column: x => x.TrabajoId,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Usuarios_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Usuarios_UsuarioDecisionId",
                        column: x => x.UsuarioDecisionId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_SectorId",
                table: "Trabajos",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_SectorId",
                table: "Tareas",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_EstadoId",
                table: "Presupuestos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_TecnicoId",
                table: "Presupuestos",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_TrabajoId",
                table: "Presupuestos",
                column: "TrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_UsuarioDecisionId",
                table: "Presupuestos",
                column: "UsuarioDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectores_UsuarioId",
                table: "Sectores",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Sectores_SectorId",
                table: "Tareas",
                column: "SectorId",
                principalTable: "Sectores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Sectores_SectorId",
                table: "Trabajos",
                column: "SectorId",
                principalTable: "Sectores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Sectores_SectorId",
                table: "Tareas");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Sectores_SectorId",
                table: "Trabajos");

            migrationBuilder.DropTable(
                name: "Presupuestos");

            migrationBuilder.DropTable(
                name: "Sectores");

            migrationBuilder.DropTable(
                name: "EstadoPresupuesto");

            migrationBuilder.DropIndex(
                name: "IX_Trabajos_SectorId",
                table: "Trabajos");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_SectorId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "Materiales",
                table: "Trabajos");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Trabajos");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Tareas");

            migrationBuilder.AlterColumn<int>(
                name: "TecnicoId",
                table: "Trabajos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
