using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ITareaRepository
    {
        Task<IEnumerable<Tarea>> ObtenerTodasAsync();
        Task<IEnumerable<Tarea>> ObtenerSoporte();
        Task<Tarea> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Tarea>> ObtenerPorSectorAsync(int sectorId);

    }
}
