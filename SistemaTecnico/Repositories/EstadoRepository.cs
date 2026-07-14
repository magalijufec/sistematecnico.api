using SistemaTecnico.Data;
using SistemaTecnico.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
