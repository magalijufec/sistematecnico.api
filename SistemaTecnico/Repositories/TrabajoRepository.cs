using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaTecnico.Data;
using SistemaTecnico.DTO;
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
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(t => t.Facturas)
            .Include(t => t.SolicitudImagenes)
            .Include(t => t.ComparacionesImagenes)
            .Include(t => t.Sector)
            .Include(t => t.Tarea)
            .Include(t => t.Tecnico)
            .Include(t => t.UsuarioCreacion)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Trabajo>>ObtenerPorTecnicoAsync(int idTecnico)
    {
        var trabajos = await _context.Trabajos
                .Include(t => t.Cliente)
                .Include(t => t.Tecnico)
                .Include(t => t.Estado)
                .Include(t => t.Cliente.Provincia)
                .Include(t => t.Cliente.Ciudad)
                .Include(t => t.Facturas)
                .Include(t => t.SolicitudImagenes)
                .Include(t => t.Sector)
                .Include(t => t.Tarea)
                .AsNoTracking()
                .ToListAsync();

        return trabajos.Where(t => !string.IsNullOrWhiteSpace(t.TecnicosAsignados) &&        
            t.TecnicosAsignados.Split(',').Contains(idTecnico.ToString()));
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorClienteAsync(int idCliente)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(x => x.Facturas)
            .Include(t => t.Tarea)
            .Include(t => t.Sector)
            .Include(t => t.SolicitudImagenes)
            .Where(t => t.Cliente.Id == idCliente)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Trabajo?> ObtenerPorIdAsync(int id)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(t => t.ComparacionesImagenes)
            .Include(t => t.SolicitudImagenes)
            .Include(t => t.Sector)
            .Include(t => t.Tarea)
            .Include(x => x.Facturas)
            .Include(t => t.UsuarioCreacion)
            .AsNoTracking()
            .AsSplitQuery()
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

    public async Task RegistrarPagoAsync(int idTrabajo)
    {
        var trabajo = await _context.Trabajos.FindAsync(idTrabajo);

        if (trabajo == null)
            throw new KeyNotFoundException($"No existe el trabajo con ID {idTrabajo}");

        trabajo.FechaPagado = DateTime.UtcNow;
        trabajo.Estado = await _context.EstadosTrabajo.FindAsync(EstadosTrabajo.Finalizado); 
        await _context.SaveChangesAsync();
    }

    public async Task CambiarEstadoAsync(int idTrabajo, int idEstado)
    {
        var trabajo = await _context.Trabajos.FindAsync(idTrabajo);

        if (trabajo == null)
            throw new KeyNotFoundException($"No existe el trabajo con ID {idTrabajo}");

        trabajo.EstadoId = idEstado;
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarPresupuestoAsync(int id, PresupuestoDecisionDTO dto, int tecnicoId) 
    {     
        var trabajo = await _context.Trabajos.FindAsync(id);

        if (trabajo == null)
            throw new KeyNotFoundException($"No existe el trabajo con ID {id}");

        trabajo.Tecnico = await _context.Usuarios.FindAsync(tecnicoId);
        trabajo.EstadoId = EstadosTrabajo.PresupuestoAprobado;

        await ActualizarAsync(trabajo);
        await GuardarCambiosAsync();
    }

    public async Task SubirFacturasAsync(int idTrabajo, IFormFile[] archivos, IWebHostEnvironment env)
    {
        if (
            archivos == null ||
            archivos.Length == 0
        )
        {
            throw new ArgumentException(
                "No se recibió ningún archivo."
            );
        }

        var trabajo =
            await _context.Trabajos.FindAsync(
                idTrabajo
            );

        if (trabajo == null)
        {
            throw new KeyNotFoundException(
                $"No existe el trabajo con ID {idTrabajo}"
            );
        }

        string carpeta =
            Path.Combine(
                env.ContentRootPath,
                "wwwroot",
                "uploads",
                "facturas",
                idTrabajo.ToString()
            );

        Directory.CreateDirectory(carpeta);

        foreach (var archivo in archivos)
        {
            var extension =
                Path.GetExtension(
                    archivo.FileName
                )
                .ToLowerInvariant();

            var extensionesPermitidas =
                new[]
                {
                ".pdf"
                };

            if (
                !extensionesPermitidas.Contains(
                    extension
                )
            )
            {
                throw new ArgumentException(
                    $"El archivo {archivo.FileName} no es un PDF."
                );
            }

            string nombreArchivo =
                $"factura_{Guid.NewGuid()}{extension}";

            string rutaFisica =
                Path.Combine(
                    carpeta,
                    nombreArchivo
                );

            using var stream =
                new FileStream(
                    rutaFisica,
                    FileMode.Create
                );

            await archivo.CopyToAsync(stream);

            var factura =
                new TrabajoFactura
                {
                    TrabajoId = trabajo.Id,

                    RutaArchivo =
                        $"/uploads/facturas/" +
                        $"{idTrabajo}/" +
                        $"{nombreArchivo}",

                    FechaCarga =
                        DateTime.Now
                };

            _context.TrabajoFacturas.Add(factura);
        }

        trabajo.EstadoId =
            EstadosTrabajo.PendientePago;

        await _context.SaveChangesAsync();
    }
}