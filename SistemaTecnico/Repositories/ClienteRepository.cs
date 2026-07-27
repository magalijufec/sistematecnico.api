using SistemaTecnico.Data;
using SistemaTecnico.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaTecnico.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes
                .Include(c => c.Provincia)
                .Include(c => c.Ciudad)
                .ToListAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Provincia)
                .Include(c => c.Ciudad)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Provincia)
                .Include(c => c.Ciudad)
                .AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cliente>> ObtenerPorProvinciaCiudadAsync(
            int provinciaId,
            int ciudadId)
        {
            return await _context.Clientes
                .Include(c => c.Provincia)
                .Include(c => c.Ciudad)
                .Where(x =>
                    x.ProvinciaId == provinciaId &&
                    x.CiudadId == ciudadId)
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }
    }
}
