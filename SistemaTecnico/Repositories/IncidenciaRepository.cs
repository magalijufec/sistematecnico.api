using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{   

    public class IncidenciaRepository : IIncidenciaRepository
    {
        private readonly AppDbContext _context;

        public IncidenciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Incidencia>> ObtenerPendientesAsync()
        {
            return await _context.Incidencias
                .Include(x => x.Usuario)
                .Include(x => x.Cliente)
                .Include(x => x.Asistencia)
                .Include(x => x.Destino)
                .Include(x => x.Tarea)
                .Where(x => x.EstadoIncidenciaId == 1) // Pendiente
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<List<Incidencia>> ObtenerFinalizadasAsync()
        {
            return await _context.Incidencias
                .Include(x => x.Usuario)
                .Include(x => x.UsuarioFinalizado)
                .Include(x => x.Cliente)
                .Include(x => x.Asistencia)
                .Include(x => x.Destino)
                .Include(x => x.Tarea)
                .Include(x => x.EstadoIncidencia)
                .Where(x => x.EstadoIncidenciaId == 2)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<Incidencia?> ObtenerPorIdAsync(int id)
        {
            return await _context.Incidencias
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CrearAsync(Incidencia incidencia)
        {
            await _context.Incidencias.AddAsync(incidencia);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Incidencia incidencia)
        {
            _context.Incidencias.Update(incidencia);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Asistencia>> ObtenerAsistenciasAsync()
        {
            return await _context.Asistencias
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<Destino>> ObtenerDestinosAsync()
        {
            return await _context.Destinos
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        public async Task<List<Tarea>> ObtenerTareasSoporteAsync()
        {
            return await _context.Tareas
                .Where(x => x.Soporte)
                .OrderBy(x => x.Descripcion)
                .ToListAsync();
        }
    }
}
