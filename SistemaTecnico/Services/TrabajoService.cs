using System.Security.Claims;
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
        private readonly IEmailService _emailService;

        public TrabajoService(
            ITrabajoRepository trabajoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            IEstadoRepository estadoRepository,
            ITareaRepository tareaRepository,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
            _tareaRepository = tareaRepository;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
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

        private (int UsuarioId, string Rol) ObtenerUsuarioActual()
        {
            var usuario = _httpContextAccessor.HttpContext?.User;

            if (usuario == null ||
                !usuario.Identity?.IsAuthenticated == true)
            {
                throw new UnauthorizedAccessException(
                    "El usuario no está autenticado."
                );
            }

            var usuarioIdClaim =
                usuario.FindFirst(ClaimTypes.NameIdentifier);

            var rolClaim =
                usuario.FindFirst(ClaimTypes.Role);

            if (usuarioIdClaim == null ||
                rolClaim == null)
            {
                throw new UnauthorizedAccessException(
                    "No se pudo obtener la información del usuario."
                );
            }

            return (
                int.Parse(usuarioIdClaim.Value),
                rolClaim.Value
            );
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

        public async Task<IEnumerable<TrabajoResponseDto>> ObtenerTrabajosNoFinalizadosAsync()
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

        private async Task<IEnumerable<Trabajo>> ObtenerTrabajosSegunUsuarioAsync()
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

        public async Task<IEnumerable<TrabajoFinalizadoDTO>>ObtenerTrabajosPendientesPagoAsync()
        {
            var trabajos =
                await ObtenerTrabajosSegunUsuarioAsync();

            return trabajos
                .Where(t => t.Estado.Id == EstadosTrabajo.PendientePago)
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

        public async Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosFinalizadosAsync()
        {
            var trabajos =
                await ObtenerTrabajosSegunUsuarioAsync();

            return trabajos
                .Where(t => t.Estado.Id == EstadosTrabajo.Finalizado)
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

        public async Task<TrabajoResponseDto?> ObtenerPorIdAsync(int id)
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
                Estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Pendiente)
            };

            await _trabajoRepository.AgregarAsync(trabajo);
            await _trabajoRepository.GuardarCambiosAsync();

            if (!string.IsNullOrWhiteSpace(trabajo.Tecnico?.Email))
            {
                var html = TrabajoEmailTemplates.NuevoTrabajoAsignado(trabajo.Tecnico.NombreApellido,
                trabajo.Id, $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre} - {trabajo.Cliente?.Direccion}", trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.Tecnico.Email,
                    $"Nuevo trabajo #{trabajo.Id}",
                    html);
            }

            return await ObtenerPorIdAsync(trabajo.Id)
                   ?? throw new Exception("Error al recuperar el trabajo creado.");
        }

        public async Task<bool> ActualizarAsync(int id, TrabajoUpdateDto dto)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (trabajo == null)
                return false;

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

        public async Task GuardarTrabajoRealizado(int id, TrabajoRealizadoDTO dto)
        {
            await _trabajoRepository.GuardarTrabajoRealizado(id, dto);
        }

        public async Task SubirFacturaAsync(int idTrabajo, IFormFile archivo)
        {
            await _trabajoRepository.SubirFacturaAsync(idTrabajo, archivo, _environment);

            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            var usuarioPagos = await _usuarioRepository.ObtenerPorPerfil(9); //pagos

            foreach (var user in usuarioPagos)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    var html = TrabajoEmailTemplates.FacturaPendientePago(trabajo.Id, 
                        $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre}", trabajo.Tecnico.NombreApellido, trabajo.Tarea.Descripcion);

                    await _emailService.EnviarAsync(
                        trabajo.Tecnico.Email,
                        $"Factura pendiente de pago #{trabajo.Id}",
                        html);
                }
            }
            
        }        

        public async Task<bool> IniciarTrabajoAsync(int idTrabajo)
        {
            var (usuarioId, rol) = ObtenerUsuarioActual();

            if (rol != "Tecnico")
                throw new UnauthorizedAccessException(
                    "Solo un técnico puede iniciar un trabajo."
                );

            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
                return false;

            if (trabajo.Tecnico.Id != usuarioId)
                throw new UnauthorizedAccessException(
                    "El trabajo no está asignado a este técnico."
                );

            if (trabajo.Estado.Id != 1)
                throw new InvalidOperationException(
                    "El trabajo no se encuentra en estado Pendiente."
                );

            trabajo.FechaInicio = DateTime.Now;

            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.EnProceso);
            trabajo.Estado = estado;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            //envia mail al usuario de sistemas que asigno el trabajo

            if (!string.IsNullOrWhiteSpace(trabajo.UsuarioCreacion.Email))
            {
                var html = TrabajoEmailTemplates.TrabajoIniciado(trabajo.Tecnico.NombreApellido, trabajo.Id,
                    $"{trabajo.Cliente?.NroCliente} {trabajo.Cliente?.Nombre}", trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.Tecnico?.Email,
                    $"Trabajo iniciado #{trabajo.Id}",
                    html);
            }

            return true;
        }

        public async Task<bool> FinalizarTrabajoAsync(int idTrabajo)
        {
            var (usuarioId, rol) = ObtenerUsuarioActual();

            if (rol != "Tecnico")
                throw new UnauthorizedAccessException(
                    "Solo un técnico puede finalizar un trabajo."
                );

            var trabajo =
                await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
                return false;

            if (trabajo.Tecnico.Id != usuarioId)
                throw new UnauthorizedAccessException(
                    "El trabajo no está asignado a este técnico."
                );

            if (trabajo.Estado.Id != 2)
                throw new InvalidOperationException(
                    "El trabajo debe estar En proceso."
                );

            if (string.IsNullOrWhiteSpace(
                trabajo.TrabajoRealizado))
            {
                throw new InvalidOperationException(
                    "Debe indicar el trabajo realizado."
                );
            }

            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.TrabajoFinalizado);
            trabajo.Estado = estado;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            //se le avisa a sistemas que finalizo el trabajo y que debe aprobarlo para que se pueda cargar la factura
            if (!string.IsNullOrWhiteSpace(trabajo.UsuarioCreacion.Email))
            {
                var html = TrabajoEmailTemplates.TrabajoPendienteAprobacion(trabajo.Id,
                    $"{trabajo.Cliente?.NroCliente} {trabajo.Cliente?.Nombre}", trabajo.Tecnico.NombreApellido, trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.UsuarioCreacion.Email,
                    $"Trabajo finalizado #{trabajo.Id} - Pendiente aprobacion",
                    html);
            }

            return true;
        }

        public async Task<bool> AprobarTrabajoAsync(int idTrabajo)
        {
            var (usuarioId, rol) = ObtenerUsuarioActual();

            if (rol != "Sistemas" && rol != "Administrador")
            {
                throw new UnauthorizedAccessException(
                    "Solo Sistemas o Administrador pueden aprobar el trabajo."
                );
            }

            var trabajo =
                await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
                return false;

            if (trabajo.Estado.Id != 3)
                throw new InvalidOperationException(
                    "El trabajo debe estar en estado Trabajo finalizado."
                );

            trabajo.FechaFinalizado = DateTime.Now;
            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Aprobado);
            trabajo.Estado = estado;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            //se le avisa al tecnico
            if (!string.IsNullOrWhiteSpace(trabajo.Tecnico.Email))
            {
                var html = TrabajoEmailTemplates.TrabajoAprobado(trabajo.Tecnico.NombreApellido, trabajo.Id,
                    $"{trabajo.Cliente?.NroCliente} {trabajo.Cliente?.Nombre}",  trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.Tecnico?.Email,
                    $"Trabajo aprobado #{trabajo.Id}",
                    html);
            }

            return true;
        }

        public async Task<bool> RegistrarPagoAsync(int idTrabajo)
        {
            var (usuarioId, rol) = ObtenerUsuarioActual();

            if (rol != "Pagos" &&
                rol != "Administrador")
            {
                throw new UnauthorizedAccessException(
                    "Solo Pagos o Administrador pueden registrar el pago."
                );
            }

            var trabajo =
                await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            if (trabajo == null)
                return false;

            if (trabajo.Estado.Id != 5)
                throw new InvalidOperationException(
                    "El trabajo debe estar pendiente de pago."
                );            

            trabajo.FechaPagado = DateTime.Now;
            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Finalizado);
            trabajo.Estado = estado;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            if (!string.IsNullOrWhiteSpace(trabajo.Tecnico.Email))
            {
                var html = TrabajoEmailTemplates.TrabajoFinalizado(trabajo.Id, 
                    $"{trabajo.Cliente?.NroCliente} {trabajo.Cliente?.Nombre}", trabajo.Tecnico.NombreApellido, trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.Tecnico?.Email,
                    $"Trabajo pagado #{trabajo.Id}",
                    html);
            }

            return true;
        }
    }
}
