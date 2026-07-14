using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ITareaRepository
    {
        Task<Tarea> ObtenerPorIdAsync(int id);
    }
}
