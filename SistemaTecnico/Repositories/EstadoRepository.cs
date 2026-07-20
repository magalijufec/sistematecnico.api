using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly AppDbContext _context;

        public EstadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EstadoTrabajo?> ObtenerPorIdAsync(int id)
        {
            return await _context.EstadosTrabajo
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<EstadoTrabajo>> ObtenerTodosAsync()
        {
            return await _context.EstadosTrabajo
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<ComboDTO>> ObtenerEstadosSiguientes(int idTrabajo)
        {
            var trabajo = await _context.Trabajos
                .Include(x => x.Estado)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == idTrabajo);

            if (trabajo == null)
                return new List<ComboDTO>();

            int siguienteEstado = trabajo.Estado.Id switch
            {
                1 => 2,
                2 => 3,
                3 => 4,
                _ => 0
            };

            if (siguienteEstado == 0)
                return new List<ComboDTO>();

            return await _context.EstadosTrabajo
                .Where(x => x.Id == siguienteEstado)
                .Select(x => new ComboDTO
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                })
                .ToListAsync();
        }
    }
}
