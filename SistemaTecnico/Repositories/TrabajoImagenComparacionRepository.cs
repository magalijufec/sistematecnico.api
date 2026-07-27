using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class TrabajoImagenComparacionRepository : ITrabajoImagenComparacionRepository
    {
        private readonly AppDbContext _context;

        public TrabajoImagenComparacionRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrabajoImagenComparacion>>ObtenerPorTrabajoAsync(int idTrabajo)
        {
            return await _context
                .TrabajoImagenComparaciones
                .Include(x => x.ImagenAntes)
                .Include(x => x.ImagenDespues)
                .Where(x => x.TrabajoId == idTrabajo)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<TrabajoImagenComparacion?>
            ObtenerPorIdAsync(int id)
        {
            return await _context
                .TrabajoImagenComparaciones
                .Include(x => x.ImagenAntes)
                .Include(x => x.ImagenDespues)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AgregarAsync(
            TrabajoImagenComparacion comparacion)
        {
            await _context
                .TrabajoImagenComparaciones
                .AddAsync(comparacion);
        }

        public async Task EliminarAsync(
            TrabajoImagenComparacion comparacion)
        {
            _context
                .TrabajoImagenComparaciones
                .Remove(comparacion);

            await Task.CompletedTask;
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
