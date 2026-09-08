using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaTecnico.Migrations
{
    /// <inheritdoc />
    public partial class sectorPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(
    MigrationBuilder migrationBuilder)
        {
            /*
             * Eliminar la FK anterior solamente si existe.
             */
            migrationBuilder.Sql("""
        IF EXISTS
        (
            SELECT 1
            FROM sys.foreign_keys
            WHERE name = N'FK_Sectores_Usuarios_UsuarioId'
              AND parent_object_id = OBJECT_ID(N'[dbo].[Sectores]')
        )
        BEGIN
            ALTER TABLE [dbo].[Sectores]
            DROP CONSTRAINT [FK_Sectores_Usuarios_UsuarioId];
        END
        """);

            /*
             * Eliminar el índice anterior solamente si existe.
             */
            migrationBuilder.Sql("""
        IF EXISTS
        (
            SELECT 1
            FROM sys.indexes
            WHERE name = N'IX_Sectores_UsuarioId'
              AND object_id = OBJECT_ID(N'[dbo].[Sectores]')
        )
        BEGIN
            DROP INDEX [IX_Sectores_UsuarioId]
            ON [dbo].[Sectores];
        END
        """);

            /*
             * Eliminar UsuarioId solamente si existe.
             */
            migrationBuilder.Sql("""
        IF COL_LENGTH(N'dbo.Sectores', N'UsuarioId') IS NOT NULL
        BEGIN
            ALTER TABLE [dbo].[Sectores]
            DROP COLUMN [UsuarioId];
        END
        """);

            /*
             * Agregar PerfilId temporalmente nullable.
             *
             * No usamos defaultValue: 0 porque podría no existir
             * un perfil con Id = 0.
             */
            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Sectores",
                type: "int",
                nullable: true);

            /*
             * Aquí debes relacionar los sectores existentes
             * con sus perfiles.
             *
             * Este script intenta relacionarlos por nombre:
             * Sector Mantenimiento -> Perfil Mantenimiento
             * Sector Monitoreo     -> Perfil Monitoreo
             * Sector Sistemas      -> Perfil Sistemas
             */
            migrationBuilder.Sql("""
        UPDATE s
        SET s.PerfilId = p.Id
        FROM [dbo].[Sectores] s
        INNER JOIN [dbo].[Perfiles] p
            ON LOWER(LTRIM(RTRIM(s.Nombre))) =
               LOWER(LTRIM(RTRIM(p.Nombre)))
        WHERE s.PerfilId IS NULL;
        """);

            /*
             * Detener la migración si algún sector no pudo
             * relacionarse con un perfil.
             */
            migrationBuilder.Sql("""
        IF EXISTS
        (
            SELECT 1
            FROM [dbo].[Sectores]
            WHERE PerfilId IS NULL
        )
        BEGIN
            THROW 50001,
            'Hay sectores sin perfil asociado. Verifique los nombres o asigne PerfilId manualmente.',
            1;
        END
        """);

            /*
             * Una vez asignados todos los perfiles,
             * PerfilId pasa a ser obligatorio.
             */
            migrationBuilder.AlterColumn<int>(
                name: "PerfilId",
                table: "Sectores",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectores_PerfilId",
                table: "Sectores",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sectores_Perfiles_PerfilId",
                table: "Sectores",
                column: "PerfilId",
                principalTable: "Perfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(
    MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sectores_Perfiles_PerfilId",
                table: "Sectores");

            migrationBuilder.DropIndex(
                name: "IX_Sectores_PerfilId",
                table: "Sectores");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Sectores");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Sectores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectores_UsuarioId",
                table: "Sectores",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sectores_Usuarios_UsuarioId",
                table: "Sectores",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
