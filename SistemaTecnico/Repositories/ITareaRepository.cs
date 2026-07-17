using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ITareaRepository
    {
        Task<IEnumerable<Tarea>> ObtenerTodasAsync();
        Task<Tarea> ObtenerPorIdAsync(int id);
    }
}
