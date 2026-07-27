using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IProvinciaService
    {
        Task<IEnumerable<ComboDTO>> ObtenerComboAsync();

        Task<ComboDTO?> ObtenerPorIdAsync(int id);
    }
}
