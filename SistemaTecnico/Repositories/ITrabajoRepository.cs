using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories;

public interface ITrabajoRepository
{
    Task<IEnumerable<Trabajo>> ObtenerTodosAsync();

    Task<Trabajo?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Trabajo trabajo);

    Task ActualizarAsync(Trabajo trabajo);

    Task EliminarAsync(Trabajo trabajo);

    Task<bool> ExisteAsync(int id);

    Task GuardarCambiosAsync();

    Task<IEnumerable<Trabajo>> ObtenerPorEstadoAsync(int idEstado);

    Task<IEnumerable<Trabajo>> ObtenerPorTecnicoAsync(int idTecnico);

    Task<IEnumerable<Trabajo>> ObtenerPorFechasAsync(DateTime desde, DateTime hasta);

    Task CambiarEstadoAsync(int idTrabajo, EstadoTrabajo estado);

    Task GuardarTrabajoRealizado(int id, TrabajoRealizadoDTO dto);
    Task RegistrarPagoAsync(int idTrabajo);
}