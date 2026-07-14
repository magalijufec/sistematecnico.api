using SistemaTecnico.DTO;

namespace SistemaTecnico.Services
{
    public interface IImagenService
    {
        Task<List<ImagenResponseDto>> ObtenerImagenesAsync(int idTrabajo);

        Task SubirImagenesAsync(int idTrabajo, List<IFormFile> archivos);

        Task EliminarImagenAsync(int idImagen);
    }
}
