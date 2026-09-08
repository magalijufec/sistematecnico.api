using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly AppDbContext _context;

        public PresupuestoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Presupuesto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Presupuestos
                .Include(x => x.Trabajo)
                .FirstOrDefaultAsync(
                    x => x.Id == id
                );
        }

        public async Task<List<Presupuesto>> ObtenerPorTrabajoAsync(int trabajoId)
        {
            return await _context.Presupuestos
                .Include(x => x.Tecnico)
                .Include(x => x.Estado)
                .Where(x => x.TrabajoId == trabajoId)
                .OrderByDescending(x => x.FechaCarga)
                .ToListAsync();
        }

        public async Task<Presupuesto> ObtenerPorTrabajoYTecnicoAsync(int trabajoId, int tecnicoId)
        {
            return await _context.Presupuestos
                .Include(x => x.Tecnico)
                .Include(x => x.Estado)
                .Where(x => x.TrabajoId == trabajoId && x.TecnicoId == tecnicoId)
                .OrderByDescending(x => x.FechaCarga)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Presupuesto>> ObtenerPorTecnicoAsync(int tecnicoId)
        {
            return await _context.Presupuestos
                .Include(x => x.Tecnico)
                .Include(x => x.Estado)
                .Include(x => x.Trabajo)
                .Where(x => x.TecnicoId == tecnicoId)
                .OrderByDescending(x => x.FechaCarga)
                .ToListAsync();
        }

        public async Task AgregarAsync(Presupuesto presupuesto)
        {
            await _context.Presupuestos.AddAsync(presupuesto);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
