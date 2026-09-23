using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class gestionIncidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Soporte",
                table: "Tareas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destinos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosIncidencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosIncidencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DestinoId = table.Column<int>(type: "integer", nullable: false),
                    TareaId = table.Column<int>(type: "integer", nullable: false),
                    AsistenciaId = table.Column<int>(type: "integer", nullable: false),
                    EstadoIncidenciaId = table.Column<int>(type: "integer", nullable: false),
                    Otro = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TrabajoRealizado = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    UsuarioFinalizadoId = table.Column<int>(type: "integer", nullable: true),
                    FechaFinalizado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidencias_Asistencias_AsistenciaId",
                        column: x => x.AsistenciaId,
                        principalTable: "Asistencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidencias_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidencias_Destinos_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Destinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidencias_EstadosIncidencia_EstadoIncidenciaId",
                        column: x => x.EstadoIncidenciaId,
                        principalTable: "EstadosIncidencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidencias_Tareas_TareaId",
                        column: x => x.TareaId,
                        principalTable: "Tareas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_UsuarioFinalizadoId",
                        column: x => x.UsuarioFinalizadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Asistencias",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Remota" },
                    { 2, "Presencial" }
                });

            migrationBuilder.InsertData(
                table: "Destinos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Farmacia" },
                    { 2, "Droguería" }
                });

            migrationBuilder.InsertData(
                table: "EstadosIncidencia",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Finalizado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_AsistenciaId",
                table: "Incidencias",
                column: "AsistenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_ClienteId",
                table: "Incidencias",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_DestinoId",
                table: "Incidencias",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_EstadoIncidenciaId",
                table: "Incidencias",
                column: "EstadoIncidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_Fecha",
                table: "Incidencias",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_TareaId",
                table: "Incidencias",
                column: "TareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_UsuarioFinalizadoId",
                table: "Incidencias",
                column: "UsuarioFinalizadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_UsuarioId",
                table: "Incidencias",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidencias");

            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "Destinos");

            migrationBuilder.DropTable(
                name: "EstadosIncidencia");

            migrationBuilder.DropColumn(
                name: "Soporte",
                table: "Tareas");
        }
    }
}
