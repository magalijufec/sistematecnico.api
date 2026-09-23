using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tarea> ObtenerPorIdAsync(int id)
        {
            return _context.Tareas
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Tarea>> ObtenerTodasAsync()
        {
            return await _context.Tareas.Where(x => !x.Soporte)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tarea>> ObtenerSoporte()
        {
            return await _context.Tareas.Where(x => x.Soporte)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorSectorAsync(int sectorId)
        {
            return _context.Tareas
                .Where(x => x.SectorId == sectorId && !x.Soporte)
                .OrderBy(x => x.Descripcion)
                .ToList();
        }
    }
}
