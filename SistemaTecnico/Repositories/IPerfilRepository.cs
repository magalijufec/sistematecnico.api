using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IPerfilRepository
    {
        Task<List<Perfil>> ObtenerTodos();
        Task<Perfil?> ObtenerPorIdAsync(int id);
    }
}
