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
                .AnyAsync(c => c.Id == id);
        }
    }
}
