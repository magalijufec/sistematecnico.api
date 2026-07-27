using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface ITrabajoImagenComparacionRepository
    {
        Task<IEnumerable<TrabajoImagenComparacion>>
            ObtenerPorTrabajoAsync(int idTrabajo);

        Task<TrabajoImagenComparacion?>
            ObtenerPorIdAsync(int id);

        Task AgregarAsync(
            TrabajoImagenComparacion comparacion);

        Task GuardarCambiosAsync();

        Task EliminarAsync(
            TrabajoImagenComparacion comparacion);
    }
}
