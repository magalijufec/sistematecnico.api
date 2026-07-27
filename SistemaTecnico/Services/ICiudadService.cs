using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ICiudadService
    {
        Task<IEnumerable<ComboDTO>> ObtenerComboAsync();

        Task<IEnumerable<ComboDTO>> ObtenerPorProvinciaAsync(
            int provinciaId);

        Task<ComboDTO?> ObtenerPorIdAsync(int id);
    }
}
