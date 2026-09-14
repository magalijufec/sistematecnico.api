using SistemaTecnico.Data;
using SistemaTecnico.DTO;
using SistemaTecnico.Models;

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
            var hoy = DateTime.UtcNow.Date;

            var inicioMes = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            );
            var manana = hoy.AddDays(1);

            return new DashboardResponseDto
            {
                Pendientes = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.PendienteRevisionSector),

                EnProceso = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.EnProceso),

                TrabajosFinalizados = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.Finalizado),

                Aprobados = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.Aprobado),

                PendientePago = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.PendientePago),

                Pagados = _context.Trabajos
                    .Count(x => x.Estado.Id == EstadosTrabajo.Finalizado),

                TotalTrabajos = _context.Trabajos.Count(),

                TotalClientes = _context.Clientes.Count(),

                TotalTecnicos = _context.Usuarios
                    .Count(x => x.Perfil.Id == Perfiles.Tecnico && x.Activo),


                TrabajosHoy = _context.Trabajos
                    .Count(x =>
                        x.FechaSolicitud >= hoy &&
                        x.FechaSolicitud < manana),

                TrabajosMes = _context.Trabajos
                    .Count(x => x.FechaSolicitud >= inicioMes)
            };
        }
    }
}