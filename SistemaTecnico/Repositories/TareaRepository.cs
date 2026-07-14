using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tarea> ObtenerPorIdAsync(int id)
        {
            return _context.Tareas
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
