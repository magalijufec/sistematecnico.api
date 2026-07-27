using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IProvinciaRepository
    {
        Task<IEnumerable<Provincia>> ObtenerTodasAsync();

        Task<Provincia?> ObtenerPorIdAsync(int id);
    }
}
