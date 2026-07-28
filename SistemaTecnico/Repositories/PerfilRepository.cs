using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly AppDbContext _context;

        public PerfilRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Perfil>> ObtenerTodos()
        {
            return await _context.Perfiles.ToListAsync();
        }

        public async Task<Perfil?> ObtenerPorIdAsync(int id)
        {
            return _context.Perfiles
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
