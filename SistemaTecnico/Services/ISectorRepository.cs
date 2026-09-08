using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ISectorService
    {
        Task<IEnumerable<ComboDTO>> ObtenerTodasAsync();
    }
}
