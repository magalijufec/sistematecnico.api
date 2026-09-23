using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public interface IIncidenciaService
    {
        Task<List<IncidenciaPendienteDto>> ObtenerPendientesAsync();
        Task<List<IncidenciaFinalizadaDto>> ObtenerFinalizadasAsync();
        Task CrearAsync(CrearIncidenciaDto incidencia);
        Task FinalizarAsync(int incidenciaId, string trabajoRealizado);
        Task<CatalogosIncidenciaDto> ObtenerCatalogosAsync();
    }
}
