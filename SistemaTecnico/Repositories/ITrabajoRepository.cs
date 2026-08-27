using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories;

public interface ITrabajoRepository
{
    Task<IEnumerable<Trabajo>> ObtenerTodosAsync();

    Task<IEnumerable<Trabajo>> ObtenerPorClienteAsync(int idCliente);

    Task<Trabajo?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Trabajo trabajo);

    Task ActualizarAsync(Trabajo trabajo);

    Task EliminarAsync(Trabajo trabajo);

    Task<bool> ExisteAsync(int id);

    Task GuardarCambiosAsync();

    Task<IEnumerable<Trabajo>> ObtenerPorEstadoAsync(int idEstado);

    Task<IEnumerable<Trabajo>> ObtenerPorTecnicoAsync(int idTecnico);

    Task<IEnumerable<Trabajo>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta);

    Task RegistrarPagoAsync(int idTrabajo);

    Task SubirFacturasAsync(int idTrabajo, IFormFile[] archivos, IWebHostEnvironment env);
}