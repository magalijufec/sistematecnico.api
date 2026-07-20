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

        public TrabajoService(
            ITrabajoRepository trabajoRepository,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            IEstadoRepository estadoRepository,
            ITareaRepository tareaRepository)
        {
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
            _tareaRepository = tareaRepository;
        }

        public async Task<IEnumerable<TrabajoResponseDto>> ObtenerTodosAsync()
        {
            var trabajos = await _trabajoRepository.ObtenerTodosAsync();

            return trabajos.Select(t => new TrabajoResponseDto
            {
                Id = t.Id,
                FechaSolicitud = t.FechaSolicitud,
                FechaTrabajo = t.FechaTrabajo,
                Estado = t.Estado.Nombre,
                EstadoColor = t.Estado.Color,
                Cliente = t.Cliente.NroCliente + " - " + t.Cliente.Nombre,
                Tecnico = t.Tecnico.NombreApellido,
                Tarea = t.Tarea.Descripcion,
                TrabajoRealizado = t.TrabajoRealizado,
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
                EstadoColor = t.Estado.Color,

                IdCliente = t.Cliente.Id,
                Cliente = t.Cliente.Nombre,

                IdTecnico = t.Tecnico.Id,
                Tecnico = t.Tecnico.NombreApellido,

                IdTarea = t.Tarea.Id,
                Tarea = t.Tarea.Descripcion,
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

            trabajo.FechaTrabajo = dto.FechaTrabajo;

            trabajo.Tecnico = await _usuarioRepository.ObtenerPorIdAsync(dto.IdTecnico);

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

        public async Task GuardarTrabajoRealizado(
            int id,
            TrabajoRealizadoDTO dto)
        {
            await _trabajoRepository.GuardarTrabajoRealizado(id, dto);
        }
    }
}
