using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IImagenRepository
    {
        Task<List<Imagen>> ObtenerPorTrabajoAsync(int idTrabajo);
        Task<Imagen?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Imagen imagen);
        Task EliminarAsync(Imagen imagen);
        Task EliminarImagenAsync(int id);
        Task GuardarCambiosAsync();
    }
}
