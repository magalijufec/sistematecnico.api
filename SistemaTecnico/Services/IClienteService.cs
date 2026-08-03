using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync();
        Task<ClienteResponseDTO?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ClienteComboDto>> ObtenerComboAsync();
        Task<ClienteResponseDTO> CrearAsync(ClienteDTO dto);
        Task<bool> ActualizarAsync(
            int id,
            ClienteDTO dto);

        Task<IEnumerable<ClienteComboDto>> ObtenerPorProvinciaCiudadAsync(
            int provinciaId,
            int ciudadId
        );
    }
}
