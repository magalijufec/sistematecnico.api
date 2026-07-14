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
    }
}
