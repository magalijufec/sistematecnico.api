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

        //public DbSet<Empresa> Empresas => Set<Empresa>();

        //public DbSet<Sector> Sectores => Set<Sector>();
        public DbSet<Tarea> Tareas => Set<Tarea>();

        public DbSet<Trabajo> Trabajos => Set<Trabajo>();

        public DbSet<Imagen> Imagenes => Set<Imagen>();

        public DbSet<EstadoTrabajo> EstadosTrabajo => Set<EstadoTrabajo>();
        public DbSet<TrabajoImagenComparacion> TrabajoImagenComparaciones => Set<TrabajoImagenComparacion>();
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<TrabajoFactura> TrabajoFacturas { get; set; }

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
        }
    }
}
