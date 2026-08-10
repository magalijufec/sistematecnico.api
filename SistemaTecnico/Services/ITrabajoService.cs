using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITrabajoService
    {
        Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosFinalizadosAsync();
        Task<IEnumerable<TrabajoResponseDto>> ObtenerTrabajosNoFinalizadosAsync();
        Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosPendientesPagoAsync();

        Task<TrabajoResponseDto?> ObtenerPorIdAsync(int id);

        Task<TrabajoResponseDto> CrearAsync(TrabajoCreateDto dto);

        Task<bool> ActualizarAsync(int id, TrabajoUpdateDto dto);

        Task<bool> EliminarAsync(int id);

        Task SubirFacturaAsync(int idTrabajo, IFormFile archivo);

        Task<bool> IniciarTrabajoAsync(int idTrabajo);

        Task<bool> FinalizarTrabajoAsync(int idTrabajo, TrabajoRealizadoDTO dto);

        Task<bool> AprobarTrabajoAsync(int idTrabajo);

        Task<bool> RegistrarPagoAsync(int idTrabajo);

        Task<bool> SolicitarMejoraAsync(int id, SolicitarMejoraDTO dto);
    }
}
