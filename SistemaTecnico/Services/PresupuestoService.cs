using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SistemaTecnico.DTO;
using SistemaTecnico.Helpers;
using SistemaTecnico.Models;
using SistemaTecnico.Repositories;

namespace SistemaTecnico.Services
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly ITrabajoRepository _trabajoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IWebHostEnvironment _environment;

        public PresupuestoService (IPresupuestoRepository presupuestoRepository, ITrabajoRepository trabajoRepository, IUsuarioRepository usuarioRepository, IWebHostEnvironment environment)
        {
            _presupuestoRepository = presupuestoRepository;
            _trabajoRepository = trabajoRepository;
            _usuarioRepository = usuarioRepository;
            _environment = environment;
        }

        public async Task<List<PresupuestoDetalleDTO>> ObtenerPresupuestosByTrabajoAsync(int idTrabajo)
        {
            var presupuestos = await _presupuestoRepository.ObtenerPorTrabajoAsync(idTrabajo);

            return presupuestos
                .Select(p => new PresupuestoDetalleDTO
                {
                    Id = p.Id,
                    RutaArchivo = p.RutaArchivo,
                    Descripcion = p.Descripcion,
                    TecnicoId = p.TecnicoId,
                    Tecnico = p.Tecnico?.NombreApellido ?? "Técnico no disponible",
                    FechaCarga = FechaHelper.AhoraArgentina(p.FechaCarga),
                    TrabajoId = p.TrabajoId,
                    EstadoId = p.EstadoId,
                    Estado = p.Estado?.Descripcion ?? "Sin estado"
                }).ToList();
        }

        public async Task<Presupuesto> CrearAsync(PresupuestoDTO dto)
        {
            var trabajo = await _trabajoRepository.ObtenerPorIdAsync(dto.TrabajoId);

            if (trabajo == null)
                throw new Exception("El trabajo no existe.");

            var tecnico = await _usuarioRepository.ObtenerPorIdActivoAsync(dto.TecnicoId);

            if (tecnico == null)
                throw new Exception("El técnico no existe.");

            var presupuesto =
                new Presupuesto
                {
                    FechaCarga = DateTime.UtcNow,
                    TecnicoId = dto.TecnicoId,
                    TrabajoId = dto.TrabajoId,
                    EstadoId = EstadosPresupuesto.EnRevision,
                    Descripcion = dto.Descripcion
                };

            await _presupuestoRepository.AgregarAsync(presupuesto);
            await _presupuestoRepository.GuardarCambiosAsync();

            var carpeta =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "presupuestos",
                    presupuesto.Id.ToString()
                );

            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{DateTime.Now:yyMMddHHmmss}";

            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await dto.Archivo.CopyToAsync(stream);
            }

            if (trabajo.EstadoId == EstadosTrabajo.PendientePresupuestos)
            {
                await _trabajoRepository.CambiarEstadoAsync(trabajo.Id, EstadosTrabajo.PendienteAprobacionPresupuesto);
            }

            presupuesto.RutaArchivo = $"/uploads/presupuestos/{presupuesto.Id}/{nombreArchivo}";

            await _presupuestoRepository.GuardarCambiosAsync();
            return presupuesto;
        }

        public async Task<Presupuesto> ActualizarAsync(int id, PresupuestoDecisionDTO dto)
        {
            var presupuesto = await _presupuestoRepository.ObtenerPorIdAsync(id);

            if (presupuesto == null)
            {
                throw new Exception("El presupuesto no existe.");
            }

            presupuesto.UsuarioDecisionId = dto.IdUsuarioDecision;
            presupuesto.FechaDecision = DateTime.UtcNow;

            if (dto.Rechazo)
            {
                presupuesto.EstadoId = EstadosPresupuesto.Rechazado;
                presupuesto.MotivoRechazo = dto.MotivoRechazo;
            }
            else
            {
                presupuesto.EstadoId = EstadosPresupuesto.Aprobado;
                presupuesto.MotivoRechazo = null;
                await _trabajoRepository.ActualizarPresupuestoAsync(presupuesto.TrabajoId, dto, presupuesto.TecnicoId);

                var presupuestosDelTrabajo = await _presupuestoRepository.ObtenerPorTrabajoAsync(presupuesto.TrabajoId);
                foreach (var item in presupuestosDelTrabajo)
                {
                    if (item.Id != presupuesto.Id)
                    {
                        item.UsuarioDecisionId = dto.IdUsuarioDecision;
                        item.FechaDecision = DateTime.UtcNow;
                        item.EstadoId = EstadosPresupuesto.Rechazado;
                        item.MotivoRechazo = "Rechazo automático. Se aprobó otro presupuesto";
                    }
                }
            }

            await _presupuestoRepository.GuardarCambiosAsync();

            return presupuesto;
        }

        public async Task<bool> RechazarAsync(int presupuestoId, string motivo)
        {
            var presupuesto =
                await _presupuestoRepository
                    .ObtenerPorIdAsync(
                        presupuestoId
                    );

            if (presupuesto == null)
                return false;

            presupuesto.EstadoId =
                EstadosPresupuesto
                    .Rechazado;

            presupuesto.MotivoRechazo =
                motivo;

            presupuesto.FechaDecision =
                DateTime.UtcNow;

            await _presupuestoRepository
                .GuardarCambiosAsync();

            return true;
        }

    }
}
