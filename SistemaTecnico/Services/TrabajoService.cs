using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class TrabajoService : ITrabajoService
    {
        private readonly ITrabajoRepository _trabajoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEstadoRepository _estadoRepository;
        private readonly ITareaRepository _tareaRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TrabajoService(
            ITrabajoRepository trabajoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            IEstadoRepository estadoRepository,
            ITareaRepository tareaRepository,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
            _tareaRepository = tareaRepository;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObtenerUsuarioIdActual()
        {
            var claim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException(
                    "No se pudo identificar al usuario."
                );

            return int.Parse(claim.Value);
        }

        private string ObtenerRolActual()
        {
            var claim = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Role);

            if (claim == null)
                throw new UnauthorizedAccessException(
                    "No se pudo identificar el rol del usuario."
                );

            return claim.Value;
        }

        public async Task<IEnumerable<TrabajoResponseDto>>
            ObtenerTrabajosNoFinalizadosAsync()
        {
            var trabajos = await ObtenerTrabajosSegunUsuarioAsync();

            return trabajos
                .Where(t => t.Estado.Id != 4)
                .Select(t => new TrabajoResponseDto
                {
                    Id = t.Id,
                    FechaSolicitud = t.FechaSolicitud,
                    FechaInicio = t.FechaInicio,

                    IdEstado = t.Estado.Id,
                    Estado = t.Estado.Nombre,
                    EstadoColor = t.Estado.Color,

                    IdCliente = t.Cliente.Id,
                    Cliente = t.Cliente.NroCliente + " - " + t.Cliente.Nombre,

                    IdTecnico = t.Tecnico.Id,
                    Tecnico = t.Tecnico.NombreApellido,

                    IdTarea = t.Tarea.Id,
                    Tarea = t.Tarea.Descripcion,

                    TrabajoRealizado = t.TrabajoRealizado,

                    Provincia = t.Cliente.Provincia.Nombre,
                    Ciudad = t.Cliente.Ciudad.Nombre,

                    TieneFactura = !string.IsNullOrEmpty(t.Factura),

                    CantidadImagenes = t.Imagenes.Count

                })
                .OrderByDescending(t => t.FechaSolicitud);
        }

        private async Task<IEnumerable<Trabajo>>
    ObtenerTrabajosSegunUsuarioAsync()
        {
            var usuarioId = ObtenerUsuarioIdActual();
            var rol = ObtenerRolActual();

            // ADMINISTRADOR Y SISTEMAS
            // Pueden ver todos
            if (rol == "Administrador" ||
                rol == "Sistemas")
            {
                return await _trabajoRepository
                    .ObtenerTodosAsync();
            }

            // TÉCNICO
            // Solo ve sus propios trabajos
            if (rol == "Tecnico")
            {
                return await _trabajoRepository
                    .ObtenerPorTecnicoAsync(usuarioId);
            }

            // FARMACIA
            // Solo ve los trabajos de su cliente
            if (rol == "Farmacia")
            {
                var usuario = await _usuarioRepository
                    .ObtenerPorIdAsync(usuarioId);

                if (usuario == null ||
                    usuario.Cliente.Id == null)
                {
                    return Enumerable.Empty<Trabajo>();
                }

                return await _trabajoRepository
                    .ObtenerPorClienteAsync(
                        usuario.Cliente.Id
                    );
            }

            // PAGOS
            // Por ahora todos
            // Después podemos crear un filtro específico
            if (rol == "Pagos")
            {
                return await _trabajoRepository
                    .ObtenerTodosAsync();
            }

            return Enumerable.Empty<Trabajo>();
        }

        public async Task<IEnumerable<TrabajoFinalizadoDTO>>
            ObtenerTrabajosPendientesPagoAsync()
        {
            var trabajos =
                await ObtenerTrabajosSegunUsuarioAsync();

            return trabajos
                .Where(t => t.Estado.Id == 3)
                .Select(t => new TrabajoFinalizadoDTO
                {
                    Id = t.Id,

                    FechaSolicitud = t.FechaSolicitud,

                    FechaInicio = t.FechaInicio,

                    FechaFinalizado = t.FechaFinalizado,

                    FechaPagado = t.FechaPagado,

                    IdCliente = t.Cliente.Id,

                    Cliente =
                        t.Cliente.NroCliente +
                        " - " +
                        t.Cliente.Nombre,

                    IdTecnico = t.Tecnico.Id,

                    Tecnico =
                        t.Tecnico.NombreApellido,

                    IdTarea = t.Tarea.Id,

                    Tarea =
                        t.Tarea.Descripcion,

                    TrabajoRealizado =
                        t.TrabajoRealizado,

                    Provincia =
                        t.Cliente.Provincia.Nombre,

                    Ciudad =
                        t.Cliente.Ciudad.Nombre

                })
                .OrderByDescending(
                    t => t.FechaFinalizado
                );
        }
        public async Task<IEnumerable<TrabajoFinalizadoDTO>>
            ObtenerTrabajosFinalizadosAsync()
        {
            var trabajos =
                await ObtenerTrabajosSegunUsuarioAsync();

            return trabajos
                .Where(t => t.Estado.Id == 4)
                .Select(t => new TrabajoFinalizadoDTO
                {
                    Id = t.Id,

                    FechaSolicitud = t.FechaSolicitud,

                    FechaInicio = t.FechaInicio,

                    FechaFinalizado = t.FechaFinalizado,

                    FechaPagado = t.FechaPagado,

                    IdCliente = t.Cliente.Id,

                    Cliente =
                        t.Cliente.NroCliente +
                        " - " +
                        t.Cliente.Nombre,

                    IdTecnico = t.Tecnico.Id,

                    Tecnico =
                        t.Tecnico.NombreApellido,

                    IdTarea = t.Tarea.Id,

                    Tarea =
                        t.Tarea.Descripcion,

                    TrabajoRealizado =
                        t.TrabajoRealizado,

                    Provincia =
                        t.Cliente.Provincia.Nombre,

                    Ciudad =
                        t.Cliente.Ciudad.Nombre

                })
                .OrderByDescending(
                    t => t.FechaFinalizado
                );
        }
        public async Task<TrabajoResponseDto?>
    ObtenerPorIdAsync(int id)
        {
            var t =
                await _trabajoRepository.ObtenerPorIdAsync(id);

            if (t == null)
                return null;

            var usuarioId =
                ObtenerUsuarioIdActual();

            var rol =
                ObtenerRolActual();

            // Administrador y Sistemas
            // pueden acceder a cualquier trabajo
            if (rol != "Administrador" &&
                rol != "Sistemas")
            {
                // Técnico
                if (rol == "Tecnico" &&
                    t.Tecnico.Id != usuarioId)
                {
                    return null;
                }

                // Farmacia
                if (rol == "Farmacia")
                {
                    var usuario =
                        await _usuarioRepository
                            .ObtenerPorIdAsync(usuarioId);

                    if (usuario == null ||
                        usuario.Cliente.Id == null ||
                        t.Cliente.Id != usuario.Cliente.Id)
                    {
                        return null;
                    }
                }
            }

            return new TrabajoResponseDto
            {
                Id = t.Id,

                FechaSolicitud =
                    t.FechaSolicitud,

                FechaInicio =
                    t.FechaInicio,

                Estado =
                    t.Estado.Nombre,

                EstadoColor =
                    t.Estado.Color,

                IdCliente =
                    t.Cliente.Id,

                Cliente =
                    t.Cliente.Nombre,

                IdTecnico =
                    t.Tecnico.Id,

                Tecnico =
                    t.Tecnico.NombreApellido,

                IdTarea =
                    t.Tarea.Id,

                Tarea =
                    t.Tarea.Descripcion,

                Comentarios =
                    t.Comentarios,

                TrabajoRealizado =
                    t.TrabajoRealizado,

                TieneFactura =
                    !string.IsNullOrEmpty(t.Factura),

                Factura =
                    t.Factura,

                CantidadImagenes =
                    t.Imagenes.Count
            };
        }

        public async Task<TrabajoResponseDto> CrearAsync(TrabajoCreateDto dto)
        {
            if (!await _usuarioRepository.ExisteAsync(dto.IdTecnico))
                throw new Exception("El técnico no existe.");

            if (!await _clienteRepository.ExisteAsync(dto.IdCliente))
                throw new Exception("El cliente no existe.");

            var trabajo = new Trabajo
            {
                FechaSolicitud = DateTime.Now,
                Tecnico = await _usuarioRepository.ObtenerPorIdActivoAsync(dto.IdTecnico),
                Cliente = await _clienteRepository.ObtenerPorIdAsync(dto.IdCliente),
                Tarea = await _tareaRepository.ObtenerPorIdAsync(dto.IdTarea),
                Comentarios = dto.Comentarios,
                Estado = await _estadoRepository.ObtenerPorIdAsync(1)
            };

            await _trabajoRepository.AgregarAsync(trabajo);
            await _trabajoRepository.GuardarCambiosAsync();
            return await ObtenerPorIdAsync(trabajo.Id)
                   ?? throw new Exception("Error al recuperar el trabajo creado.");
        }

        public async Task<bool> ActualizarAsync(int id, TrabajoUpdateDto dto)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (trabajo == null)
                return false;

            trabajo.FechaInicio = dto.FechaInicio;

            trabajo.Tecnico = await _usuarioRepository.ObtenerPorIdActivoAsync(dto.IdTecnico);

            trabajo.Cliente = await _clienteRepository.ObtenerPorIdAsync(dto.IdCliente);

            trabajo.Tarea = await _tareaRepository.ObtenerPorIdAsync(dto.IdTarea);

            trabajo.Comentarios = dto.Comentarios;

            trabajo.TrabajoRealizado = dto.TrabajoRealizado;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (trabajo == null)
                return false;

            await _trabajoRepository.EliminarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            return true;
        }

        public async Task CambiarEstadoAsync(int idTrabajo, CambiarEstadoDTO dto)
        {
            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(dto.IdEstado);

            await _trabajoRepository.CambiarEstadoAsync(idTrabajo, estado);
        }

        public async Task GuardarTrabajoRealizado(int id, TrabajoRealizadoDTO dto)
        {
            await _trabajoRepository.GuardarTrabajoRealizado(id, dto);
        }

        public async Task SubirFacturaAsync(int idTrabajo, IFormFile archivo)
        {
            await _trabajoRepository.SubirFacturaAsync(idTrabajo, archivo, _environment);
        }

        public async Task RegistrarPagoAsync(int idTrabajo)
        {
            await _trabajoRepository.RegistrarPagoAsync(idTrabajo);
        }
    }
}
