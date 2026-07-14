using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> ExisteAsync(int id);
        Task<Cliente?> ObtenerPorIdAsync(int id);
    }
}
