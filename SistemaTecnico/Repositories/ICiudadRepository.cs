using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ICiudadRepository
    {
        Task<IEnumerable<Ciudad>> ObtenerTodasAsync();

        Task<IEnumerable<Ciudad>> ObtenerPorProvinciaAsync(
            int provinciaId);

        Task<Ciudad?> ObtenerPorIdAsync(int id);
    }
}
