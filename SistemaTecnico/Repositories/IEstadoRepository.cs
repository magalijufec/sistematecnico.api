using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IEstadoRepository
    {
        Task<bool> ExisteAsync(int id);
        Task<EstadoTrabajo?> ObtenerPorIdAsync(int id);
    }
}
