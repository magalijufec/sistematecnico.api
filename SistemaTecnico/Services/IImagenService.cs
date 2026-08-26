using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public interface IImagenService
    {
        Task<List<ImagenResponseDto>> ObtenerImagenesAsync(int idTrabajo);

        Task<List<Imagen>> ObtenerPorTrabajo(int idTrabajo);

        Task SubirImagenes(int idTrabajo, List<IFormFile> archivos);

        Task EliminarImagenAsync(int idImagen);
    }
}
