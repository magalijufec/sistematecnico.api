using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IEstadoRepository
    {
        Task<bool> ExisteAsync(int id);
        Task<IEnumerable<EstadoTrabajo>> ObtenerTodosAsync();
        Task<EstadoTrabajo?> ObtenerPorIdAsync(int id);
        Task<List<ComboDTO>> ObtenerEstadosSiguientes(int idTrabajo);
    }
}
