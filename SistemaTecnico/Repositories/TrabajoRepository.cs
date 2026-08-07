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

    public TrabajoRepository(
            AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trabajo>> ObtenerTodosAsync()
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorTecnicoAsync(
        int idTecnico)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
            .Where(t => t.Tecnico.Id == idTecnico)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Trabajo>> ObtenerPorClienteAsync(
        int idCliente)
    {
        return await _context.Trabajos
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .Include(t => t.Estado)
            .Include(t => t.Cliente.Provincia)
            .Include(t => t.Cliente.Ciudad)
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
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
            .Include(t => t.Imagenes)
            .Include(t => t.Tarea)
            .Include(t => t.UsuarioCreacion)
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

    //public async Task CambiarEstadoAsync(int idTrabajo, EstadoTrabajo estado)
    //{
    //    var trabajo = await _context.Trabajos.FindAsync(idTrabajo);

    //    if (trabajo == null)
    //        throw new Exception("Trabajo no encontrado");

    //    trabajo.Estado = estado;

    //    await _context.SaveChangesAsync();
    //}

    public async Task GuardarTrabajoRealizado(
            int id,
            TrabajoRealizadoDTO dto)
    {
        var trabajo = await _context.Trabajos.FindAsync(id);

        if (trabajo == null)
            throw new Exception("Trabajo no encontrado");

        trabajo.TrabajoRealizado = dto.TrabajoRealizado;

        await _context.SaveChangesAsync();
    }

    public async Task RegistrarPagoAsync(int idTrabajo)
    {
        var trabajo = await _context.Trabajos.FindAsync(idTrabajo);
        if (trabajo == null)
            throw new KeyNotFoundException(
                $"No existe el trabajo con ID {idTrabajo}");

        trabajo.FechaPagado = DateTime.Now;
        var estado = await _context.EstadosTrabajo.FindAsync(4); // Finalizado
        trabajo.Estado = estado;

        await _context.SaveChangesAsync();
    }

    public async Task SubirFacturaAsync(int idTrabajo, IFormFile archivo, IWebHostEnvironment env)
    {
        if (archivo == null || archivo.Length == 0)
        {
            throw new ArgumentException("No se recibió ningún archivo.");
        }

        var trabajo = await _context.Trabajos.FindAsync(idTrabajo);

        if (trabajo == null)
        {
            throw new KeyNotFoundException($"No existe el trabajo con ID {idTrabajo}");
        }

        // Validar extensión
        var extension =
            Path.GetExtension(archivo.FileName)
            .ToLowerInvariant();

        var extensionesPermitidas = new[]
        {
                ".pdf",
                ".jpg",
                ".jpeg",
                ".png"
            };

        if (!extensionesPermitidas.Contains(extension))
        {
            throw new ArgumentException(
                "El archivo debe ser PDF, JPG, JPEG o PNG.");
        }

        // Carpeta
        string carpeta = Path.Combine(
            env.ContentRootPath,
            "wwwroot",
            "uploads",
            "facturas",
            idTrabajo.ToString()
        );

        Directory.CreateDirectory(carpeta);

        // Si ya existe una factura, eliminarla
        if (!string.IsNullOrEmpty(trabajo.Factura))
        {
            string rutaAnterior =
                Path.Combine(
                    env.ContentRootPath,
                    "wwwroot",
                    trabajo.Factura.TrimStart('/')
                        .Replace("/", Path.DirectorySeparatorChar.ToString())
                );

            if (File.Exists(rutaAnterior))
            {
                File.Delete(rutaAnterior);
            }
        }

        // Nombre nuevo
        string nombreArchivo =
            $"factura{DateTime.Now:yyMMddHHmmss}{extension}";

        string rutaFisica =
            Path.Combine(
                carpeta,
                nombreArchivo
            );

        // Guardar archivo
        using var stream =
            new FileStream(
                rutaFisica,
                FileMode.Create
            );

        await archivo.CopyToAsync(stream);

        // Ruta que se guarda en DB
        trabajo.Factura =
            $"/uploads/facturas/" +
            $"{idTrabajo}/" +
            $"{nombreArchivo}";

        await _context.SaveChangesAsync();
    }
}