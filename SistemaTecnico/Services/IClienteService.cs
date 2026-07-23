using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync();
        Task<IEnumerable<ComboDTO>> ObtenerComboAsync();
    }
}
