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
    }
}
