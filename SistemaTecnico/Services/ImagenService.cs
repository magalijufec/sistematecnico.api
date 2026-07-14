using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class ImagenService : IImagenService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly IImagenRepository _imagenRepository;

        private readonly ITrabajoRepository _trabajoRepository;

        public ImagenService(
            IWebHostEnvironment environment,
            IImagenRepository imagenRepository,
            ITrabajoRepository trabajoRepository)
        {
            _environment = environment;
            _imagenRepository = imagenRepository;
            _trabajoRepository = trabajoRepository;
        }

        public async Task<List<ImagenResponseDto>> ObtenerImagenesAsync(int idTrabajo)
        {
            var imagenes =
                await _imagenRepository.ObtenerPorTrabajoAsync(idTrabajo);

            return imagenes.Select(i => new ImagenResponseDto
            {
                Id = i.Id,
                Tipo = i.Tipo,
                NombreArchivo = i.NombreArchivo,
                RutaArchivo = i.RutaArchivo,
                Extension = i.Extension,
                Tamanio = i.Tamanio,
                FechaCarga = i.FechaCarga
            }).ToList();
        }

        public async Task SubirImagenesAsync(
            int idTrabajo,
            List<IFormFile> archivos,
            string tipo)
        {
            if (!await _trabajoRepository.ExisteAsync(idTrabajo))
                throw new Exception("El trabajo no existe.");

            string carpetaTrabajo =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "trabajos",
                    idTrabajo.ToString());

            if (!Directory.Exists(carpetaTrabajo))
                Directory.CreateDirectory(carpetaTrabajo);

            foreach (var archivo in archivos)
            {
                var nombreArchivo =
                    $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";

                var rutaFisica =
                    Path.Combine(carpetaTrabajo, nombreArchivo);

                using var stream =
                    new FileStream(rutaFisica, FileMode.Create);

                await archivo.CopyToAsync(stream);

                var imagen = new Imagen
                {
                    Tipo = tipo,

                    NombreArchivo = nombreArchivo,

                    RutaArchivo =
                        Path.Combine(
                            "uploads",
                            "trabajos",
                            idTrabajo.ToString(),
                            nombreArchivo),

                    Extension =
                        Path.GetExtension(archivo.FileName),

                    Tamanio = archivo.Length,

                    FechaCarga = DateTime.Now
                };

                await _imagenRepository.AgregarAsync(imagen);
            }

            await _imagenRepository.GuardarCambiosAsync();
        }

        public async Task<bool> EliminarImagenAsync(int idImagen)
        {
            var imagen =
                await _imagenRepository.ObtenerPorIdAsync(idImagen);

            if (imagen == null)
                return false;

            string rutaFisica =
                Path.Combine(
                    _environment.WebRootPath,
                    imagen.RutaArchivo);

            if (File.Exists(rutaFisica))
                File.Delete(rutaFisica);

            await _imagenRepository.EliminarAsync(imagen);

            await _imagenRepository.GuardarCambiosAsync();

            return true;
        }

        public Task SubirImagenesAsync(int idTrabajo, List<IFormFile> archivos)
        {
            throw new NotImplementedException();
        }

        Task IImagenService.EliminarImagenAsync(int idImagen)
        {
            return EliminarImagenAsync(idImagen);
        }
    }
}