using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class SectorRepository : ISectorRepository
    {
        private readonly AppDbContext _context;

        public SectorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sector> ObtenerPorIdAsync(int id)
        {
            return _context.Sectores    
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Sector>> ObtenerTodasAsync()
        {
            return await _context.Sectores
                .ToListAsync();
        }
    }
}
