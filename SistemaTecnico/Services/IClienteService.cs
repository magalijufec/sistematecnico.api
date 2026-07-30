using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync();
        Task<IEnumerable<ClienteComboDto>> ObtenerComboAsync();

        Task<IEnumerable<ClienteComboDto>> ObtenerPorProvinciaCiudadAsync(
            int provinciaId,
            int ciudadId
        );
    }
}
