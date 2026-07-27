using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class ProvinciaRepository : IProvinciaRepository
    {
        private readonly AppDbContext _context;

        public ProvinciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Provincia>> ObtenerTodasAsync()
        {
            return _context.Provincias
                .OrderBy(x => x.Nombre).ToList();
        }

        public async Task<Provincia?> ObtenerPorIdAsync(int id)
        {
            return _context.Provincias
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
