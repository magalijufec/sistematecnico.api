using SistemaTecnico.Data;
using SistemaTecnico.DTO;

namespace SistemaTecnico.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> ObtenerDashboardAsync()
        {
            var hoy = DateTime.Today;

            var inicioMes = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1);

            return new DashboardResponseDto
            {
                Pendientes = _context.Trabajos
                    .Count(x => x.Estado.Id == 1),

                EnProceso = _context.Trabajos
                    .Count(x => x.Estado.Id == 2),

                PendientePago = _context.Trabajos
                    .Count(x => x.Estado.Id == 3),

                Finalizados = _context.Trabajos
                    .Count(x => x.Estado.Id == 4),

                TotalTrabajos = _context.Trabajos.Count(),

                TotalClientes = _context.Clientes.Count(),

                TotalTecnicos = _context.Usuarios
                    .Count(x => x.Perfil.Id == 2),

                TrabajosHoy = _context.Trabajos
                    .Count(x => x.FechaSolicitud.Date == hoy),

                TrabajosMes = _context.Trabajos
                    .Count(x => x.FechaSolicitud >= inicioMes)
            };
        }
    }
}