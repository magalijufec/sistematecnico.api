using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponseDTO>> ObtenerTodosAsync();

        Task<UsuarioDetalleDTO> ObtenerPorIdAsync(int id);
        Task<UsuarioDetalleDTO> ObtenerPorIdActivoAsync(int id);

        Task CrearAsync(UsuarioDTO usuario);

        Task<bool> ActualizarAsync(int id, UsuarioActualizarDTO usuario);

        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<TecnicoComboDTO>> ObtenerTecnicosAsync();
    }
}
