using System.Security.Claims;
using QuestPDF.Fluent;
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
        private readonly ITrabajoImagenComparacionRepository _trabajoImagenComparacionRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IImagenService _imagenService;

        public TrabajoService(
            ITrabajoRepository trabajoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            IEstadoRepository estadoRepository,
            ITareaRepository tareaRepository,
            ITrabajoImagenComparacionRepository trabajoImagenComparacionRepository,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IImagenService imagenService)
        {
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
            _tareaRepository = tareaRepository;
            _trabajoImagenComparacionRepository = trabajoImagenComparacionRepository;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _imagenService = imagenService;
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
                .Where(t => t.Estado.Id != EstadosTrabajo.Finalizado)
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

                    //CantidadImagenes = t.Imagenes.Count

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

        public async Task<IEnumerable<TrabajoFinalizadoDTO>> ObtenerTrabajosPendientesPagoAsync()
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
                    IdCliente = t.Cliente.Id,
                    Cliente =
                        t.Cliente.NroCliente +
                        " - " +
                        t.Cliente.Nombre,
                    IdTecnico = t.Tecnico.Id,
                    Tecnico = t.Tecnico.NombreApellido,
                    IdTarea = t.Tarea.Id,
                    Tarea = t.Tarea.Descripcion,
                    TrabajoRealizado = t.TrabajoRealizado,
                    Provincia = t.Cliente.Provincia.Nombre,
                    Ciudad = t.Cliente.Ciudad.Nombre,
                    Factura = t.Factura
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
            if (rol != "Administrador" && rol != "Sistemas")
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

            var idsComparacion = t.ComparacionesImagenes
                                    .SelectMany(c => new int?[]
                                    {
                                        c.ImagenAntesId,
                                        c.ImagenDespuesId
                                    })
                                    .Where(x => x.HasValue)
                                    .Select(x => x!.Value)
                                    .ToHashSet();

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

                ImagenesSolicitud =
                    t.SolicitudImagenes
                        .Where(x => !idsComparacion.Contains(x.Id))
                        .Select(x => new ImagenDTO
                        {
                            Id = x.Id,
                            RutaArchivo = x.RutaArchivo
                        }).ToList(),

                Solicitante = t.UsuarioCreacion?.NombreApellido
            };
        }

        public async Task<TrabajoResponseDto> CrearAsync(TrabajoCreateDto dto)
        {
            var tecnicoExiste = await _usuarioRepository.ExisteAsync(dto.IdTecnico);

            if (!tecnicoExiste)
                throw new Exception($"El técnico con ID {dto.IdTecnico} no existe.");

            if (!await _clienteRepository.ExisteAsync(dto.IdCliente))
                throw new Exception("El cliente no existe.");

            var usuarioId = int.Parse(
                                    _httpContextAccessor.HttpContext!
                                        .User
                                        .FindFirst(ClaimTypes.NameIdentifier)!
                                        .Value
                                );

            var trabajo = new Trabajo
            {
                FechaSolicitud = DateTime.UtcNow,
                Tecnico = await _usuarioRepository.ObtenerPorIdActivoAsync(dto.IdTecnico),
                Cliente = await _clienteRepository.ObtenerPorIdAsync(dto.IdCliente),
                Tarea = await _tareaRepository.ObtenerPorIdAsync(dto.IdTarea),
                Comentarios = dto.Comentarios,
                Estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Pendiente),
                UsuarioCreacion = await _usuarioRepository.ObtenerPorIdActivoAsync(usuarioId)
            };

            await _trabajoRepository.AgregarAsync(trabajo);
            await _trabajoRepository.GuardarCambiosAsync();

            if (dto.Archivos != null && dto.Archivos.Count > 0)
            {
                await _imagenService.SubirImagenes(trabajo.Id, dto.Archivos);
            }

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

            trabajo.FechaInicio = DateTime.UtcNow;

            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.EnProceso);
            trabajo.Estado = estado;

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

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            return true;
        }

        public async Task<bool> FinalizarTrabajoAsync(int idTrabajo, TrabajoRealizadoDTO dto)
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

            if (string.IsNullOrWhiteSpace(dto.TrabajoRealizado))
            {
                throw new InvalidOperationException(
                    "Debe indicar el trabajo realizado."
                );
            }

            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.TrabajoFinalizado);
            trabajo.Estado = estado;
            trabajo.TrabajoRealizado = dto.TrabajoRealizado;
            trabajo.FechaFinalizado = DateTime.UtcNow;

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            //se le avisa a sistemas que finalizo el trabajo y que debe aprobarlo para que se pueda cargar la factura
            if (!string.IsNullOrWhiteSpace(trabajo.UsuarioCreacion.Email))
            {
                var comparaciones = await _trabajoImagenComparacionRepository.ObtenerPorTrabajoAsync(trabajo.Id);

                var imagenes = comparaciones.Select(x => (
                                Antes: x.ImagenAntes != null
                                    ? $"https://localhost:7122{x.ImagenAntes.RutaArchivo}"
                                    : null,

                                Despues: x.ImagenDespues != null
                                    ? $"https://localhost:7122{x.ImagenDespues.RutaArchivo}"
                                    : null
                            ));

                var html = TrabajoEmailTemplates.TrabajoPendienteAprobacion(trabajo.Tecnico.NombreApellido,
                                trabajo.Id,
                                $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre}",
                                trabajo.Tarea.Descripcion,
                                trabajo.TrabajoRealizado ?? "",
                                imagenes
                            );

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

            trabajo.FechaFinalizado = DateTime.UtcNow;
            EstadoTrabajo estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Aprobado);
            trabajo.Estado = estado;

            //se le avisa al tecnico
            if (!string.IsNullOrWhiteSpace(trabajo.Tecnico.Email))
            {
                var html = TrabajoEmailTemplates.TrabajoAprobado(trabajo.Tecnico.NombreApellido, trabajo.Id,
                    $"{trabajo.Cliente?.NroCliente} {trabajo.Cliente?.Nombre}", trabajo.Tarea.Descripcion);

                await _emailService.EnviarAsync(
                    trabajo.Tecnico?.Email,
                    $"Trabajo aprobado #{trabajo.Id}",
                    html);
            }

            await _trabajoRepository.ActualizarAsync(trabajo);

            await _trabajoRepository.GuardarCambiosAsync();

            return true;
        }

        public async Task SubirFacturaAsync(int idTrabajo, IFormFile archivo)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(idTrabajo);

            var usuarioPagos = await _usuarioRepository.ObtenerPorPerfil(9); //pagos

            await _trabajoRepository.SubirFacturaAsync(idTrabajo, archivo, _environment);

            foreach (var user in usuarioPagos)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    var html = TrabajoEmailTemplates.FacturaPendientePago(trabajo.Id,
                        $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre}", trabajo.Tecnico.NombreApellido, trabajo.Tarea.Descripcion);

                    await _emailService.EnviarAsync(
                        user.Email,
                        $"Factura pendiente de pago #{trabajo.Id}",
                        html);
                }
            }

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

            trabajo.FechaPagado = DateTime.UtcNow;
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

        public async Task<bool> SolicitarMejoraAsync(int id, SolicitarMejoraDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Comentario))
                throw new InvalidOperationException(
                    "Debe indicar qué mejora debe realizar el técnico."
                );

            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (trabajo == null)
                return false;

            if (trabajo.Estado.Id != EstadosTrabajo.TrabajoFinalizado)
            {
                throw new InvalidOperationException(
                    "Solo se puede solicitar una mejora cuando el trabajo está finalizado."
                );
            }

            if (trabajo.Tecnico == null)
            {
                throw new InvalidOperationException(
                    "El trabajo no tiene un técnico asignado."
                );
            }

            trabajo.Comentarios = dto.Comentario.Trim();

            trabajo.Estado = await _estadoRepository.ObtenerPorIdAsync(EstadosTrabajo.Pendiente);

            // El trabajo realizado anterior puede quedar
            // registrado hasta que el técnico lo reemplace.
            // Si querés borrarlo:
            //
            // trabajo.TrabajoRealizado = null;

            await _trabajoRepository.ActualizarAsync(trabajo);
            await _trabajoRepository.GuardarCambiosAsync();

            if (!string.IsNullOrWhiteSpace(trabajo.Tecnico.Email))
            {
                var html =
                    TrabajoEmailTemplates.MejoraTrabajoSolicitada(
                        trabajo.Tecnico.NombreApellido,
                        trabajo.Id,
                        $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre}",
                        trabajo.Tarea.Descripcion,
                        trabajo.Comentarios
                    );

                await _emailService.EnviarAsync(
                    trabajo.Tecnico.Email,
                    $"Se solicitó una mejora - Trabajo #{trabajo.Id}",
                    html
                );
            }

            return true;
        }

        private string ObtenerRutaFisicaImagen(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                return string.Empty;

            var ruta = rutaArchivo
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .TrimStart(
                    '/',
                    '\\'
                );

            return Path.Combine(
                _environment.WebRootPath,
                ruta
            );
        }

        public async Task<byte[]> GenerarInformePdfAsync(int id)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (trabajo == null)
                throw new KeyNotFoundException($"No existe el trabajo #{id}.");

            if (trabajo.Estado == null ||
                trabajo.Estado.Id < EstadosTrabajo.TrabajoFinalizado)
            {
                throw new InvalidOperationException(
                    "El informe PDF solamente está disponible cuando el trabajo está finalizado."
                );
            }

            var usuarioActual = ObtenerUsuarioActual();

            var puedeVerTodos =
                usuarioActual.Rol == "Administrador" ||
                usuarioActual.Rol == "Sistemas" ||
                usuarioActual.Rol == "Pagos";

            if (!puedeVerTodos)
            {
                if (usuarioActual.Rol == "Tecnico" &&
                    trabajo.Tecnico?.Id != usuarioActual.UsuarioId)
                {
                    throw new UnauthorizedAccessException(
                        "No tiene permiso para consultar este trabajo."
                    );
                }

                if (usuarioActual.Rol == "Farmacia")
                {
                    var usuario = await _usuarioRepository
                        .ObtenerPorIdAsync(usuarioActual.UsuarioId);

                    if (usuario?.Cliente?.Id == null ||
                        trabajo.Cliente?.Id != usuario.Cliente.Id)
                    {
                        throw new UnauthorizedAccessException(
                            "No tiene permiso para consultar este trabajo."
                        );
                    }
                }
            }

            // Todas las imágenes del trabajo.
            var todasLasImagenes =
                (await _imagenService.ObtenerPorTrabajo(id))?
                    .Where(imagen =>
                        imagen != null &&
                        !string.IsNullOrWhiteSpace(imagen.RutaArchivo))
                    .OrderBy(imagen => imagen.Id)
                    .ToList()
                ?? new List<Imagen>();

            // Comparaciones registradas para el trabajo.
            var comparaciones =
                (await _trabajoImagenComparacionRepository
                    .ObtenerPorTrabajoAsync(id))?
                    .Where(comparacion =>
                        comparacion.ImagenAntesId.HasValue ||
                        comparacion.ImagenDespuesId.HasValue)
                    .OrderBy(comparacion => comparacion.Id)
                    .ToList()
                ?? new List<TrabajoImagenComparacion>();

            // Índice para resolver Antes/Después aunque las navegaciones EF no estén cargadas.
            var imagenesPorId = todasLasImagenes
                .GroupBy(imagen => imagen.Id)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.First()
                );

            var idsImagenesComparacion = comparaciones
                .SelectMany(comparacion => new int?[]
                {
                    comparacion.ImagenAntesId,
                    comparacion.ImagenDespuesId
                })
                .Where(idImagen => idImagen.HasValue)
                .Select(idImagen => idImagen!.Value)
                .ToHashSet();

            // Solo imágenes que no están utilizadas como Antes/Después.
            var imagenesSolicitud = todasLasImagenes
                .Where(imagen => !idsImagenesComparacion.Contains(imagen.Id))
                .OrderBy(imagen => imagen.Id)
                .ToList();

            using var stream = new MemoryStream();

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(QuestPDF.Helpers.PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(style => style.FontSize(10));

                    page.Header().Element(header =>
                    {
                        header.Column(column =>
                        {
                            column.Item()
                                .Text($"INFORME DE TRABAJO #{trabajo.Id}")
                                .FontSize(20)
                                .Bold();

                            column.Item()
                                .Text("Sistema Técnico")
                                .FontSize(11);

                            column.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1);
                        });
                    });

                    page.Content()
                        .PaddingTop(15)
                        .Column(column =>
                        {
                            // 1. INFORMACIÓN GENERAL
                            column.Item()
                                .Text("Información del trabajo")
                                .FontSize(14)
                                .Bold();

                            column.Item()
                                .PaddingTop(8)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    FilaPdf(table, "Nº Trabajo", trabajo.Id.ToString());
                                    FilaPdf(table, "Estado", trabajo.Estado?.Nombre ?? "-");
                                    FilaPdf(table, "Fecha solicitud",
                                        trabajo.FechaSolicitud.ToString("dd/MM/yyyy HH:mm"));
                                    FilaPdf(table, "Fecha inicio",
                                        trabajo.FechaInicio?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                                    FilaPdf(table, "Fecha finalización",
                                        trabajo.FechaFinalizado?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                                    FilaPdf(table, "Fecha pago",
                                        trabajo.FechaPagado?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                                });

                            // 2. DATOS DE LA SOLICITUD
                            column.Item()
                                .PaddingTop(20)
                                .Text("Solicitud")
                                .FontSize(14)
                                .Bold();

                            column.Item()
                                .PaddingTop(8)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    FilaPdf(table, "Solicitante",
                                        trabajo.UsuarioCreacion?.NombreApellido ?? "-");
                                    FilaPdf(table, "Técnico",
                                        trabajo.Tecnico?.NombreApellido ?? "-");
                                    FilaPdf(table, "Cliente",
                                        $"{trabajo.Cliente?.NroCliente} - {trabajo.Cliente?.Nombre}");
                                    FilaPdf(table, "Dirección",
                                        trabajo.Cliente?.Direccion ?? "-");
                                    FilaPdf(table, "Provincia",
                                        trabajo.Cliente?.Provincia?.Nombre ?? "-");
                                    FilaPdf(table, "Ciudad",
                                        trabajo.Cliente?.Ciudad?.Nombre ?? "-");
                                    FilaPdf(table, "Tarea",
                                        trabajo.Tarea?.Descripcion ?? "-");
                                });

                            // 3. IMÁGENES DE LA SOLICITUD
                            if (imagenesSolicitud.Count > 0)
                            {
                                column.Item()
                                    .PaddingTop(20)
                                    .Text("Imágenes de la solicitud")
                                    .FontSize(14)
                                    .Bold();

                                foreach (var grupo in imagenesSolicitud.Chunk(2))
                                {
                                    column.Item()
                                        .PaddingTop(10)
                                        .Row(row =>
                                        {
                                            for (var indice = 0;
                                                 indice < grupo.Length;
                                                 indice++)
                                            {
                                                var imagen = grupo[indice];
                                                var item = row.RelativeItem();

                                                item = indice == 0
                                                    ? item.PaddingRight(5)
                                                    : item.PaddingLeft(5);

                                                item.Element(contenedor =>
                                                    DibujarImagenSolicitud(
                                                        contenedor,
                                                        imagen));
                                            }

                                            if (grupo.Length == 1)
                                                row.RelativeItem().PaddingLeft(5);
                                        });
                                }
                            }

                            // 4. COMENTARIOS DE LA SOLICITUD
                            column.Item()
                                .PaddingTop(20)
                                .Text("Comentarios de la solicitud")
                                .FontSize(14)
                                .Bold();

                            column.Item()
                                .PaddingTop(5)
                                .Border(1)
                                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                                .Padding(10)
                                .Text(string.IsNullOrWhiteSpace(trabajo.Comentarios)
                                    ? "Sin comentarios"
                                    : trabajo.Comentarios);

                            // 5. TRABAJO REALIZADO
                            column.Item()
                                .PaddingTop(25)
                                .Text("Trabajo realizado")
                                .FontSize(14)
                                .Bold();

                            column.Item()
                                .PaddingTop(5)
                                .Border(1)
                                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                                .Padding(10)
                                .Text(string.IsNullOrWhiteSpace(trabajo.TrabajoRealizado)
                                    ? "Sin detalle del trabajo realizado"
                                    : trabajo.TrabajoRealizado);

                            // 6. COMPARACIONES ANTES / DESPUÉS
                            if (comparaciones.Count > 0)
                            {
                                column.Item()
                                    .PaddingTop(20)
                                    .Text("Imágenes del trabajo realizado")
                                    .FontSize(14)
                                    .Bold();

                                var numeroComparacion = 1;

                                foreach (var comparacion in comparaciones)
                                {
                                    var numeroActual = numeroComparacion++;

                                    Imagen? imagenAntes = null;
                                    Imagen? imagenDespues = null;

                                    if (comparacion.ImagenAntesId.HasValue)
                                    {
                                        imagenesPorId.TryGetValue(
                                            comparacion.ImagenAntesId.Value,
                                            out imagenAntes);
                                    }

                                    if (comparacion.ImagenDespuesId.HasValue)
                                    {
                                        imagenesPorId.TryGetValue(
                                            comparacion.ImagenDespuesId.Value,
                                            out imagenDespues);
                                    }

                                    column.Item()
                                        .PaddingTop(12)
                                        .Border(1)
                                        .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                                        .Padding(10)
                                        .Column(comparacionColumn =>
                                        {
                                            comparacionColumn.Item()
                                                .Text($"Comparación #{numeroActual}")
                                                .FontSize(11)
                                                .Bold();

                                            comparacionColumn.Item()
                                                .PaddingTop(8)
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .PaddingRight(5)
                                                        .Element(contenedor =>
                                                            DibujarImagenComparacion(
                                                                contenedor,
                                                                "Antes",
                                                                imagenAntes));

                                                    row.RelativeItem()
                                                        .PaddingLeft(5)
                                                        .Element(contenedor =>
                                                            DibujarImagenComparacion(
                                                                contenedor,
                                                                "Después",
                                                                imagenDespues));
                                                });
                                        });
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Sistema Técnico - ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            });

            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        private void DibujarImagenSolicitud(
                    QuestPDF.Infrastructure.IContainer container,
                    Imagen imagen)
        {
            container
                .Border(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten1)
                .Padding(8)
                .Column(column =>
                {
                    var resultadoImagen = ObtenerBytesImagen(imagen.RutaArchivo);

                    if (resultadoImagen.Bytes == null)
                    {
                        DibujarImagenNoDisponible(
                            column,
                            resultadoImagen.Mensaje);
                        return;
                    }

                    column.Item()
                        .Height(210)
                        .AlignCenter()
                        .AlignMiddle()
                        .Image(resultadoImagen.Bytes)
                        .FitArea();
                });
        }
        private (byte[]? Bytes, string Mensaje) ObtenerBytesImagen(string? rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                return (
                    null,
                    "Sin imagen"
                );
            }

            var rutaFisica =
                ObtenerRutaFisicaImagen(
                    rutaArchivo
                );

            if (
                string.IsNullOrWhiteSpace(rutaFisica) ||
                !System.IO.File.Exists(rutaFisica)
            )
            {
                return (
                    null,
                    "Archivo no encontrado"
                );
            }

            try
            {
                return (
                    System.IO.File.ReadAllBytes(
                        rutaFisica
                    ),
                    string.Empty
                );
            }
            catch
            {
                return (
                    null,
                    "Error al leer imagen"
                );
            }
        }

        private static void DibujarImagenNoDisponible(
                    QuestPDF.Fluent.ColumnDescriptor column,
                    string mensaje)
        {
            column.Item()
                .Height(210)
                .Background(QuestPDF.Helpers.Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                .Text(mensaje)
                .FontSize(9)
                .FontColor(QuestPDF.Helpers.Colors.Grey.Darken1);
        }
        private void DibujarImagenComparacion(
                    QuestPDF.Infrastructure.IContainer container,
                    string titulo,
                    Imagen? imagen)
        {
            container
                .Border(1)
                .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2)
                .Padding(8)
                .Column(column =>
                {
                    column.Item()
                        .AlignCenter()
                        .Text(titulo)
                        .FontSize(11)
                        .Bold();

                    if (imagen == null)
                    {
                        DibujarImagenNoDisponible(column, "Sin imagen");
                        return;
                    }

                    var resultadoImagen = ObtenerBytesImagen(imagen.RutaArchivo);

                    if (resultadoImagen.Bytes == null)
                    {
                        DibujarImagenNoDisponible(
                            column,
                            resultadoImagen.Mensaje);
                        return;
                    }

                    column.Item()
                        .PaddingTop(8)
                        .Height(210)
                        .AlignCenter()
                        .AlignMiddle()
                        .Image(resultadoImagen.Bytes)
                        .FitArea();
                });
        }
        private static void FilaPdf(
            QuestPDF.Fluent.TableDescriptor table,
            string titulo,
            string? valor)
        {
            table.Cell()
                .BorderBottom(1)
                .Padding(5)
                .Text(titulo)
                .Bold();

            table.Cell()
                .BorderBottom(1)
                .Padding(5)
                .Text(string.IsNullOrWhiteSpace(valor) ? "-" : valor);
        }
    }
}