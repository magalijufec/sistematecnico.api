using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Models;

namespace SistemaTecnico.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Perfil> Perfiles => Set<Perfil>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Provincia> Provincias => Set<Provincia>();
        public DbSet<Ciudad> Ciudades => Set<Ciudad>();
        public DbSet<Sector> Sectores => Set<Sector>();
        public DbSet<Tarea> Tareas => Set<Tarea>();
        public DbSet<Trabajo> Trabajos => Set<Trabajo>();
        public DbSet<Imagen> Imagenes => Set<Imagen>();
        public DbSet<EstadoTrabajo> EstadosTrabajo => Set<EstadoTrabajo>();
        public DbSet<TrabajoImagenComparacion> TrabajoImagenComparaciones => Set<TrabajoImagenComparacion>();
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<TrabajoFactura> TrabajoFacturas { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }

        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<EstadoIncidencia> EstadosIncidencia { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Destino> Destinos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trabajo>()
                .HasOne(t => t.Tecnico)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trabajo>()
                .HasOne(t => t.UsuarioCreacion)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Provincia)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Ciudad)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TrabajoImagenComparacion>()
                .HasOne(x => x.Trabajo)
                .WithMany(x => x.ComparacionesImagenes)
                .HasForeignKey(x => x.TrabajoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrabajoImagenComparacion>()
                .HasOne(x => x.ImagenAntes)
                .WithMany()
                .HasForeignKey(x => x.ImagenAntesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrabajoImagenComparacion>()
                .HasOne(x => x.ImagenDespues)
                .WithMany()
                .HasForeignKey(x => x.ImagenDespuesId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trabajo>()
                .HasOne(x => x.Sector)
                .WithMany()
                .HasForeignKey(x => x.SectorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sector>()
                .HasOne(x => x.Perfil)
                .WithMany()
                .HasForeignKey(x => x.PerfilId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasOne(i => i.UsuarioFinalizado)
                .WithMany()
                .HasForeignKey(i => i.UsuarioFinalizadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incidencia>()
                .HasIndex(x => x.EstadoIncidenciaId);

            modelBuilder.Entity<Incidencia>()
                .HasIndex(x => x.ClienteId);

            modelBuilder.Entity<Incidencia>()
                .HasIndex(x => x.Fecha);

            modelBuilder.Entity<Incidencia>()
                .HasIndex(x => x.UsuarioId);

            modelBuilder.Entity<EstadoIncidencia>().HasData(
                new EstadoIncidencia { Id = 1, Nombre = "Pendiente" },
                new EstadoIncidencia { Id = 2, Nombre = "Finalizado" }
            );

            modelBuilder.Entity<Asistencia>().HasData(
                new Asistencia { Id = 1, Nombre = "Remota" },
                new Asistencia { Id = 2, Nombre = "Presencial" }
            );

            modelBuilder.Entity<Destino>().HasData(
                new Destino { Id = 1, Nombre = "Farmacia" },
                new Destino { Id = 2, Nombre = "Droguería" }
            );

        }
    }
}
