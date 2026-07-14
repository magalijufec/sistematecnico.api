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

        public TrabajoService(
            ITrabajoRepository trabajoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            IEstadoRepository estadoRepository)
        {
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task<IEnumerable<TrabajoResponseDto>> ObtenerTodosAsync()
        {
            var trabajos = await _trabajoRepository.ObtenerTodosAsync();

            return trabajos.Select(t => new TrabajoResponseDto
            {
                Id = t.Id,
                FechaSolicitud = t.FechaSolicitud,
                FechaTrabajo = t.FechaTrabajo,

                //IdEstado = t.IdEstado,
                Estado = t.Estado.Nombre,

                //IdCliente = t.IdCliente,
                Cliente = t.Cliente.Nombre,

                //IdTecnico = t.IdTecnico,
                Tecnico = t.Tecnico.NombreApellido,

                Tarea = t.Tarea,
                Comentarios = t.Comentarios,
                TrabajoRealizado = t.TrabajoRealizado,

                //Sector = t.Sector?.Nombre,

                TieneFactura = !string.IsNullOrEmpty(t.Factura),

                CantidadImagenes = t.Imagenes.Count
            });
        }

        public async Task<TrabajoResponseDto?> ObtenerPorIdAsync(int id)
        {
            var t = await _trabajoRepository.ObtenerPorIdAsync(id);

            if (t == null)
                return null;

            return new TrabajoResponseDto
            {
                Id = t.Id,
                FechaSolicitud = t.FechaSolicitud,
                FechaTrabajo = t.FechaTrabajo,

                //IdEstado = t.IdEstado,
                Estado = t.Estado.Nombre,

                //IdCliente = t.IdCliente,
                Cliente = t.Cliente.Nombre,

                //IdTecnico = t.IdTecnico,
                Tecnico = t.Tecnico.NombreApellido,

                Tarea = t.Tarea,
                Comentarios = t.Comentarios,
                TrabajoRealizado = t.TrabajoRealizado,

                //Sector = t.Sector?.Nombre,

                TieneFactura = !string.IsNullOrEmpty(t.Factura),

                CantidadImagenes = t.Imagenes.Count
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

                FechaTrabajo = dto.FechaTrabajo,

                Tecnico = await _usuarioRepository.ObtenerPorIdAsync(dto.IdTecnico),

                Cliente = await _clienteRepository.ObtenerPorIdAsync(dto.IdCliente),

                //IdSector = dto.IdSector,

                Tarea = dto.Tarea,

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

            trabajo.FechaTrabajo = dto.FechaTrabajo;

            //trabajo.IdTecnico = dto.IdTecnico;

            //trabajo.IdCliente = dto.IdCliente;

            //trabajo.IdSector = dto.IdSector;

            trabajo.Tarea = dto.Tarea;

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
    }
}
