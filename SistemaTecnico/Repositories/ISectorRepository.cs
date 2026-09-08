using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ISectorRepository
    {
        Task<IEnumerable<Sector>> ObtenerTodasAsync();
        Task<Sector> ObtenerPorIdAsync(int id);

    }
}
