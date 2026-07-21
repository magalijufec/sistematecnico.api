using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITrabajoService
    {
        Task<IEnumerable<TrabajoResponseDto>> ObtenerTodosAsync();

        Task<TrabajoResponseDto?> ObtenerPorIdAsync(int id);

        Task<TrabajoResponseDto> CrearAsync(TrabajoCreateDto dto);

        Task<bool> ActualizarAsync(int id, TrabajoUpdateDto dto);

        Task<bool> EliminarAsync(int id);

        Task CambiarEstadoAsync(int idTrabajo, CambiarEstadoDTO dto);

        Task GuardarTrabajoRealizado(int id, TrabajoRealizadoDTO dto);

        Task SubirFacturaAsync(int idTrabajo, IFormFile archivo);
        }
}
