using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<bool> ExisteAsync(int id);
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Cliente>> ObtenerPorProvinciaCiudadAsync(
            int provinciaId,
            int ciudadId
        );
    }
}
