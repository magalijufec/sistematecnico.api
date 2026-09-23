using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITareaService
    {
        Task<IEnumerable<ComboDTO>> ObtenerTodasAsync();
        Task<IEnumerable<ComboDTO>> ObtenerSoporte();
        Task<IEnumerable<ComboDTO>> ObtenerPorSectorAsync(int sectorId);
    }
}
