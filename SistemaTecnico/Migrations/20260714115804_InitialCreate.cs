using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosTrabajo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProvincia = table.Column<int>(type: "int", nullable: false),
                    ProvinciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudades_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressShipToCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinciaId = table.Column<int>(type: "int", nullable: false),
                    CiudadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Ciudades_CiudadId",
                        column: x => x.CiudadId,
                        principalTable: "Ciudades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clientes_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCelular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    ProvinciaId = table.Column<int>(type: "int", nullable: true),
                    CiudadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Ciudades_CiudadId",
                        column: x => x.CiudadId,
                        principalTable: "Ciudades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Provincias_ProvinciaId",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trabajos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaTrabajo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TecnicoId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Tarea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrabajoRealizado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Factura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPagado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trabajos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trabajos_EstadosTrabajo_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadosTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trabajos_Usuarios_TecnicoId",
                        column: x => x.TecnicoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrabajoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tamanio = table.Column<long>(type: "bigint", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagenes_Trabajos_TrabajoId",
                        column: x => x.TrabajoId,
                        principalTable: "Trabajos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ciudades_ProvinciaId",
                table: "Ciudades",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CiudadId",
                table: "Clientes",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ProvinciaId",
                table: "Clientes",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_TrabajoId",
                table: "Imagenes",
                column: "TrabajoId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_ClienteId",
                table: "Trabajos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_EstadoId",
                table: "Trabajos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajos_TecnicoId",
                table: "Trabajos",
                column: "TecnicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CiudadId",
                table: "Usuarios",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PerfilId",
                table: "Usuarios",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProvinciaId",
                table: "Usuarios",
                column: "ProvinciaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "Trabajos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "EstadosTrabajo");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "Perfiles");

            migrationBuilder.DropTable(
                name: "Provincias");
        }
    }
}
