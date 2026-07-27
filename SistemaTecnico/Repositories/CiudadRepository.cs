using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class CiudadRepository : ICiudadRepository
    {
        private readonly AppDbContext _context;

        public CiudadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ciudad>> ObtenerTodasAsync()
        {
            return _context.Ciudades
                .Include( x => x.Provincia)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        public async Task<IEnumerable<Ciudad>> ObtenerPorProvinciaAsync(
            int provinciaId)
        {
            return _context.Ciudades
                .Where(x => x.Provincia.Id == provinciaId)
                .OrderBy(x => x.Nombre)
                .ToList();
        }

        public async Task<Ciudad?> ObtenerPorIdAsync(int id)
        {
            return _context.Ciudades
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
