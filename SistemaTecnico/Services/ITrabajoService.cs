using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITrabajoService
    {
        Task<IEnumerable<TrabajoSolicitudDTO>> ObtenerSolicitudesDeTrabajoAsync();
        Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosPagadosAsync();
        Task<IEnumerable<TrabajoResponseDto>> ObtenerTrabajosNoFinalizadosAsync();
        Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosPendientesPagoAsync();
        Task<TrabajoResponseDto?> ObtenerPorIdAsync(int id);
        Task<TrabajoResponseDto> CrearAsync(TrabajoCreateDto dto);
        Task<bool> ActualizarAsync(int id, TrabajoUpdateDto dto);
        Task<bool> EliminarAsync(int id);
        Task SubirFacturasAsync(int idTrabajo, IFormFile[] archivos);
        Task<bool> PendienteAprobacionTrabajoAsync(int idTrabajo, TrabajoRealizadoDTO dto);
        Task<bool> AprobarTrabajoAsync(int idTrabajo);
        Task<bool> CambiarEstadoTrabajoAsync(int idTrabajo, bool aprobado);
        Task<bool> AsignarTecnicosAsync(int idTrabajo, List<int> tecnicosIds);
        Task<bool> CargarMaterialesAsync(int idTrabajo, string? materiales);
        Task<bool> MarcarMaterialesEnviadosAsync(int idTrabajo);
        Task<bool> MaterialesEnviadosAsync(int idTrabajo);
        Task<RegistrarPagoFacturaResponseDto> RegistrarPagoAsync(int idTrabajo, int idFactura);
        Task<bool> SolicitarMejoraAsync(int id, SolicitarMejoraDTO dto);
        Task<byte[]> GenerarInformePdfAsync(int id);
    }
}
