using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories;

public class TrabajoRepository : ITrabajoRepository
{
    private readonly AppDbContext _context;

    public TrabajoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trabajo>> ObtenerTodosAsync()
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            //.Include(t => t.Sector)
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Trabajo?> ObtenerPorIdAsync(int id)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            //.Include(t => t.Sector)
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
            //.Include(t => t.HistorialEstados)
            //    .ThenInclude(h => h.Usuario)
            //.Include(t => t.HistorialEstados)
            // .ThenInclude(h => h.Estado)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AgregarAsync(Trabajo trabajo)
    {
        await _context.Trabajos.AddAsync(trabajo);
    }

    public Task ActualizarAsync(Trabajo trabajo)
    {
        _context.Trabajos.Update(trabajo);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Trabajo trabajo)
    {
        _context.Trabajos.Remove(trabajo);
        return Task.CompletedTask;
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Trabajos.AnyAsync(x => x.Id == id);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorEstadoAsync(int idEstado)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Where(t => t.Estado.Id == idEstado)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorTecnicoAsync(int idTecnico)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Estado)
            .Where(t => t.Tecnico.Id == idTecnico)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Estado)
            .Where(t => t.FechaSolicitud >= desde &&
                        t.FechaSolicitud <= hasta)
            .AsNoTracking()
            .ToListAsync();
    }
}