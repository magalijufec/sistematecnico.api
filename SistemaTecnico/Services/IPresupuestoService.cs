using SistemaTecnico.DTO;
using SistemaTecnico.Models;

namespace SistemaTecnico.Services
{
    public interface IPresupuestoService
    {
        Task<List<PresupuestoDetalleDTO>> ObtenerPresupuestosByTrabajoAsync(int trabajoId);
        Task<Presupuesto> CrearAsync(PresupuestoDTO dto);
        Task<Presupuesto> ActualizarAsync(int id, PresupuestoDecisionDTO dto);
        Task<bool> RechazarAsync(int presupuestoId, string motivo);
    }
}
