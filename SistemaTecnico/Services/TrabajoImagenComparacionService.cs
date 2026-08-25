using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class TrabajoImagenComparacionService : ITrabajoImagenComparacionService
    {
        private readonly IWebHostEnvironment _environment;

        private readonly ITrabajoImagenComparacionRepository
            _comparacionRepository;

        private readonly IImagenRepository
            _imagenRepository;

        private readonly ITrabajoRepository
            _trabajoRepository;

        public TrabajoImagenComparacionService(
            IWebHostEnvironment environment,
            ITrabajoImagenComparacionRepository comparacionRepository,
            IImagenRepository imagenRepository,
            ITrabajoRepository trabajoRepository)
        {
            _environment = environment;
            _comparacionRepository = comparacionRepository;
            _imagenRepository = imagenRepository;
            _trabajoRepository = trabajoRepository;
        }

        public async Task<List<TrabajoImagenComparacionDTO>>
            ObtenerPorTrabajoAsync(int idTrabajo)
        {
            var comparaciones =
                await _comparacionRepository
                    .ObtenerPorTrabajoAsync(idTrabajo);

            return comparaciones
                .Select(x => new TrabajoImagenComparacionDTO
                {
                    Id = x.Id,

                    TrabajoId = x.TrabajoId,

                    ImagenAntes =
                        x.ImagenAntes == null
                            ? null
                            : MapearImagen(x.ImagenAntes),

                    ImagenDespues =
                        x.ImagenDespues == null
                            ? null
                            : MapearImagen(x.ImagenDespues)

                })
                .ToList();
        }

        public async Task<TrabajoImagenComparacionDTO>
            CrearAsync(int idTrabajo)
        {
            var trabajo =
                await _trabajoRepository
                    .ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
            {
                throw new KeyNotFoundException(
                    $"No existe el trabajo con ID {idTrabajo}");
            }

            var comparacion =
                new TrabajoImagenComparacion
                {
                    TrabajoId = idTrabajo
                };

            await _comparacionRepository
                .AgregarAsync(comparacion);

            await _comparacionRepository
                .GuardarCambiosAsync();

            return new TrabajoImagenComparacionDTO
            {
                Id = comparacion.Id,

                TrabajoId = comparacion.TrabajoId
            };
        }

        public async Task SubirImagenAntesAsync(
            int idComparacion,
            IFormFile archivo)
        {
            await SubirImagenAsync(
                idComparacion,
                archivo,
                true);
        }

        public async Task SubirImagenDespuesAsync(
            int idComparacion,
            IFormFile archivo)
        {
            await SubirImagenAsync(
                idComparacion,
                archivo,
                false);
        }

        private async Task SubirImagenAsync(
            int idComparacion,
            IFormFile archivo,
            bool esAntes)
        {
            if (archivo == null ||
                archivo.Length == 0)
            {
                throw new ArgumentException(
                    "No se recibió ningún archivo.");
            }

            var comparacion =
                await _comparacionRepository
                    .ObtenerPorIdAsync(idComparacion);

            if (comparacion == null)
            {
                throw new KeyNotFoundException(
                    $"No existe la comparación con ID {idComparacion}");
            }

            var idTrabajo =
                comparacion.TrabajoId;

            string tipoCarpeta =
                esAntes
                    ? "antes"
                    : "despues";

            string carpeta =
                Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot",
                    "uploads",
                    "trabajos",
                    idTrabajo.ToString(),
                    "comparaciones",
                    idComparacion.ToString(),
                    tipoCarpeta
                );

            Directory.CreateDirectory(carpeta);

            string extension =
                Path.GetExtension(
                    archivo.FileName);

            string nombreArchivo =
                $"{Guid.NewGuid()}{extension}";

            string rutaFisica =
                Path.Combine(
                    carpeta,
                    nombreArchivo);

            using var stream =
                new FileStream(
                    rutaFisica,
                    FileMode.Create);

            await archivo.CopyToAsync(stream);

            var imagen = new Imagen
            {
                TrabajoId = idTrabajo,

                NombreArchivo =
                    archivo.FileName,

                Extension =
                    extension,

                Tipo =
                    archivo.ContentType,

                Tamanio =
                    archivo.Length,

                FechaCarga =
                    DateTime.UtcNow,

                RutaArchivo =
                    $"/uploads/trabajos/" +
                    $"{idTrabajo}/" +
                    $"comparaciones/" +
                    $"{idComparacion}/" +
                    $"{tipoCarpeta}/" +
                    $"{nombreArchivo}"
            };

            await _imagenRepository
                .AgregarAsync(imagen);

            await _imagenRepository
                .GuardarCambiosAsync();

            if (esAntes)
            {
                comparacion.ImagenAntes =
                    imagen;

                comparacion.ImagenAntesId =
                    imagen.Id;
            }
            else
            {
                comparacion.ImagenDespues =
                    imagen;

                comparacion.ImagenDespuesId =
                    imagen.Id;
            }

            await _comparacionRepository
                .GuardarCambiosAsync();
        }

        public async Task EliminarAsync(
            int idComparacion)
        {
            var comparacion =
                await _comparacionRepository
                    .ObtenerPorIdAsync(
                        idComparacion);

            if (comparacion == null)
            {
                throw new KeyNotFoundException(
                    $"No existe la comparación con ID {idComparacion}");
            }

            await _comparacionRepository
                .EliminarAsync(comparacion);

            await _comparacionRepository
                .GuardarCambiosAsync();
        }

        private static ImagenResponseDto
            MapearImagen(Imagen imagen)
        {
            return new ImagenResponseDto
            {
                Id = imagen.Id,

                Tipo = imagen.Tipo,

                NombreArchivo =
                    imagen.NombreArchivo,

                RutaArchivo =
                    imagen.RutaArchivo,

                Extension =
                    imagen.Extension,

                Tamanio =
                    imagen.Tamanio,

                FechaCarga =
                    imagen.FechaCarga
            };
        }
    }
}
