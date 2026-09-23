using System.Security.Claims;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{ 
    public class IncidenciaService : IIncidenciaService
    {
        private readonly IIncidenciaRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IncidenciaService(IIncidenciaRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<IncidenciaPendienteDto>> ObtenerPendientesAsync()
        {
            var incidencias = await _repository.ObtenerPendientesAsync();

            return incidencias
                .Select(x => new IncidenciaPendienteDto
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Usuario = x.Usuario.NombreApellido,
                    Cliente = x.Cliente.NroCliente + " - " + x.Cliente.Nombre,
                    Asistencia = x.Asistencia.Nombre,
                    Incidencia = x.Tarea.Descripcion,
                    Otro = x.Otro,
                    Comentario = x.Comentario,
                    Guardia = x.Guardia
                })
                .ToList();
        }

        public async Task<List<IncidenciaFinalizadaDto>> ObtenerFinalizadasAsync()
        {
            var incidencias = await _repository.ObtenerFinalizadasAsync();

            return incidencias.Select(x =>
            {
                bool creadaYFinalizadaEnElMomento =
                    x.UsuarioId == x.UsuarioFinalizadoId
                    && x.FechaFinalizado.HasValue
                    && x.Fecha == x.FechaFinalizado.Value;

                return new IncidenciaFinalizadaDto
                {
                    Id = x.Id,

                    Fecha = x.Fecha,

                    Usuario = x.Usuario.NombreApellido,

                    UsuarioFinalizado =
                        creadaYFinalizadaEnElMomento
                        ? "-"
                        : x.UsuarioFinalizado?.NombreApellido ?? "-",

                    FechaFinalizado =
                        creadaYFinalizadaEnElMomento
                        ? null
                        : x.FechaFinalizado,

                    Cliente = x.Cliente.NroCliente + " - " + x.Cliente.Nombre,
                    Destino = x.Destino.Nombre,
                    Asistencia = x.Asistencia.Nombre,

                    Incidencia = x.Tarea.Descripcion,

                    Otro = x.Otro,

                    TrabajoRealizado =
                        x.TrabajoRealizado,

                    Guardia = x.Guardia
                };
            }).ToList();
        }

        public async Task CrearAsync(CrearIncidenciaDto dto)
        {
            var incidencia = new Incidencia
            {
                ClienteId = dto.ClienteId,
                DestinoId = dto.DestinoId,
                TareaId = dto.TareaId,
                AsistenciaId = dto.AsistenciaId,
                EstadoIncidenciaId = dto.EstadoIncidenciaId,
                Otro = dto.Otro,
                TrabajoRealizado = dto.TrabajoRealizado,
                UsuarioId = ObtenerUsuarioIdActual(),
                Fecha = DateTime.UtcNow,
                Guardia = dto.Guardia
            };

            // Finalizado
            if (dto.EstadoIncidenciaId == 2)
            {
                incidencia.UsuarioFinalizadoId = ObtenerUsuarioIdActual();
                incidencia.FechaFinalizado = DateTime.UtcNow;
            } else if (dto.EstadoIncidenciaId == 1)
            {
                incidencia.Comentario = dto.Comentario;
            }

            await _repository.CrearAsync(incidencia);
        }

        public async Task FinalizarAsync(int incidenciaId, string trabajoRealizado)
        {
            var incidencia = await _repository.ObtenerPorIdAsync(incidenciaId);

            if (incidencia == null) throw new Exception("Incidencia no encontrada");

            incidencia.EstadoIncidenciaId = 2; // Finalizado

            incidencia.UsuarioFinalizadoId = ObtenerUsuarioIdActual();

            incidencia.FechaFinalizado = DateTime.UtcNow;

            incidencia.TrabajoRealizado = trabajoRealizado;

            await _repository.ActualizarAsync(incidencia);
        }

        public async Task<CatalogosIncidenciaDto> ObtenerCatalogosAsync()
        {
            var asistencias =
                await _repository.ObtenerAsistenciasAsync();

            var destinos =
                await _repository.ObtenerDestinosAsync();

            var tareas =
                await _repository.ObtenerTareasSoporteAsync();

            return new CatalogosIncidenciaDto
            {
                Asistencias = asistencias
                    .Select(x => new ComboDTO
                    {
                        Id = x.Id,
                        Nombre = x.Nombre
                    })
                    .ToList(),

                Destinos = destinos
                    .Select(x => new ComboDTO
                    {
                        Id = x.Id,
                        Nombre = x.Nombre
                    })
                    .ToList(),

                Tareas = tareas
                    .Select(x => new ComboDTO
                    {
                        Id = x.Id,
                        Nombre = x.Descripcion
                    })
                    .ToList()
            };
        }

        private int ObtenerUsuarioIdActual()
        {
            var claim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null) throw new UnauthorizedAccessException("No se pudo identificar al usuario.");

            return int.Parse(claim.Value);
        }
    }
}