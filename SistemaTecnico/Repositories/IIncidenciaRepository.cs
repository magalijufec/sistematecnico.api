using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories 
{ 
    public interface IIncidenciaRepository
    {
        Task<List<Incidencia>> ObtenerPendientesAsync();
        Task<List<Incidencia>> ObtenerFinalizadasAsync();
        Task<Incidencia?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Incidencia incidencia);
        Task ActualizarAsync(Incidencia incidencia);
        Task<List<Asistencia>> ObtenerAsistenciasAsync();
        Task<List<Destino>> ObtenerDestinosAsync();
        Task<List<Tarea>> ObtenerTareasSoporteAsync();

    }
}
