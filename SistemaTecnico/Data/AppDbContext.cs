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

        public DbSet<Trabajo> Trabajos => Set<Trabajo>();

        public DbSet<Imagen> Imagenes => Set<Imagen>();

        public DbSet<EstadoTrabajo> EstadosTrabajo => Set<EstadoTrabajo>();

        //public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Provincia)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Ciudad)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
