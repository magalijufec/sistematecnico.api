IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [EstadosTrabajo] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_EstadosTrabajo] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Perfiles] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Perfiles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Provincias] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Provincias] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Ciudades] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [IdProvincia] int NOT NULL,
        [ProvinciaId] int NOT NULL,
        CONSTRAINT [PK_Ciudades] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Ciudades_Provincias_ProvinciaId] FOREIGN KEY ([ProvinciaId]) REFERENCES [Provincias] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [NroCliente] nvarchar(max) NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [RazonSocial] nvarchar(max) NULL,
        [Direccion] nvarchar(max) NULL,
        [AddressShipToCode] nvarchar(max) NULL,
        [ProvinciaId] int NOT NULL,
        [CiudadId] int NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Clientes_Ciudades_CiudadId] FOREIGN KEY ([CiudadId]) REFERENCES [Ciudades] ([Id]),
        CONSTRAINT [FK_Clientes_Provincias_ProvinciaId] FOREIGN KEY ([ProvinciaId]) REFERENCES [Provincias] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] int NOT NULL IDENTITY,
        [UserName] nvarchar(max) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [NombreApellido] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NULL,
        [NumeroCelular] nvarchar(max) NULL,
        [Activo] bit NOT NULL,
        [PerfilId] int NOT NULL,
        [ProvinciaId] int NULL,
        [CiudadId] int NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Usuarios_Ciudades_CiudadId] FOREIGN KEY ([CiudadId]) REFERENCES [Ciudades] ([Id]),
        CONSTRAINT [FK_Usuarios_Perfiles_PerfilId] FOREIGN KEY ([PerfilId]) REFERENCES [Perfiles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Usuarios_Provincias_ProvinciaId] FOREIGN KEY ([ProvinciaId]) REFERENCES [Provincias] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Trabajos] (
        [Id] int NOT NULL IDENTITY,
        [FechaSolicitud] datetime2 NOT NULL,
        [FechaTrabajo] datetime2 NULL,
        [TecnicoId] int NOT NULL,
        [ClienteId] int NOT NULL,
        [Tarea] nvarchar(max) NOT NULL,
        [Comentarios] nvarchar(max) NULL,
        [TrabajoRealizado] nvarchar(max) NULL,
        [EstadoId] int NOT NULL,
        [Factura] nvarchar(max) NULL,
        [FechaPagado] datetime2 NULL,
        [FechaAlta] datetime2 NOT NULL,
        CONSTRAINT [PK_Trabajos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Trabajos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Trabajos_EstadosTrabajo_EstadoId] FOREIGN KEY ([EstadoId]) REFERENCES [EstadosTrabajo] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Trabajos_Usuarios_TecnicoId] FOREIGN KEY ([TecnicoId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE TABLE [Imagenes] (
        [Id] int NOT NULL IDENTITY,
        [TrabajoId] int NOT NULL,
        [Tipo] nvarchar(max) NOT NULL,
        [NombreArchivo] nvarchar(max) NOT NULL,
        [RutaArchivo] nvarchar(max) NOT NULL,
        [Extension] nvarchar(max) NULL,
        [Tamanio] bigint NOT NULL,
        [FechaCarga] datetime2 NOT NULL,
        CONSTRAINT [PK_Imagenes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Imagenes_Trabajos_TrabajoId] FOREIGN KEY ([TrabajoId]) REFERENCES [Trabajos] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Ciudades_ProvinciaId] ON [Ciudades] ([ProvinciaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Clientes_CiudadId] ON [Clientes] ([CiudadId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Clientes_ProvinciaId] ON [Clientes] ([ProvinciaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Imagenes_TrabajoId] ON [Imagenes] ([TrabajoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Trabajos_ClienteId] ON [Trabajos] ([ClienteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Trabajos_EstadoId] ON [Trabajos] ([EstadoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Trabajos_TecnicoId] ON [Trabajos] ([TecnicoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Usuarios_CiudadId] ON [Usuarios] ([CiudadId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Usuarios_PerfilId] ON [Usuarios] ([PerfilId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Usuarios_ProvinciaId] ON [Usuarios] ([ProvinciaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714115804_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260714115804_InitialCreate', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714121625_EliminoCampoEnCiudad'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260714121625_EliminoCampoEnCiudad', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trabajos]') AND [c].[name] = N'Tarea');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Trabajos] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Trabajos] DROP COLUMN [Tarea];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    ALTER TABLE [Trabajos] ADD [TareaId] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    CREATE TABLE [Tareas] (
        [Id] int NOT NULL IDENTITY,
        [Descripcion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Tareas] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    CREATE INDEX [IX_Trabajos_TareaId] ON [Trabajos] ([TareaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    ALTER TABLE [Trabajos] ADD CONSTRAINT [FK_Trabajos_Tareas_TareaId] FOREIGN KEY ([TareaId]) REFERENCES [Tareas] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260714201410_CampoTareaComoInt'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260714201410_CampoTareaComoInt', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716142626_colorEstadoTrabajo'
)
BEGIN
    ALTER TABLE [EstadosTrabajo] ADD [Color] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260716142626_colorEstadoTrabajo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260716142626_colorEstadoTrabajo', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260720142758_AgregarImagen'
)
BEGIN
    ALTER TABLE [Imagenes] ADD [EsAntes] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260720142758_AgregarImagen'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260720142758_AgregarImagen', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724121544_CambioCampoFechasTrabajo'
)
BEGIN
    EXEC sp_rename N'[Trabajos].[FechaTrabajo]', N'FechaFinalizado', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724121544_CambioCampoFechasTrabajo'
)
BEGIN
    EXEC sp_rename N'[Trabajos].[FechaAlta]', N'FechaInicio', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724121544_CambioCampoFechasTrabajo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260724121544_CambioCampoFechasTrabajo', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724124224_FechaInicioNulleableTrabajo'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trabajos]') AND [c].[name] = N'FechaInicio');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Trabajos] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Trabajos] ALTER COLUMN [FechaInicio] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724124224_FechaInicioNulleableTrabajo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260724124224_FechaInicioNulleableTrabajo', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724213908_campoClienteAUsuario'
)
BEGIN
    ALTER TABLE [Usuarios] ADD [ClienteId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724213908_campoClienteAUsuario'
)
BEGIN
    CREATE INDEX [IX_Usuarios_ClienteId] ON [Usuarios] ([ClienteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724213908_campoClienteAUsuario'
)
BEGIN
    ALTER TABLE [Usuarios] ADD CONSTRAINT [FK_Usuarios_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260724213908_campoClienteAUsuario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260724213908_campoClienteAUsuario', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Imagenes]') AND [c].[name] = N'EsAntes');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Imagenes] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Imagenes] DROP COLUMN [EsAntes];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    CREATE TABLE [TrabajoImagenComparaciones] (
        [Id] int NOT NULL IDENTITY,
        [TrabajoId] int NOT NULL,
        [ImagenAntesId] int NULL,
        [ImagenDespuesId] int NULL,
        CONSTRAINT [PK_TrabajoImagenComparaciones] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TrabajoImagenComparaciones_Imagenes_ImagenAntesId] FOREIGN KEY ([ImagenAntesId]) REFERENCES [Imagenes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TrabajoImagenComparaciones_Imagenes_ImagenDespuesId] FOREIGN KEY ([ImagenDespuesId]) REFERENCES [Imagenes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TrabajoImagenComparaciones_Trabajos_TrabajoId] FOREIGN KEY ([TrabajoId]) REFERENCES [Trabajos] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    CREATE INDEX [IX_TrabajoImagenComparaciones_ImagenAntesId] ON [TrabajoImagenComparaciones] ([ImagenAntesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    CREATE INDEX [IX_TrabajoImagenComparaciones_ImagenDespuesId] ON [TrabajoImagenComparaciones] ([ImagenDespuesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    CREATE INDEX [IX_TrabajoImagenComparaciones_TrabajoId] ON [TrabajoImagenComparaciones] ([TrabajoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260727204747_cambioModeloImagenComparacion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260727204747_cambioModeloImagenComparacion', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803114628_addCampoEmailAClientes'
)
BEGIN
    ALTER TABLE [Clientes] ADD [Email] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803114628_addCampoEmailAClientes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260803114628_addCampoEmailAClientes', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803153037_addCampoActivoClientes'
)
BEGIN
    ALTER TABLE [Clientes] ADD [Activo] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260803153037_addCampoActivoClientes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260803153037_addCampoActivoClientes', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    ALTER TABLE [Trabajos] DROP CONSTRAINT [FK_Trabajos_Usuarios_TecnicoId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    ALTER TABLE [Trabajos] ADD [UsuarioCreacionId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    CREATE INDEX [IX_Trabajos_UsuarioCreacionId] ON [Trabajos] ([UsuarioCreacionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    ALTER TABLE [Trabajos] ADD CONSTRAINT [FK_Trabajos_Usuarios_TecnicoId] FOREIGN KEY ([TecnicoId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    ALTER TABLE [Trabajos] ADD CONSTRAINT [FK_Trabajos_Usuarios_UsuarioCreacionId] FOREIGN KEY ([UsuarioCreacionId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131527_addUsuarioCreacionTablaTrabajo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805131527_addUsuarioCreacionTablaTrabajo', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131713_updateUsuarioCreacionNotNull'
)
BEGIN
    DROP INDEX [IX_Trabajos_UsuarioCreacionId] ON [Trabajos];
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trabajos]') AND [c].[name] = N'UsuarioCreacionId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Trabajos] DROP CONSTRAINT ' + @var3 + ';');
    EXEC(N'UPDATE [Trabajos] SET [UsuarioCreacionId] = 0 WHERE [UsuarioCreacionId] IS NULL');
    ALTER TABLE [Trabajos] ALTER COLUMN [UsuarioCreacionId] int NOT NULL;
    ALTER TABLE [Trabajos] ADD DEFAULT 0 FOR [UsuarioCreacionId];
    CREATE INDEX [IX_Trabajos_UsuarioCreacionId] ON [Trabajos] ([UsuarioCreacionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260805131713_updateUsuarioCreacionNotNull'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260805131713_updateUsuarioCreacionNotNull', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821135011_CrearTablaErrorLogs'
)
BEGIN
    CREATE TABLE [ErrorLogs] (
        [Id] int NOT NULL IDENTITY,
        [Fecha] datetime2 NOT NULL,
        [Mensaje] nvarchar(max) NULL,
        [StackTrace] nvarchar(max) NULL,
        [InnerException] nvarchar(max) NULL,
        [Endpoint] nvarchar(max) NULL,
        [Metodo] nvarchar(max) NULL,
        [Usuario] nvarchar(max) NULL,
        [Ip] nvarchar(max) NULL,
        CONSTRAINT [PK_ErrorLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260821135011_CrearTablaErrorLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260821135011_CrearTablaErrorLogs', N'10.0.9');
END;

COMMIT;
GO

