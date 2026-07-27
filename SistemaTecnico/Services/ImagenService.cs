using Microsoft.EntityFrameworkCore;
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
            var imagenes = await _imagenRepository.ObtenerPorTrabajoAsync(idTrabajo);

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

        public async Task<List<Imagen>> ObtenerPorTrabajo(int idTrabajo)
        {
            return await _imagenRepository.ObtenerPorTrabajoAsync(idTrabajo);
        }

        public async Task SubirImagenes(
           int idTrabajo,
           bool antes,
           List<IFormFile> archivos)
        {
            if (archivos == null || archivos.Count == 0)
                throw new ArgumentException("No se recibieron archivos.");

            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
                throw new KeyNotFoundException(
                    $"No existe el trabajo con ID {idTrabajo}");

            string tipoCarpeta = antes
                ? "antes"
                : "despues";

            string carpeta = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "uploads",
                "trabajos",
                idTrabajo.ToString(),
                tipoCarpeta
            );

            Directory.CreateDirectory(carpeta);

            foreach (var archivo in archivos)
            {
                if (archivo == null || archivo.Length == 0)
                    continue;

                string extension = Path.GetExtension(archivo.FileName);
                string nombreArchivo = $"{DateTime.Now:yyMMdd_HHmmss}{extension}";

                string rutaFisica =
                    Path.Combine(
                        carpeta,
                        nombreArchivo
                    );

                using var stream =
                    new FileStream(
                        rutaFisica,
                        FileMode.Create
                    );

                await archivo.CopyToAsync(stream);

                var imagen = new Imagen
                {
                    TrabajoId = idTrabajo,
                    NombreArchivo = archivo.FileName,
                    Extension = extension,
                    Tipo = archivo.ContentType,
                    Tamanio = archivo.Length,
                    FechaCarga = DateTime.Now,
                    RutaArchivo =
                        $"/uploads/trabajos/" +
                        $"{idTrabajo}/" +
                        $"{tipoCarpeta}/" +
                        $"{nombreArchivo}"
                };

                await _imagenRepository.AgregarAsync(imagen);
            }

            await _imagenRepository.GuardarCambiosAsync();
        }

        public async Task EliminarImagenAsync(int idImagen)
        {
            await _imagenRepository.EliminarImagenAsync(idImagen);
            await _imagenRepository.GuardarCambiosAsync();
        }
    }
}