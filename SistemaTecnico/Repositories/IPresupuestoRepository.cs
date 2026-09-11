using SistemaTecnico.Models;

namespace SistemaTecnico.Repositories
{
    public interface IPresupuestoRepository
    {
        Task<Presupuesto?> ObtenerPorIdAsync(int id);
        Task<List<Presupuesto>> ObtenerPorTrabajoAsync(int trabajoId);
        Task<Presupuesto> ObtenerPorTrabajoYTecnicoAsync(int trabajoId, int tecnicoId);
        Task<List<Presupuesto>> ObtenerPorTecnicoAsync(int tecnicoId);
        Task AgregarAsync(Presupuesto presupuesto);
        Task GuardarCambiosAsync();
        Task<Presupuesto?> ObtenerAprobadoPorTrabajoAsync(int idTrabajo);
    }
}
