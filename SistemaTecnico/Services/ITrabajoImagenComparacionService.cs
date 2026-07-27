using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface ITrabajoImagenComparacionService
    {
        Task<List<TrabajoImagenComparacionDTO>>
            ObtenerPorTrabajoAsync(int idTrabajo);

        Task<TrabajoImagenComparacionDTO>
            CrearAsync(int idTrabajo);

        Task SubirImagenAntesAsync(
            int idComparacion,
            IFormFile archivo);

        Task SubirImagenDespuesAsync(
            int idComparacion,
            IFormFile archivo);

        Task EliminarAsync(
            int idComparacion);
    }
}
